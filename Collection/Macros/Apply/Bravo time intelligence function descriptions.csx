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

// Time-intelligence prefix dictionary
var tiPrefixes = new Dictionary<string, string> {
    { "YTD", "Year-to-date" },
    { "QTD", "Quarter-to-date" },
    { "MTD", "Month-to-date" },
    { "PY", "Previous year" },
    { "YOY", "Year-over-year change in" },
    { "YOY %", "Year-over-year percent change in" },
    { "PQ", "Previous quarter" },
    { "QOQ", "Quarter-over-quarter change in" },
    { "QOQ %", "Quarter-over-quarter percent change in" },
    { "PM", "Previous month" },
    { "MOM", "Month-over-month change in" },
    { "MOM %", "Month-over-month percent change in" },
    { "PYTD", "Previous year-to-date" },
    { "YOYTD", "Year-over-year-to-date change in" },
    { "YOYTD %", "Year-over-year-to-date percent change in" },
    { "PQTD", "Previous quarter-to-date" },
    { "QOQTD", "Quarter-over-quarter-to-date change in" },
    { "QOQTD %", "Quarter-over-quarter-to-date percent change in" },
    { "PMTD", "Previous month-to-date" },
    { "MOMTD", "Month-over-month-to-date change in" },
    { "MOMTD %", "Month-over-month-to-date percent change in" },
    { "PYC", "Previous year close" },
    { "YTDOPY", "Year-to-date over previous year" },
    { "YDTOPY %", "Year-to-date over previous year percent" },
    { "PQC", "Previous quarter close" },
    { "QTDOPQ", "Quarter-to-date over previous quarter" },
    { "QDTOPQ %", "Quarter-to-date over previous quarter percent" },
    { "PMC", "Previous month close" },
    { "MTDOPM", "Month-to-date over previous month" },
    { "MTDOPM %", "Month-to-date over previous month percent" },
    { "MAT", "Moving annual total" },
    { "PYMAT", "Previous year moving annual total" },
    { "MATG", "Moving annual total growth in" },
    { "MATG %", "Moving annual total growth percent in" },
};

var table = Model.Tables["Metrics"];

foreach (var m in table.Measures.ToList())
{
    if (!string.IsNullOrEmpty(m.Description))
        continue;

    var name = m.Name;
    var nameLower = name.ToLowerInvariant();

    // Try to match a time-intelligence prefix
    string bestPrefix = null;
    string bestSuffix = null;

    // Sort by prefix length descending so longer prefixes match first (e.g. "YOYTD %" before "YOY")
    foreach (var kvp in tiPrefixes.OrderByDescending(k => k.Key.Length))
    {
        var prefixLower = kvp.Key.ToLowerInvariant();
        if (nameLower.StartsWith(prefixLower + " "))
        {
            bestPrefix = kvp.Value;
            bestSuffix = name.Substring(kvp.Key.Length + 1);
            break;
        }
    }

    if (bestPrefix != null && bestSuffix != null)
    {
        m.Description = bestPrefix + " " + bestSuffix;
    }
}