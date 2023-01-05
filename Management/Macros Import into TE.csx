#r "TabularEditor.dll"

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//  *** Warning! ***  this will clear all existing macros in Tabular Editor / Tabular Editor 3
//  *** Warning! ***  this will clear all existing macros in Tabular Editor / Tabular Editor 3
//  *** Warning! ***  this will clear all existing macros in Tabular Editor / Tabular Editor 3

var inFolder = @".\Collection";
var TE3overTE2 = true;

// Check folder path exists
if (!Directory.Exists(inFolder)) { return; }

// Define C# scripting environment
var assemblyList = new List<string>()
{
    @"#r """ + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Tabular Editor\TabularEditor.exe""",
    @"#r """ + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor\TOMWrapper14.dll""",
    $"#r \"{Directory.GetFiles(@"C:\Windows\Microsoft.NET\Framework", "System.Windows.Forms.dll", SearchOption.AllDirectories)[0]}\"",
    "// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***"
};
var namespaceList = new List<string>()
{
    "using System;",
    "using System.Linq;",
    "using System.Collections.Generic;",
    "using Newtonsoft.Json;",
    "using TabularEditor;",
    "using TabularEditor.TOMWrapper;",
    "using TabularEditor.TOMWrapper.Utils;",
    "using TabularEditor.UI;",
    "using TabularEditor.Scripting;",
    // "using TOM = Microsoft.AnalysisServices.Tabular;",
    "// *** The above namespaces are required for the C# scripting environment, remove in Tabular Editor ***"
};
var classVariableList = new List<string>()
{
    "static readonly Model Model;",
    "static readonly UITreeSelection Selected;",
    "// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***"
};

// Pull details from csx and json files
var jsonArray = new JArray();
foreach (var filePath in Directory.EnumerateFiles(inFolder, "*.csx", SearchOption.AllDirectories))
{

    // Extract macro, removing C# scripting environment modifications
    var scriptBodyList = new List<string>();
    foreach (var line in File.ReadLines(filePath).Skip(4))
    {
        if (!assemblyList.Contains(line) && !namespaceList.Contains(line) && !classVariableList.Contains(line)) { scriptBodyList.Add(line); }
    }
    var csxContent = String.Join("\n", scriptBodyList)
                        .Replace("ScriptHelper.", string.Empty)
                        .Trim('\n');

    // Build MacroAction and add to 'Actions' json array
    var jsonContent = (JObject.Parse(File.ReadAllText(filePath.Replace(".csx", ".json"))));
    jsonContent.Add("Execute", csxContent);
    jsonArray.Add(jsonContent);

}

// Recreate 'Actions' object
var jsonObject = new JObject();
jsonObject.Add("Actions", jsonArray);

// Write MacroActions
var jsonFile = string.Empty;
var jsonFileV2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor\MacroActions.json";
var jsonFileV3 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor3\MacroActions.json";
if (TE3overTE2) { jsonFile = jsonFileV3; } else { jsonFile = jsonFileV2; }
Console.WriteLine("Using \"" + jsonFile + "\"");

var jsonContent = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);
File.WriteAllText(jsonFile, jsonContent, System.Text.Encoding.UTF8);