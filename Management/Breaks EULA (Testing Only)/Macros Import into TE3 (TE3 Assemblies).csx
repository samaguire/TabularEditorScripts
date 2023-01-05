#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/* TODO

- Update to the new logic used in the standard export/import

*/

//  *** Warning! ***  this will clear all existing macros in Tabular Editor 3
//  *** Warning! ***  this will clear all existing macros in Tabular Editor 3
//  *** Warning! ***  this will clear all existing macros in Tabular Editor 3

var inFolder = @".\CollectionTE3";

// Check folder path exists
if (!Directory.Exists(inFolder)) { return; }

// Pull details from csx and json files
var jsonArray = new JArray();
foreach (var filePath in Directory.EnumerateFiles(inFolder, "*.csx", SearchOption.AllDirectories))
{

    // Extract macros, removing C# scripting environment modifications
    var csxContent = String.Join("\n", File.ReadAllLines(filePath).Skip(15))
                        .Replace("ScriptHost.", string.Empty)
                        .Replace("// #r", "#r")
                        .Trim('\n');
    var jsonContent = (JObject.Parse(File.ReadAllText(filePath.Replace(".csx", ".json"))));

    // Recreate json objects of macros and build array
    jsonContent.Add("Execute", csxContent);
    jsonArray.Add(jsonContent);

}

// Recreate 'Actions' object
var jsonObject = new JObject();
jsonObject.Add("Actions", jsonArray);

// Write json to Tabular Editor 3 settings file
var jsonFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor3\MacroActions.json";
var jsonContent = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);
File.WriteAllText(jsonFilePath, jsonContent, System.Text.Encoding.UTF8);