#load "..\Management\Common Library.csx"

#r "nuget: Newtonsoft.Json, 13.0.2"

using TabularEditor.Scripting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//  *** Warning! ***  this will clear all existing script content in the 'collectionFolder' folder
//  *** Warning! ***  this will clear all existing script content in the 'collectionFolder' folder
//  *** Warning! ***  this will clear all existing script content in the 'collectionFolder' folder

var collectionFolder = @".\Collection";
var TE3overTE2 = true;

// Load MacroActions
var jsonFile = string.Empty;
var jsonFileV2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor\MacroActions.json";
var jsonFileV3 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor3\MacroActions.json";

if (File.Exists(jsonFileV2)) { jsonFile = jsonFileV2; }
if (File.Exists(jsonFileV3) && TE3overTE2) { jsonFile = jsonFileV3; }
if (string.IsNullOrEmpty(jsonFile))
{
    Console.WriteLine("\"MacroActions.json\" location not found!");
    return;
}
else
{
    Console.WriteLine("Using \"" + jsonFile + "\"");
}

var json = JObject.Parse(File.ReadAllText(jsonFile));

// Check folder path and clear existing files and empty directories
if (!Directory.Exists(collectionFolder)) { Directory.CreateDirectory(collectionFolder); }
foreach (var f in Directory.EnumerateFiles(collectionFolder, "*.csx", SearchOption.AllDirectories)) { File.Delete(f); }
foreach (var f in Directory.EnumerateFiles(collectionFolder, "*.json", SearchOption.AllDirectories)) { File.Delete(f); }
foreach (var d in Directory.EnumerateDirectories(collectionFolder, "*", SearchOption.AllDirectories))
{
    if (!Directory.EnumerateFiles(d).Any() && !Directory.EnumerateDirectories(d).Any()) { Directory.Delete(d); }
}

// Export each MacroAction
foreach (var jtokenItem in json["Actions"])
{

    // Generate filename without extension and relataive path
    var fileName = string.Join("_", jtokenItem["Name"].Value<string>().Replace('\\', '~').Split(Path.GetInvalidFileNameChars())).Replace('~', '\\');
    // if (jsonFile == jsonFileV3) { fileName = fileName + " [" + jtokenItem["Id"].Value<string>() + "]"; } // ignored as causes git version control changes when macro ids are reset, however, the watchout is the duplicate names

    // Generate "Common Library.csx" load directive
    var relativeBasePath = string.Concat (Enumerable.Repeat (@"..\", collectionFolder.Count(x => x == '\\')));
    var relativeMacroPath = string.Concat (Enumerable.Repeat (@"..\", fileName.Count(x => x == '\\')));
    var loadCommonLibrary = @"#load """ + relativeBasePath + relativeMacroPath + @"Management\Common Library.csx""";
    var loadCustomClasses = @"#load """ + relativeBasePath + relativeMacroPath + @"Management\Custom Classes.csx""";

    // Get csxContent and adapt to C# scripting environment
    var csxContent = "\n" + jtokenItem["Execute"].Value<string>();

    // Prefix ScriptHelper to ScriptHelper methods
    var scripthelperMethods = typeof(ScriptHelper).GetMethods().Select(x => x.Name.Replace("get_", "").Replace("set_", "")).Distinct().ToList();
    foreach (var item in scripthelperMethods)
    {
        csxContent = csxContent
            .Replace(" " + item, " ScriptHelper." + item)
            .Replace("(" + item, "(ScriptHelper." + item)
            .Replace("{" + item, "{ScriptHelper." + item)
            .Replace("!" + item, "!ScriptHelper." + item)
            .Replace("\n" + item, "\nScriptHelper." + item);
    }

    // General cleanup
    csxContent = csxContent
        .Replace("ScriptHost", "ScriptHelper")
        .Replace("typeof(ScriptHelper.", "typeof(");

    // Define C# scripting environment
    var assemblyList = new List<string>()
    {
        loadCommonLibrary,
        loadCustomClasses,
        @"// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***"
    };
    var assemblyHashset = new HashSet<string>(assemblyList);
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
        @"// *** The above namespaces are required for the C# scripting environment, remove in Tabular Editor ***"
    };
    var namespaceHashset = new HashSet<string>(namespaceList);
    var scriptBodyList = new List<string>();

    // Deconstruct csxContent to C# scripting environment lists
    using (var reader = new StringReader(csxContent))
    {
        var line = "";
        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("#load ") || line.StartsWith("// #load "))
            {
                continue;
            }
            else if (line.StartsWith("#r "))
            {
                if (assemblyHashset.Add(line)) { assemblyList.Add(line); }
            }
            else if (line.StartsWith("using ") && !line.StartsWith("using ("))
            {
                if (namespaceHashset.Add(line)) { namespaceList.Add(line); }
            }
            else { scriptBodyList.Add(line); }
        }
    }

    // Reconstruct csxContent from lists
    csxContent = string.Join(Environment.NewLine, new List<string>()
    {
        string.Join(Environment.NewLine, assemblyList) + Environment.NewLine,
        string.Join(Environment.NewLine, namespaceList) + Environment.NewLine,
        string.Join(Environment.NewLine, scriptBodyList).Trim('\r', '\n')
            .Replace(Environment.NewLine + Environment.NewLine + Environment.NewLine, Environment.NewLine + Environment.NewLine)
    });

    // Save csxContent (and create directory)
    var csxFilePath = collectionFolder + @"\" + fileName + ".csx";
    if (!Directory.Exists(Path.GetDirectoryName(csxFilePath))) { Directory.CreateDirectory(Path.GetDirectoryName(csxFilePath)); }
    File.WriteAllText(csxFilePath, csxContent, System.Text.Encoding.UTF8);

    // Save jsonContent
    jtokenItem["Id"].Parent.Remove();
    jtokenItem["Execute"].Parent.Remove();
    var jsonContent = JsonConvert.SerializeObject(jtokenItem, Newtonsoft.Json.Formatting.Indented);
    var jsonFilePath = collectionFolder + @"\" + fileName + ".json";
    File.WriteAllText(jsonFilePath, jsonContent, System.Text.Encoding.UTF8);

}