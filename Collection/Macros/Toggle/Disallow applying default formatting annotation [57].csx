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

foreach (var m in Selected.Measures)
{
    if (String.IsNullOrEmpty(m.GetAnnotation("DisallowApplyingDefaultFormatting")))
    {
        m.SetAnnotation("DisallowApplyingDefaultFormatting", "true");
    }
    else
    {
        m.RemoveAnnotation("DisallowApplyingDefaultFormatting");
    }
}

foreach (var c in Selected.Columns)
{
    if (String.IsNullOrEmpty(c.GetAnnotation("DisallowApplyingDefaultFormatting")))
    {
        c.SetAnnotation("DisallowApplyingDefaultFormatting", "true");
    }
    else
    {
        c.RemoveAnnotation("DisallowApplyingDefaultFormatting");
    }
}

ScriptHelper.Info("Script finished.");