#r "C:\Program Files (x86)\Tabular Editor\TabularEditor.exe"
#r "C:\Users\samag\AppData\Local\TabularEditor\TOMWrapper14.dll"
#r "C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Windows.Forms\v4.0_4.0.0.0__b77a5c561934e089\System.Windows.Forms.dll"
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

foreach (var c in Selected.Columns)
{
    if (c.DataType == DataType.Decimal || c.DataType == DataType.Double || c.DataType == DataType.Int64)
    {
        if (String.IsNullOrEmpty(c.GetExtendedProperty("ParameterMetadata")))
        {
            c.SetExtendedProperty("ParameterMetadata", "{\"version\":0}", ExtendedPropertyType.Json);
        }
        else
        {
            c.RemoveExtendedProperty("ParameterMetadata");
        }
    }
}

ScriptHelper.Info("Script finished.");