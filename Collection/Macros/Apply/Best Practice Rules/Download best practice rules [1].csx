#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.12\ref\net6.0\System.Windows.Forms.dll"
#r "C:\Program Files\Tabular Editor 3\TabularEditor3.Shared.dll"
#r "C:\Program Files\Tabular Editor 3\TOMWrapper.dll"
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.Shared;
using TabularEditor.Shared.Scripting;
using TabularEditor.Shared.Interaction;
using TabularEditor.Shared.Services;

/*** Everything ABOVE this point is required for the C# scripting environment, remove in TE3 ***/

// https://github.com/microsoft/Analysis-Services/tree/master/BestPracticeRules

using System.IO;

var url = "https://raw.githubusercontent.com/microsoft/Analysis-Services/master/BestPracticeRules/BPARules.json";
var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
var jsonPath = "";
var version = typeof(Model).Assembly.GetName().Version.Major;

switch (version)
{

    case 3:
        jsonPath = path + @"\TabularEditor3\BPARules.json";
        break;

    case 2:
        jsonPath = path + @"\TabularEditor\BPARules.json";
        break;

    default:
        ScriptHost.Error("Couldn't identify the version of Tabular Editor: " + version);
        return;

}

using (var client = new System.Net.Http.HttpClient())
{
    var responseBody = client.GetStringAsync(url).GetAwaiter().GetResult();
    File.WriteAllText(jsonPath, responseBody, System.Text.Encoding.UTF8);
}