#load "..\Management\Common Library.csx"
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

int version = typeof(TabularEditor.TOMWrapper.Model).Assembly.GetName().Version.Major;
if (version == 2)
{
    // Tabular Editor 2.x specific code
}
if (version == 3)
{
    // Tabular Editor 3.x specific code
}