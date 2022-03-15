#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "System.Xml"

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***
using System.Xml;

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

// https://github.com/microsoft/Analysis-Services/tree/master/BestPracticeRules



// Split Date/Time recommendation
string annName = "DateTimeWithHourMinSec";
foreach (var c in Model.AllColumns.Where(a => a.DataType == DataType.DateTime))
{
    string columnName = c.Name;
    string tableName = c.Table.Name;
    var obj = Model.Tables[tableName].Columns[columnName];
    
    var result = ExecuteDax("EVALUATE TOPN(5,SUMMARIZECOLUMNS('"+tableName+"'["+columnName+"]))").Tables[0];

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

Info("Script finished.");
