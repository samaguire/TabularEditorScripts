#load "..\..\..\Management\Common Library.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***

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

var t = Selected.Table;

// check if annotation already exists because first phase has already been completed
if (t.GetAnnotation("Parent-child hierarchy") == null)
{

    // get EntityKey from user
    var cEntityKey = ScriptHelper.SelectColumn(
        label: "Select 'EntityKey' column:",
        columns: t.Columns.OrderBy(c => c.Name)
    );
    if (cEntityKey == null) { return; }

    // get EntityParentKey from user
    var cEntityParentKey = ScriptHelper.SelectColumn(
        label: "Select 'EntityParentKey' column:",
        columns: t.Columns.OrderBy(c => c.Name)
    );
    if (cEntityParentKey == null) { return; }

    // get EntityName from user
    var cEntityName = ScriptHelper.SelectColumn(
        label: "Select 'EntityName' column:",
        columns: t.Columns.OrderBy(c => c.Name)
    );
    if (cEntityName == null) { return; }

    // set datatype for use in PATHITEM()
    var dataTypeEntityKey = String.Empty;
    if (cEntityKey.DataType == DataType.Int64) { dataTypeEntityKey = "INTEGER"; } else { dataTypeEntityKey = "TEXT"; }

    // create ParentCount column
    var nameParentCount = cEntityKey.Name + " ParentCount";
    var daxParentCount = @"
VAR _ParentId = <cEntityParentKey>
RETURN
    COALESCE ( CALCULATE ( COUNTROWS ( <t> ), REMOVEFILTERS (), <cEntityKey> = _ParentId ), 0 )"
        .Replace("<cEntityParentKey>", cEntityParentKey.DaxObjectFullName)
        .Replace("<t>", t.DaxObjectFullName)
        .Replace("<cEntityKey>", cEntityKey.DaxObjectFullName);
    foreach (var c in t.Columns.Where(x => x.Name == nameParentCount).ToList()) { c.Delete(); }
    var cParentCount = t.AddCalculatedColumn(
        name: nameParentCount,
        expression: daxParentCount
    );
    cParentCount.IsHidden = true;

    // create ParentSafe column
    var nameParentSafe = cEntityKey.Name + " ParentSafe";
    var daxParentSafe = @"
IF ( <cParentCount> > 0, <cEntityParentKey> )"
        .Replace("<cParentCount>", cParentCount.DaxObjectFullName)
        .Replace("<cEntityParentKey>", cEntityParentKey.DaxObjectFullName);
    foreach (var c in t.Columns.Where(x => x.Name == nameParentSafe).ToList()) { c.Delete(); }
    var cParentSafe = t.AddCalculatedColumn(
        name: nameParentSafe,
        expression: daxParentSafe
    );
    cParentSafe.IsHidden = true;

    // create Path column
    var namePath = cEntityKey.Name + " Path";
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

    // create Depth column
    var nameDepth = cEntityKey.Name + " Depth";
    var daxDepth = @"
PATHLENGTH( <cPath> )"
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
    var dataTypeEntityKey = String.Empty;
    if (cEntityKey.DataType == DataType.Int64) { dataTypeEntityKey = "INTEGER"; } else { dataTypeEntityKey = "TEXT"; }

    // set previously created objects
    var cParentCount = t.Columns[cEntityKey.Name + " ParentCount"];
    var cParentSafe = t.Columns[cEntityKey.Name + " ParentSafe"];
    var cPath = t.Columns[cEntityKey.Name + " Path"];
    var cDepth = t.Columns[cEntityKey.Name + " Depth"];

    // create hierarchy
    var nameLevels = cEntityName.Name + " Hierarchy";
    foreach (var h in t.Hierarchies.Where(x => x.Name == nameLevels).ToList()) { h.Delete(); }
    var hLevels = t.AddHierarchy(
        name: nameLevels
    );

    // setup hashset for generating daxBrowseDepth measure expression
    var daxBrowseDepthHashSet = new HashSet<string>();

    // get max hierarchy depth
    var maxDepth = Convert.ToInt64(ScriptHelper.EvaluateDax($"VAR maxValue = MAX( {cDepth.DaxObjectFullName} ) RETURN IF( maxValue = BLANK(), 0, maxValue )"));

    // loop for each level and create columns
    for (int i = 1; i <= maxDepth; i++)
    {

        // set column details
        var nameLevel = cEntityName.Name + $" Level {i}";
        var daxLevel = @"
VAR _LevelNumber = <i>
RETURN
    IF (
        <cDepth> >= _LevelNumber,
        IF (
            <cDepth> = 1 || <cParentCount> = 1,
            LOOKUPVALUE (
                <cEntityName>,
                <cEntityKey>,
                PATHITEM ( <cPath>, _LevelNumber, <dataTypeEntityKey> ),
                <cEntityName>
            )
        )
    )"
            .Replace("<i>", i.ToString())
            .Replace("<cDepth>", cDepth.DaxObjectFullName)
            .Replace("<cParentCount>", cParentCount.DaxObjectFullName)
            .Replace("<cEntityName>", cEntityName.DaxObjectFullName)
            .Replace("<cEntityKey>", cEntityKey.DaxObjectFullName)
            .Replace("<cPath>", cPath.DaxObjectFullName)
            .Replace("<dataTypeEntityKey>", dataTypeEntityKey);

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
    var nameBrowseDepth = cEntityName.Name + " BrowseDepth";
    var daxBrowseDepth = Environment.NewLine + string.Join(Environment.NewLine + "+ ", daxBrowseDepthHashSet);
    foreach (var m in t.Measures.Where(x => x.Name == nameBrowseDepth).ToList()) { m.Delete(); }
    var mBrowseDepth = t.AddMeasure(
        name: nameBrowseDepth,
        expression: daxBrowseDepth
    );
    mBrowseDepth.IsHidden = true;

    // create RowDepth measure
    var nameRowDepth = cEntityName.Name + " RowDepth";
    var daxRowDepth = @"
MAX ( <cDepth> )"
        .Replace("<cDepth>", cDepth.DaxObjectFullName);
    foreach (var m in t.Measures.Where(x => x.Name == nameRowDepth).ToList()) { m.Delete(); }
    var mRowDepth = t.AddMeasure(
        name: nameRowDepth,
        expression: daxRowDepth
    );
    mRowDepth.IsHidden = true;

    // remove annotation
    t.RemoveAnnotation("Parent-child hierarchy");

}