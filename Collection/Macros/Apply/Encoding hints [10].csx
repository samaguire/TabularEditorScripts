#load "..\..\..\Management\Common Library.csx"
#load "..\..\..\Management\Custom Classes.csx"
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

// https://community.powerbi.com/t5/Desktop/Vertipaq-Engine-VALUE-vs-HASH/m-p/690874#M333145

foreach (var c in Model.AllColumns)
{
    if (c.DataType == DataType.DateTime || c.DataType == DataType.Decimal || c.DataType == DataType.Double || c.DataType == DataType.Int64)
    {
        c.EncodingHint = EncodingHintType.Value;
    }
}

ScriptHelper.Info("Script finished.");