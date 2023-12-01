#r "..\Assemblies\TabularEditor.exe"
#r "..\Assemblies\TOMWrapper14.dll"
#r "..\Assemblies\System.Drawing.Common.dll"
#r "..\Assemblies\System.Windows.Forms.dll"
#r "..\Assemblies\System.Windows.Forms.Primitives.dll"
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
// using TOM = Microsoft.AnalysisServices.Tabular;
// *** The above namespaces are required for the C# scripting environment, remove in Tabular Editor ***

static readonly Model Model;
static readonly UITreeSelection Selected;
// *** The above class variables are required for the C# scripting environment, remove in Tabular Editor ***

// https://docs.tabulareditor.com/te3/features/csharp-scripts.html