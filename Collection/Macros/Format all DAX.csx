#load "..\..\Management\Common Library.csx"
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

void FormatDaxObjects(IEnumerable<IDaxDependantObject> daxObjects) {
    daxObjects.FormatDax(shortFormat: true, skipSpaceAfterFunctionName: false);
}

FormatDaxObjects(Model.AllMeasures);
FormatDaxObjects(Model.Tables.Where(x => !x.Name.StartsWith("DateTableTemplate") && !x.Name.StartsWith("LocalDateTable") && !x.Name.StartsWith("DateAutoTemplate")));
FormatDaxObjects(Model.AllColumns.OfType<CalculatedColumn>().Where(x => !x.DaxTableName.StartsWith("DateTableTemplate") && !x.DaxTableName.StartsWith("LocalDateTable") && !x.Name.StartsWith("DateAutoTemplate")));
FormatDaxObjects(Model.AllCalculationItems);
FormatDaxObjects(Model.Roles.SelectMany(r => r.TablePermissions));
FormatDaxObjects(Model.Functions);