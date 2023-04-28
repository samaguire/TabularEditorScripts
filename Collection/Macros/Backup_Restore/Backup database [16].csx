#load "..\..\..\Management\Common Library.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "Microsoft.AnalysisServices.Core"

using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using TabularEditor;
using TabularEditor.TOMWrapper;
using TabularEditor.TOMWrapper.Utils;
using TabularEditor.UI;
using TabularEditor.Scripting;
// *** The above namespaces are required for the C# scripting environment, remove in Tabular Editor ***
using Microsoft.AnalysisServices.Core;

Model.Database.TOMDatabase.Backup(
    file: Model.Database.Name + ".abf",
    allowOverwrite: true,
    backupRemotePartitions: default,
    locations: default,
    applyCompression: true,
    password: default
);

ScriptHelper.Info("Script finished.");