#load "..\..\..\..\Management\Common Library.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "System.Net.Http"

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
using System.IO;

// https://github.com/microsoft/Analysis-Services/tree/master/BestPracticeRules

var url = "https://raw.githubusercontent.com/microsoft/Analysis-Services/master/BestPracticeRules/BPARules.json";
var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
var jsonPath = String.Empty;
var version = typeof(Model).Assembly.GetName().Version.Major;

switch (version)
{

    case 3:
        jsonPath = path + @"\TabularEditor3\BPARules.json";
        break;

    case 2:
        jsonPath = path + @"\TabularEditor\BPARules.json";
        break;

    default:
        ScriptHelper.Error("Couldn't identify the version of Tabular Editor: " + version);
        return;

}

using (var client = new System.Net.Http.HttpClient())
{
    var responseBody = client.GetStringAsync(url).GetAwaiter().GetResult();
    File.WriteAllText(jsonPath, responseBody, System.Text.Encoding.UTF8);
}