using System.IO.Compression;

string localFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
string[] appSubfolders = { "TabularEditor", "TabularEditor3" };

foreach (var item in appSubfolders)
{
    var folderPath = $"{localFolder}\\{item}";
    var zipPath = $"{localFolder}\\{item}.zip";
    if (File.Exists(zipPath))
    {
        ZipFile.ExtractToDirectory(zipPath, folderPath, true);
    }
}