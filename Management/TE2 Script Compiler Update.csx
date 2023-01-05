#r "TabularEditor.dll"
#r "nuget: Microsoft.Net.Compilers.Toolset, 4.4.0"

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

var jsonFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor\Preferences.json";
var json = JObject.Parse(File.ReadAllText(jsonFile));

json["ScriptCompilerDirectoryPath"] = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.nuget\packages\microsoft.net.compilers.toolset\3.1.0\tasks\net472";
json["ScriptCompilerOptions"] = "-langversion:8.0";

File.WriteAllText(jsonFile, JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.Indented), System.Text.Encoding.UTF8);