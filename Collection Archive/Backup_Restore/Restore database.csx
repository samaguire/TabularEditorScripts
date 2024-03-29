﻿#load "..\..\..\Management\Common Library.csx"
#load "..\..\..\Management\Custom Classes.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
#r "Microsoft.AnalysisServices.Core"
#r "Microsoft.VisualBasic"

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
using Microsoft.VisualBasic;

var dbName = Interaction.InputBox(
    Prompt: "Provide the name of the restored database. (This will overwrite the database if it already exists.)",
    Title: "Set database name:",
    DefaultResponse: Model.Database.Name
);

if(dbName == String.Empty) { return; }

Model.Database.TOMDatabase.Server.Restore(
    file: Model.Database.Name + ".abf",
    databaseName: dbName,
    allowOverwrite: true
);