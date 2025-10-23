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

void FormatObjects(IEnumerable<IDaxDependantObject> daxObjects) {
    ScriptHelper.FormatDax(objects: daxObjects, shortFormat: true, skipSpaceAfterFunctionName: false);
}

FormatObjects(Model.AllMeasures);
FormatObjects(Model.Tables.Where(x => !x.Name.StartsWith("DateTableTemplate") && !x.Name.StartsWith("LocalDateTable") && x.Name != "DateAutoTemplate"));
FormatObjects(Model.AllColumns.OfType<CalculatedColumn>().Where(x => !x.DaxTableName.StartsWith("DateTableTemplate") && !x.DaxTableName.StartsWith("LocalDateTable") && x.Name != "DateAutoTemplate"));
FormatObjects(Model.AllCalculationItems);
FormatObjects(Model.Roles.SelectMany(r => r.TablePermissions));
FormatObjects(Model.Functions);