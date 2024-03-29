﻿#load "..\Management\Common Library.csx"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***
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
using Microsoft.VisualBasic;

var selectedObject = ScriptHelper.SelectObject(
    new TabularNamedObject[]
    {
        Model.Tables["Account"],
        (Model.Tables["Dates"].Columns["Date"] as DataColumn),
        Model.Tables["Transactional"].Measures["Total Actual COGS"]
    }
    );

selectedObject.Output();