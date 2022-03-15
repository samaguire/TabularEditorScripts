#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

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

ScriptHelper.Info("Script finished.");
