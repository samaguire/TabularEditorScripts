﻿#load "..\Management\Common Library.csx"

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TabularEditor.Scripting;

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
if (!Directory.Exists(collectionFolder))
{
    Directory.CreateDirectory(collectionFolder);
}

// Delete files with .csx and .json extensions and remove empty directories
foreach (var entry in Directory.EnumerateFileSystemEntries(collectionFolder, "*", SearchOption.AllDirectories))
{
    if (File.Exists(entry) && (entry.EndsWith(".csx") || entry.EndsWith(".json")))
    {
        File.Delete(entry);
    }
    else if (Directory.Exists(entry) && !Directory.EnumerateFileSystemEntries(entry).Any())
    {
        Directory.Delete(entry);
    }
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

    // Get csxContent and adapt to C# scripting environment
    var csxContent = "\n" + jtokenItem["Execute"].Value<string>();

    // Prefix ScriptHelper to ScriptHelper methods
        // Run in TE2 to get the list of ScriptHelper methods:
        // var scripthelperMethods = typeof(ScriptHelper).GetMethods()
        //     .Select(x => x.Name.Replace("get_", "").Replace("set_", ""))
        //     .Distinct().OrderBy(name => name)
        //     .ToList();
        // Output(scripthelperMethods);
    var scripthelperMethods = new List<string>()
    {
        @"AfterScriptExecution",
        @"BeforeScriptExecution",
        @"CallDaxFormatter",
        @"ConvertDax",
        @"CustomAction",
        @"Equals",
        @"Error",
        @"EvaluateDax",
        @"ExecuteCommand",
        @"ExecuteDax",
        @"ExecuteReader",
        @"FormatDax",
        @"GetHashCode",
        @"GetType",
        @"Info",
        @"Output",
        @"OutputErrors",
        @"ReadFile",
        @"ReferenceEquals",
        @"SaveFile",
        @"SchemaCheck",
        @"SelectColumn",
        @"SelectMeasure",
        @"SelectObject",
        @"SelectTable",
        @"SuspendWaitForm",
        @"ToString",
        @"WaitFormVisible",
        @"Warning"
    };
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
        var line = String.Empty;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.StartsWith("#r "))
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