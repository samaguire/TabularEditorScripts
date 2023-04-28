#load "..\..\..\Management\Common Library.csx"
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

// Get the number of decimal places from the user
var decimalPlaces = Interaction.InputBox(
    Prompt: "Provide the number of decimal places to be displayed.",
    Title: "Set decimal places:",
    DefaultResponse: "1"
);

if(decimalPlaces == "") { return; }

// Update model's compatibility level if required
if (Model.Database.CompatibilityLevel < 1601)
{
    Model.Database.CompatibilityLevel = 1601;
}

// Set template measure format string expression
var templateMeasureFormatStringExpression = @"
VAR DecimalPlaces = <d>
VAR CurrentValue = SELECTEDMEASURE()
VAR ValueLog =
    IF(
        CurrentValue <> 0,
        INT( LOG( ABS( CurrentValue ), 1000 ) ),
        0
    )
VAR Commas = REPT( "","", MIN( 4, ValueLog ) )
VAR Suffix =
    SWITCH(
        ValueLog,
        0, """",
        1, ""K"",
        2, ""M"",
        3, ""bn"",
        ""T""
    )
VAR Decimals = ""."" & REPT( 0, DecimalPlaces )
RETURN
    IF(
        DecimalPlaces > 0,
        ""#,##0"" & Commas & Decimals & Suffix
            & "";-#,##0"" & Commas & Decimals & Suffix
            & "";-"",
        ""#,##0"" & Commas & Suffix
            & "";-#,##0"" & Commas & Suffix
            & "";-""
    )";

// Apply format string expression
foreach (var m in Selected.Measures)
{

    m.FormatString = string.Empty;

    m.FormatStringExpression = templateMeasureFormatStringExpression
        .Replace("<d>", decimalPlaces);

    m.SetAnnotation("DisallowApplyingDefaultFormatting", "true");

}

ScriptHelper.Info("Script finished.");