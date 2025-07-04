﻿#load "..\..\..\..\Management\Common Library.csx"
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

// https://github.com/microsoft/Analysis-Services/tree/master/BestPracticeRules

int maxLen = 100;
string annName = "LongLengthRowCount";

foreach (var c in Model.AllColumns.Where(a => a.DataType == DataType.String))
{

    string tableName = c.Table.Name;
    string columnName = c.Name;
    
    var obj = Model.Tables[tableName].Columns[columnName];
    var result = ScriptHelper.EvaluateDax("SUMMARIZECOLUMNS(\"test\",CALCULATE(COUNTROWS(DISTINCT('"+tableName+"'["+columnName+"])),LEN('"+tableName+"'["+columnName+"]) > "+maxLen+"))");
    
    obj.SetAnnotation(annName,result.ToString());
    
    if (obj.GetAnnotation(annName) == "Table")
    {
        obj.SetAnnotation(annName,"0");
    }

}