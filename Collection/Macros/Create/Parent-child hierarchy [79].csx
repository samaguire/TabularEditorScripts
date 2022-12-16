#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.12\ref\net6.0\System.Windows.Forms.dll"
#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.Shared;
using TabularEditor.Shared.Scripting;
using TabularEditor.Shared.Interaction;
using TabularEditor.Shared.Services;

/*** Everything ABOVE this point is required for the C# scripting environment, remove in TE3 ***/

var t = ScriptHost.Selected.Table;

if (t.GetAnnotation("Parent-child hierarchy") == null)
{

    var cEntityKey = ScriptHost.SelectColumn(
        label: "Select 'EntityKey' column:",
        table: t
    );
    if (cEntityKey == null) { return; }

    var cEntityParentKey = ScriptHost.SelectColumn(
        label: "Select 'EntityParentKey' column:",
        table: t
    );
    if (cEntityParentKey == null) { return; }

    var cEntityName = ScriptHost.SelectColumn(
        label: "Select 'EntityName' column:",
        table: t
    );
    if (cEntityName == null) { return; }

    var dataTypeEntityKey = "";
    if (cEntityKey.DataType == DataType.Int64) { dataTypeEntityKey = "INTEGER"; } else { dataTypeEntityKey = "TEXT"; }

    // Create ParentSafe column

    var nameParentSafe = cEntityKey.Name + "ParentSafe";

    var daxParentSafe = @"
        VAR ParentId = <cEntityParentKey>
        VAR Result =
            IF(
                NOT ISEMPTY( FILTER( <t>, <cEntityKey> = ParentId ) ),
                <cEntityParentKey>
            )
        RETURN
            Result"
        .Replace("<t>", t.DaxTableName)
        .Replace("<cEntityKey>", cEntityKey.DaxObjectFullName)
        .Replace("<cEntityParentKey>", cEntityParentKey.DaxObjectFullName);

    foreach (var c in t.Columns.Where(x => x.Name == nameParentSafe).ToList()) { c.Delete(); }

    var cParentSafe = t.AddCalculatedColumn(
        name: nameParentSafe,
        expression: daxParentSafe
    );

    cParentSafe.IsHidden = true;

    // Create ParentMissing column

    var nameParentMissing = cEntityKey.Name + "ParentMissing";

    var daxParentMissing = @"
        NOT <cEntityParentKey> == <cParentSafe>"
        .Replace("<cEntityParentKey>", cEntityParentKey.DaxObjectFullName)
        .Replace("<cParentSafe>", cParentSafe.DaxObjectFullName);

    foreach (var c in t.Columns.Where(x => x.Name == nameParentMissing).ToList()) { c.Delete(); }

    var cParentMissing = t.AddCalculatedColumn(
        name: nameParentMissing,
        expression: daxParentMissing
    );

    cParentMissing.IsHidden = true;

    // Create Path column

    var namePath = cEntityKey.Name + "Path";

    var daxPath = @"
        PATH( <cEntityKey>, <cParentSafe> )"
        .Replace("<cEntityKey>", cEntityKey.DaxObjectFullName)
        .Replace("<cParentSafe>", cParentSafe.DaxObjectFullName);

    foreach (var c in t.Columns.Where(x => x.Name == namePath).ToList()) { c.Delete(); }

    var cPath = t.AddCalculatedColumn(
        name: namePath,
        expression: daxPath
    );

    cPath.IsHidden = true;

    // Create Detached column

    var nameDetached = cEntityKey.Name + "Detached";

    var daxDetached = @"
        VAR LevelKey = PATHITEM( <cPath>, 1, <dataTypeEntityKey> )
        VAR LevelParent =
            LOOKUPVALUE(
                <cEntityParentKey>,
                <cEntityKey>,
                LevelKey
            )
        VAR Result = NOT LevelParent = BLANK( )
        RETURN
            Result"
        .Replace("<cPath>", cPath.DaxObjectFullName)
        .Replace("<cEntityParentKey>", cEntityParentKey.DaxObjectFullName)
        .Replace("<cEntityKey>", cEntityKey.DaxObjectFullName)
        .Replace("<dataTypeEntityKey>", dataTypeEntityKey);

    foreach (var c in t.Columns.Where(x => x.Name == nameDetached).ToList()) { c.Delete(); }

    var cDetached = t.AddCalculatedColumn(
        name: nameDetached,
        expression: daxDetached
    );

    cDetached.IsHidden = true;

    // Create Depth column

    var nameDepth = cEntityKey.Name + "Depth";

    var daxDepth = @"
        IF(
            NOT <cDetached>,
            PATHLENGTH( <cPath> )
        )"
        .Replace("<cDetached>", cDetached.DaxObjectFullName)
        .Replace("<cPath>", cPath.DaxObjectFullName);

    foreach (var c in t.Columns.Where(x => x.Name == nameDepth).ToList()) { c.Delete(); }

    var cDepth = t.AddCalculatedColumn(
        name: nameDepth,
        expression: daxDepth
    );

    cDepth.IsHidden = true;

    t.SetAnnotation("Parent-child hierarchy", $"{cEntityKey.Name}|{cEntityParentKey.Name}|{cEntityName.Name}");

    ScriptHost.Warning("Please save the model to the server and re-run the script to continue.");

    if (ScriptHost.Model.Database.ServerVersion.Contains("Power BI Desktop"))
    {
        ScriptHost.Warning("Power BI Desktop detected. Please also manually refresh when prompted.");
    }

    return;

}
else
{

    if (!ScriptHost.Model.Database.ServerVersion.Contains("Power BI Desktop"))
    {
        ScriptHost.ExecuteCommand(
            tmslOrXmla: $"{{ \"refresh\": {{ \"type\": \"calculate\", \"objects\": [ {{ \"database\": \"{Model.Database.Name}\", \"table\": \"{t.Name}\"  }} ] }} }}",
            isXmla: false
            );
    }

    var annotatedColumnNames = t.GetAnnotation("Parent-child hierarchy").Split('|');

    var cEntityKey = t.Columns[annotatedColumnNames[0]];
    var cEntityParentKey = t.Columns[annotatedColumnNames[1]];
    var cEntityName = t.Columns[annotatedColumnNames[2]];

    var dataTypeEntityKey = "";
    if (cEntityKey.DataType == DataType.Int64) { dataTypeEntityKey = "INTEGER"; } else { dataTypeEntityKey = "TEXT"; }

    var cParentSafe = t.Columns[cEntityKey.Name + "ParentSafe"];
    var cParentMissing = t.Columns[cEntityKey.Name + "ParentMissing"];
    var cPath = t.Columns[cEntityKey.Name + "Path"];
    var cDetached = t.Columns[cEntityKey.Name + "Detached"];
    var cDepth = t.Columns[cEntityKey.Name + "Depth"];

    // Create Level columns and hierarchy

    var nameLevels = cEntityName.Name + " Hierarchy";

    foreach (var h in t.Hierarchies.Where(x => x.Name == nameLevels).ToList()) { h.Delete(); }

    var hLevels = t.AddHierarchy(
        name: nameLevels
    );

    var maxDepth = Convert.ToInt64(ScriptHost.EvaluateDax($"VAR maxValue = MAX( {cDepth.DaxObjectFullName} ) RETURN IF( maxValue = BLANK( ), 0, maxValue )"));

    for (int i = 1; i <= maxDepth; i++)
    {

        var nameLevel = cEntityName.Name + $" Level {i}";

        var daxLevel = @"
            IF(
                NOT <cDetached>,
                VAR LevelNumber = <i>
                VAR LevelKey =
                    PATHITEM( <cPath>, LevelNumber, <dataTypeEntityKey> )
                VAR Result =
                    LOOKUPVALUE( {cEntityName}, <cEntityKey>, LevelKey )
                RETURN
                    Result
            )"
            .Replace("<cDetached>", cDetached.DaxObjectFullName)
            .Replace("<i>", i.ToString())
            .Replace("<cPath>", cPath.DaxObjectFullName)
            .Replace("<dataTypeEntityKey>", dataTypeEntityKey)
            .Replace("{cEntityName}", cEntityName.DaxObjectFullName)
            .Replace("<cEntityKey>", cEntityKey.DaxObjectFullName);

        foreach (var c in t.Columns.Where(x => x.Name == nameLevel).ToList()) { c.Delete(); }

        var cLevel = t.AddCalculatedColumn(
            name: nameLevel,
            expression: daxLevel
        );

        cLevel.IsHidden = true;

        hLevels.AddLevel(
            column: cLevel,
            levelName: $"Level {i}"
        );

    }

    t.RemoveAnnotation("Parent-child hierarchy");

}

ScriptHost.Info("Script finished.");