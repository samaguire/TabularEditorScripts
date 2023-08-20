#load "..\..\..\Management\Common Library.csx"
#load "..\..\..\Management\Custom Classes.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "Microsoft.VisualBasic"

using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.UI;
using TabularEditor.Scripting;
// *** The above namespaces are required for the C# scripting environment, remove in Tabular Editor ***
using Microsoft.VisualBasic;

var t = Selected.Table;

// check if annotation already exists because first phase has already been completed
if (t.GetAnnotation("Parent-child hierarchy") == null)
{

    // get EntityKey from user
    var cEntityKey = ScriptHelper.SelectColumn(
        label: "Select 'EntityKey' column:",
        table: t
    );
    if (cEntityKey == null) { return; }

    // get EntityParentKey from user
    var cEntityParentKey = ScriptHelper.SelectColumn(
        label: "Select 'EntityParentKey' column:",
        table: t
    );
    if (cEntityParentKey == null) { return; }

    // get EntityName from user
    var cEntityName = ScriptHelper.SelectColumn(
        label: "Select 'EntityName' column:",
        table: t
    );
    if (cEntityName == null) { return; }

    // set datatype for use in PATHITEM()
    var dataTypeEntityKey = "";
    if (cEntityKey.DataType == DataType.Int64) { dataTypeEntityKey = "INTEGER"; } else { dataTypeEntityKey = "TEXT"; }

    // create ParentSafe column
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

    // create ParentMissing column
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

    // create Path column
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

    // create Detached column
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

    // create Depth column
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

    // save details to annotation
    t.SetAnnotation("Parent-child hierarchy", $"{cEntityKey.Name}|{cEntityParentKey.Name}|{cEntityName.Name}");

    // warn to save the model and re-run
    ScriptHelper.Warning("Please save the model to the server and re-run the script to continue.");

    // if a power bi desktop model, warn to refresh
    if (Model.Database.ServerVersion.Contains("Power BI Desktop"))
    {
        ScriptHelper.Warning("Power BI Desktop detected. Please also manually refresh when prompted.");
    }

    // exit script
    return;

}
else
{

    // trigger calculation refresh of the table if NOT a power bi desktop model
    if (!Model.Database.ServerVersion.Contains("Power BI Desktop"))
    {
        ScriptHelper.ExecuteCommand(
            tmslOrXmla: $"{{ \"refresh\": {{ \"type\": \"calculate\", \"objects\": [ {{ \"database\": \"{Model.Database.Name}\", \"table\": \"{t.Name}\"  }} ] }} }}",
            isXmla: false
            );
    }

    // get details from annotation
    var annotatedColumnNames = t.GetAnnotation("Parent-child hierarchy").Split('|');
    var cEntityKey = t.Columns[annotatedColumnNames[0]];
    var cEntityParentKey = t.Columns[annotatedColumnNames[1]];
    var cEntityName = t.Columns[annotatedColumnNames[2]];

    // set datatype for use in PATHITEM()
    var dataTypeEntityKey = "";
    if (cEntityKey.DataType == DataType.Int64) { dataTypeEntityKey = "INTEGER"; } else { dataTypeEntityKey = "TEXT"; }

    // set previously created objects
    var cParentSafe = t.Columns[cEntityKey.Name + "ParentSafe"];
    var cParentMissing = t.Columns[cEntityKey.Name + "ParentMissing"];
    var cPath = t.Columns[cEntityKey.Name + "Path"];
    var cDetached = t.Columns[cEntityKey.Name + "Detached"];
    var cDepth = t.Columns[cEntityKey.Name + "Depth"];

    // create hierarchy
    var nameLevels = cEntityName.Name + " Hierarchy";
    foreach (var h in t.Hierarchies.Where(x => x.Name == nameLevels).ToList()) { h.Delete(); }
    var hLevels = t.AddHierarchy(
        name: nameLevels
    );

    // setup hashset for generating daxBrowseDepth measure expression
    var daxBrowseDepthHashSet = new HashSet<string>();

    // get max hierarchy depth
    var maxDepth = Convert.ToInt64(ScriptHelper.EvaluateDax($"VAR maxValue = MAX( {cDepth.DaxObjectFullName} ) RETURN IF( maxValue = BLANK( ), 0, maxValue )"));

    // loop for each level and create columns
    for (int i = 1; i <= maxDepth; i++)
    {

        // set column details
        var nameLevel = cEntityName.Name + $" Level {i}";
        var daxLevel = @"
            IF(
                NOT <cDetached>,
                VAR LevelNumber = <i>
                VAR LevelKey =
                    PATHITEM( <cPath>, LevelNumber, <dataTypeEntityKey> )
                VAR Result =
                    LOOKUPVALUE( <cEntityName>, <cEntityKey>, LevelKey )
                RETURN
                    Result
            )"
            .Replace("<cDetached>", cDetached.DaxObjectFullName)
            .Replace("<i>", i.ToString())
            .Replace("<cPath>", cPath.DaxObjectFullName)
            .Replace("<dataTypeEntityKey>", dataTypeEntityKey)
            .Replace("<cEntityName>", cEntityName.DaxObjectFullName)
            .Replace("<cEntityKey>", cEntityKey.DaxObjectFullName);

        // remove column if it exists
        foreach (var c in t.Columns.Where(x => x.Name == nameLevel).ToList()) { c.Delete(); }

        // add hierarchy column
        var cLevel = t.AddCalculatedColumn(
            name: nameLevel,
            expression: daxLevel
        );
        cLevel.IsHidden = true;

        // add column to hierarchy
        hLevels.AddLevel(
            column: cLevel,
            levelName: nameLevel
        );

        // add column to hashset
        daxBrowseDepthHashSet.Add(
            $"ISINSCOPE( {cLevel.DaxObjectFullName} )"
        );

    }

    // create BrowseDepth measure
    var nameBrowseDepth = cEntityName.Name + " Browse Depth";
    var daxBrowseDepth = string.Join(Environment.NewLine + "+ ", daxBrowseDepthHashSet);
    foreach (var m in t.Measures.Where(x => x.Name == nameBrowseDepth).ToList()) { m.Delete(); }
    var mBrowseDepth = t.AddMeasure(
        name: nameBrowseDepth,
        expression: daxBrowseDepth
    );
    mBrowseDepth.IsHidden = true;
    
    // remove annotation
    t.RemoveAnnotation("Parent-child hierarchy");

}

ScriptHelper.Info("Script finished.");