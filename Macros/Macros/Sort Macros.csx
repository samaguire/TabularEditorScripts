#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***


var jsonFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor3\MacroActions.json";
var json = JObject.Parse(File.ReadAllText(jsonFile));
json["Actions"] = new JArray(json["Actions"].OrderBy(i => i["Name"]));
File.WriteAllText(jsonFile, JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.Indented), System.Text.Encoding.UTF8);
ScriptHelper.Warning("Script finished. Please restart Tabular Editor 3.");
