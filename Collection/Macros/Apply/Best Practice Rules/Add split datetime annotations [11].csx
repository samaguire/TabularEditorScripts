#load "..\..\..\..\Management\Common Library.csx"
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
using System.Xml;

// https://github.com/microsoft/Analysis-Services/tree/master/BestPracticeRules

// Split Date/Time recommendation
string annName = "DateTimeWithHourMinSec";
foreach (var c in Model.AllColumns.Where(a => a.DataType == DataType.DateTime))
{
    string columnName = c.Name;
    string tableName = c.Table.Name;
    var obj = Model.Tables[tableName].Columns[columnName];
    
    var result = ScriptHelper.ExecuteDax("EVALUATE TOPN(5,SUMMARIZECOLUMNS('"+tableName+"'["+columnName+"]))").Tables[0];

    for (int r = 0; r < result.Rows.Count; r++)
    {
        string resultValue = result.Rows[r][0].ToString();
        if (!resultValue.EndsWith("12:00:00 AM"))
        {
            obj.SetAnnotation(annName,"1");
            r=50;
        }
        else
        {
            obj.SetAnnotation(annName,"0");
        }
    }
}