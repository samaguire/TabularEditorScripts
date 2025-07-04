#load "..\Management\Common Library.csx"

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//  *** Warning! ***  this will clear all existing macros in Tabular Editor / Tabular Editor 3
//  *** Warning! ***  this will clear all existing macros in Tabular Editor / Tabular Editor 3
//  *** Warning! ***  this will clear all existing macros in Tabular Editor / Tabular Editor 3

var collectionFolder = @".\Collection";
var TE3overTE2 = true;

// Check paths
if (!Directory.Exists(collectionFolder)) { return; }

var jsonFile = string.Empty;
var jsonFileV2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor\MacroActions.json";
var jsonFileV3 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor3\MacroActions.json";
if (Directory.Exists(Path.GetDirectoryName(jsonFileV2))) { jsonFile = jsonFileV2; }
if (Directory.Exists(Path.GetDirectoryName(jsonFileV3)) && TE3overTE2) { jsonFile = jsonFileV3; }
if (string.IsNullOrEmpty(jsonFile))
{
    Console.WriteLine("\"MacroActions.json\" location not found!");
    return;
}
else
{
    Console.WriteLine("Using \"" + jsonFile + "\"");
}

// Define C# scripting environment
var assemblyList = new List<string>()
{
    @"Common Library.csx""",
    "// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***"
};
var namespaceList = new List<string>()
{
    @"using System;",
    @"using System.Linq;",
    @"using System.Collections.Generic;",
    @"using Newtonsoft.Json;",
    @"using TabularEditor;",
    @"using TabularEditor.TOMWrapper;",
    @"using TabularEditor.TOMWrapper.Utils;",
    @"using TabularEditor.UI;",
    @"using TabularEditor.Scripting;",
    @"// using TOM = Microsoft.AnalysisServices.Tabular;",
    @"// *** The above namespaces are required for the C# scripting environment, remove in Tabular Editor ***"
};

// Pull details from csx and json files
var jsonArray = new JArray();
int loopCounter = 0;
foreach (var filePath in Directory.EnumerateFiles(collectionFolder, "*.csx", SearchOption.AllDirectories))
{

    // Extract macro, removing C# scripting environment modifications
    var scriptBodyList = new List<string>(){};
    foreach (var line in File.ReadLines(filePath))
    {
        if (!((line.StartsWith("#") && assemblyList.Contains(line.Split('\\').Last())) || assemblyList.Contains(line)) && !namespaceList.Contains(line)) { scriptBodyList.Add(line); }
    }
    var csxContent = String.Join(Environment.NewLine, scriptBodyList)
                        .Replace("ScriptHelper.", string.Empty)
                        .Trim('\r', '\n');

    // Build MacroAction and add to 'Actions' json array
    var jsonContent = (JObject.Parse(File.ReadAllText(filePath.Replace(".csx", ".json"))));
    jsonContent["Id"] = loopCounter;
    jsonContent.Add("Execute", csxContent);
    jsonArray.Add(jsonContent);

    loopCounter++;

}

// Write MacroActions
var jsonObject = new JObject();
jsonObject.Add("Actions", jsonArray);
var jsonContent = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);
File.WriteAllText(jsonFile, jsonContent, System.Text.Encoding.UTF8);