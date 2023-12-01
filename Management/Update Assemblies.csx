using System;

var rootFolder = @"C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref";
var subFolders = Directory.GetDirectories(rootFolder, "7.*");
var dotNetPath = Path.Combine(rootFolder, subFolders[0]);

var assemblyFiles = new List<string>()
{
    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Tabular Editor\TabularEditor.exe",
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor\TOMWrapper14.dll",
    {Directory.GetFiles(dotNetPath, "System.Drawing.Common.dll", SearchOption.AllDirectories)[0]},
    {Directory.GetFiles(dotNetPath, "System.Windows.Forms.dll", SearchOption.AllDirectories)[0]},
    {Directory.GetFiles(dotNetPath, "System.Windows.Forms.Primitives.dll", SearchOption.AllDirectories)[0]}
};

var destFolder = @".\Assemblies";

foreach (var sourceFile in assemblyFiles)
{
    var destFile = Path.Combine(destFolder, Path.GetFileName(sourceFile));
    Directory.CreateDirectory(destFolder);
    File.Copy(sourceFile, destFile, true);
}