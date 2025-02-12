using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var rootFolder = @"C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref";
// Get all subfolders and order them by version number in descending order
var subFolders = Directory.GetDirectories(rootFolder).OrderByDescending(folder => new Version(Path.GetFileName(folder))).ToList();
if (subFolders.Count == 0)
{
    throw new DirectoryNotFoundException("No versioned subfolders found.");
}
// Use the newest version subfolder
var dotNetPath = Path.Combine(rootFolder, subFolders[0]);

var assemblyFiles = new List<string>()
{
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Tabular Editor", "TabularEditor.exe"),
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TabularEditor", "TOMWrapper14.dll"),
    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TabularEditor", "newtonsoft.json.dll"),
    Directory.GetFiles(dotNetPath, "System.Drawing.Common.dll", SearchOption.AllDirectories).FirstOrDefault(),
    Directory.GetFiles(dotNetPath, "System.Windows.Forms.dll", SearchOption.AllDirectories).FirstOrDefault(),
    Directory.GetFiles(dotNetPath, "System.Windows.Forms.Primitives.dll", SearchOption.AllDirectories).FirstOrDefault()
};

var destFolder = @".\Management\Assemblies";

foreach (var sourceFile in assemblyFiles.Where(file => !string.IsNullOrEmpty(file)))
{
    try
    {
        var destFile = Path.Combine(destFolder, Path.GetFileName(sourceFile));
        Directory.CreateDirectory(destFolder);
        File.Copy(sourceFile, destFile, true);
        // Console.WriteLine($"Successfully copied {sourceFile} to {destFile}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"*** Warning! *** Failed to copy {sourceFile}. Error: {ex.Message}");
    }
}