#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
#r "nuget: Newtonsoft.Json, 13.0.1"

using TabularEditor.Shared.Scripting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//  *** Warning! ***  this will clear all existing content in the 'outFolder' folder
//  *** Warning! ***  this will clear all existing content in the 'outFolder' folder
//  *** Warning! ***  this will clear all existing content in the 'outFolder' folder

var outFolder = @".\CollectionTest";

// Check folder path and clear existing files and empty directories
if (!Directory.Exists(outFolder)) { Directory.CreateDirectory(outFolder); }
foreach (var f in Directory.EnumerateFiles(outFolder, "*.csx", SearchOption.AllDirectories)) { File.Delete(f); }
foreach (var f in Directory.EnumerateFiles(outFolder, "*.json", SearchOption.AllDirectories)) { File.Delete(f); }
foreach (var d in Directory.EnumerateDirectories(outFolder, "*", SearchOption.AllDirectories))
{
    if (!Directory.EnumerateFiles(d).Any() && !Directory.EnumerateDirectories(d).Any()) { Directory.Delete(d); }
}

// Define C# scripting environment
var winFormsPath = Directory.GetFiles(@"C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref", "System.Windows.Forms.dll", SearchOption.AllDirectories)[0];
var scriptingEnvironment = string.Format(@"#r ""{0}""
#r ""C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll""
#r ""C:\Program Files\Tabular Editor 3\TOMWrapper.dll""
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.Shared;
using TabularEditor.Shared.Scripting;
using TabularEditor.Shared.Interaction;
using TabularEditor.Shared.Services;

/*** Everything ABOVE this point is required for the C# scripting environment, remove in TE3 ***/
", winFormsPath);

// Load MacroActions
var jsonFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor3\MacroActions.json";
var json = JObject.Parse(File.ReadAllText(jsonFile));

// Export each MacroAction
foreach (var jtokenItem in json["Actions"])
{

    // Generate filename without extension and relataive path
    var fileName = string.Join("_", jtokenItem["Name"].Value<string>().Replace('\\', '~').Split(Path.GetInvalidFileNameChars())).Replace('~', '\\') + " [" + jtokenItem["Id"].Value<string>() + "]";

    // Get csxContent and adapt to C# scripting environment
    var csxContent = "\n" + jtokenItem["Execute"].Value<string>();
    var scripthostMethods = typeof(ScriptHost).GetMethods().Select(x => x.Name.Replace("get_", "").Replace("set_", "")).Distinct().ToList();
    foreach (var item in scripthostMethods)
    {
        csxContent = csxContent
            .Replace(" " + item, " ScriptHost." + item)
            .Replace("(" + item, "(ScriptHost." + item)
            .Replace("{" + item, "{ScriptHost." + item)
            .Replace("!" + item, "!ScriptHost." + item)
            .Replace("\n" + item, "\nScriptHost." + item);
    }
    csxContent = csxContent.Replace("ScriptHelper", "ScriptHost").Replace("#r", "// #r");

    // Save csxContent (and create directory)
    var csxFilePath = outFolder + @"\" + fileName + ".csx";
    if (!Directory.Exists(Path.GetDirectoryName(csxFilePath))) { Directory.CreateDirectory(Path.GetDirectoryName(csxFilePath)); }
    File.WriteAllText(csxFilePath, scriptingEnvironment + csxContent, System.Text.Encoding.UTF8);

    // Save jsonContent
    jtokenItem["Execute"].Parent.Remove();
    var jsonContent = JsonConvert.SerializeObject(jtokenItem, Newtonsoft.Json.Formatting.Indented);
    var jsonFilePath = outFolder + @"\" + fileName + ".json";
    File.WriteAllText(jsonFilePath, jsonContent, System.Text.Encoding.UTF8);

}