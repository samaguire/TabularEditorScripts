#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.9\ref\net6.0\System.Windows.Forms.dll"
#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
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

// https://github.com/microsoft/Analysis-Services/tree/master/BestPracticeRules

//// System.Net.WebClient w = new System.Net.WebClient();
var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
var url = "https://raw.githubusercontent.com/microsoft/Analysis-Services/master/BestPracticeRules/BPARules.json";
var version = typeof(Model).Assembly.GetName().Version.Major;
var downloadLoc = "";

if (version == 3)
{
    downloadLoc = path + @"\TabularEditor3\BPARules.json";
}
else if (version == 2)
{
    downloadLoc = path + @"\TabularEditor\BPARules.json";
}
else
{
    ScriptHost.Error("Couldn't identify the version of Tabular Editor: " + version);
    return;
}

//// w.DownloadFile(url, downloadLoc);
var httpClient = new System.Net.Http.HttpClient();
 
using (var httpStream = await httpClient.GetStreamAsync(url))
{
    using (var fileStream = new FileStream(downloadLoc, FileMode.CreateNew))
    {
        await httpStream.CopyToAsync(fileStream);
    }
}

ScriptHost.Info("Script finished.");
