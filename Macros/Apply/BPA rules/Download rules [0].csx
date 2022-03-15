#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting, remove in TE3 ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting, remove in TE3 ***
using TabularEditor.Scripting; // *** Needed for C# scripting, remove in TE3 ***

Model Model; // *** Needed for C# scripting, remove in TE3 ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting, remove in TE3 ***

// https://github.com/microsoft/Analysis-Services/tree/master/BestPracticeRules

System.Net.WebClient w = new System.Net.WebClient(); 

var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
var url = "https://raw.githubusercontent.com/microsoft/Analysis-Services/master/BestPracticeRules/BPARules.json";
var version = typeof(Model).Assembly.GetName().Version.Major;
var downloadLoc = "";

if (version == 3)
{

    downloadLoc = path+@"\TabularEditor3\BPARules.json";

}
else if (version == 2)
{

    downloadLoc = path+@"\TabularEditor\BPARules.json";

}
else
{

    Error("Couldn't identify the version of Tabular Editor: "+version);
    return;

}

w.DownloadFile(url, downloadLoc);

Info("Script finished.");
