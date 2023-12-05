#load "..\..\..\Management\Common Library.csx"
#load "..\..\..\Management\Custom Classes.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "System.Globalization"

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
using System.Globalization;

CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
TextInfo textInfo = cultureInfo.TextInfo;

string ReplaceUnderscoreAndCapitalize(string input) => textInfo.ToTitleCase(input.Replace("_", " "));

foreach (var c in Model.AllColumns) { c.Name = ReplaceUnderscoreAndCapitalize(c.Name); }
foreach (var t in Model.Tables) { t.Name = ReplaceUnderscoreAndCapitalize(t.Name); }