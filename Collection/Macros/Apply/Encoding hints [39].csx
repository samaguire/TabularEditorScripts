#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
#r "C:\Users\samag\AppData\Local\TabularEditor\TOMWrapper14.dll"
#r "C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\6.0.13\ref\net6.0\System.Windows.Forms.dll"
// *** The above assemblies are required for the C# scripting environment, remove in Tabular Editor ***

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

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

// https://community.powerbi.com/t5/Desktop/Vertipaq-Engine-VALUE-vs-HASH/m-p/690874#M333145

foreach (var c in Model.AllColumns)
{
    if (c.DataType == DataType.DateTime || c.DataType == DataType.Decimal || c.DataType == DataType.Double || c.DataType == DataType.Int64)
    {
        c.EncodingHint = EncodingHintType.Value;
    }
}

ScriptHelper.Info("Script finished.");