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

// #r "Microsoft.AnalysisServices.Core.dll"

using Microsoft.AnalysisServices.Core;

ScriptHost.Model.Database.TOMDatabase.Backup(
    file: ScriptHost.Model.Database.Name + ".abf",
    allowOverwrite: true,
    backupRemotePartitions: default,
    locations: default,
    applyCompression: true,
    password: default
);

ScriptHost.Info("Script finished.");