﻿#r "C:\Program Files\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***
#r "C:\Program Files (x86)\Tabular Editor 3\TabularEditor3.exe" // *** Needed for C# scripting ***

using TabularEditor.TOMWrapper; // *** Needed for C# scripting ***
using TabularEditor.Scripting; // *** Needed for C# scripting ***

Model Model; // *** Needed for C# scripting ***
TabularEditor.Shared.Interaction.Selection Selected; // *** Needed for C# scripting ***

// https://community.powerbi.com/t5/Desktop/Vertipaq-Engine-VALUE-vs-HASH/m-p/690874#M333145

foreach (var c in Model.AllColumns)
{
    if (c.DataType == DataType.DateTime) { c.EncodingHint = EncodingHintType.Value; }
    if (c.DataType == DataType.Decimal) { c.EncodingHint = EncodingHintType.Value; }
    if (c.DataType == DataType.Double) { c.EncodingHint = EncodingHintType.Value; }
    if (c.DataType == DataType.Int64) { c.EncodingHint = EncodingHintType.Value; }
}

ScriptHelper.Info("Script finished.");