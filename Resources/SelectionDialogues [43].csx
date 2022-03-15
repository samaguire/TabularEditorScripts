#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

// Generic methods:

//T SelectObject<T>(this IEnumerable<T> columns, T preselect = null, string label = "Select object:")
//    where T : TabularNamedObject;
//IList<T> SelectObjects<T>(this IEnumerable<T> objects, IEnumerable<T> preselect = null, string label = "Select object(s):")
//    where T : TabularNamedObject;

// Specific methods:

//Table SelectTable(Table preselect = null, string label = "Select table:");
//Table SelectTable(this IEnumerable<Table> tables, Table preselect = null, string label = "Select table:");

//Column SelectColumn(this Table table, Column preselect = null, string label = "Select column:");
//Column SelectColumn(this IEnumerable<Column> columns, Column preselect = null, string label = "Select column:");

//Measure SelectMeasure(Measure preselect = null, string label = "Select measure:");
//Measure SelectMeasure(this Table table, Measure preselect = null, string label = "Select measure:");
//Measure SelectMeasure(this IEnumerable<Measure> measures, Measure preselect = null, string label = "Select measure:");

SelectObject(new TabularNamedObject[] { Model.Tables["Account"], (Model.Tables["Dates"].Columns["Date"] as DataColumn), Model.Tables["Transactional"].Measures["Total Actual COGS"] });
