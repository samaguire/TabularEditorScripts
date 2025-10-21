#load "..\..\..\Management\Common Library.csx"
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