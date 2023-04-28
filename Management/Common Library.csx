#r "..\Assemblies\TabularEditor.exe"
#r "..\Assemblies\TOMWrapper14.dll"
#r "..\Assemblies\System.Windows.Forms.dll"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***

using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.UI;
using TabularEditor.Scripting;
// using TOM = Microsoft.AnalysisServices.Tabular;
// *** The above namespaces are required for the C# scripting environment, remove in Tabular Editor ***

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

var assemblyFiles = new List<string>()
{
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Tabular Editor\TabularEditor.exe",
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor\TOMWrapper14.dll",
    {Directory.GetFiles(@"C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref", "System.Windows.Forms.dll", SearchOption.AllDirectories)[0]}
};

var destFolder = @"..\Assemblies";

foreach (var sourceFile in assemblyFiles)
{
    var destFile = Path.Combine(destFolder, Path.GetFileName(sourceFile));
    Directory.CreateDirectory(destFolder);
    File.Copy(sourceFile, destFile, true);
}