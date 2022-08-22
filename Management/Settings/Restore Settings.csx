using System.IO.Compression;

var te3Path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor3";
var zipPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\TabularEditor3.zip";

ZipFile.ExtractToDirectory(zipPath, te3Path, true);