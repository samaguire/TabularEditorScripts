// https://notes.mthierba.net/power-bi/analysis-services/list-new-features-in-tom-library#linqpad-script
// https://www.nuget.org/packages/Microsoft.AnalysisServices.retail.amd64#versions-body-tab

#r "nuget: Microsoft.AnalysisServices.NetCore.retail.amd64"

using System.Reflection;
using TOM = Microsoft.AnalysisServices.Tabular;

var asm = typeof(TOM.Server).Assembly;
var compatAttr = asm.GetType("Microsoft.AnalysisServices.Tabular.CompatibilityRequirementAttribute");

string ReadProperty(string name, object attr) => compatAttr.GetProperty(name).GetValue(attr).ToString();
string GetMemberType(Type t) => t.IsEnum ? "Enum" : t.IsInterface ? "Interface" : "Class";
Attribute GetCustomAttributeSafe(MemberInfo member, Type t)
{ // This is needed to avoid errors on a few specific attributes containing unsupported expressions - we're simply ignoring those
    try
    {
        return member.GetCustomAttribute(t);
    }
    catch (TOM.TomException)
    {
        return null;
    }
}

var members = asm.GetTypes()
    .Select(t => new
    {
        Type = t,
        CompatAttribute = t.GetCustomAttribute(compatAttr)
    })
    .Where(x => x.CompatAttribute != null)
    .Select(x => new
    {
        Name = x.Type.FullName,
        MemberType = GetMemberType(x.Type),
        Box = ReadProperty("Box", x.CompatAttribute),
        Excel = ReadProperty("Excel", x.CompatAttribute),
        PBI = ReadProperty("Pbi", x.CompatAttribute)
    })
    .Union(
        asm.GetTypes()
        .SelectMany(t => t.GetMembers())
        .Select(m => new
        {
            Member = m,
            Name = $"{m.DeclaringType.FullName}.{m.Name}",
            CompatAttribute = GetCustomAttributeSafe(m, compatAttr),
            MemberType = m.MemberType.ToString()
        })
        .Where(x => x.CompatAttribute != null)
        .Select(x => new
        {
            x.Name,
            x.MemberType,
            Box = ReadProperty("Box", x.CompatAttribute),
            Excel = ReadProperty("Excel", x.CompatAttribute),
            PBI = ReadProperty("Pbi", x.CompatAttribute)
        })
    )
    .Where(x => /* toggle this for 1200/1400: */ !(((int.TryParse(x.Box, out var box) && box <= 1400) || x.Box == "Unsupported")
        && ((int.TryParse(x.Excel, out var excel) && excel <= 1400) || x.Excel == "Unsupported")
        && ((int.TryParse(x.PBI, out var pbi) && pbi <= 1400) || x.PBI == "Unsupported")))
    .OrderBy(x => x.Name)
    .ToArray();

// Convert to markdown table for blog post:
using (StreamWriter outputFile = new StreamWriter(@".\Management\TOM-Extract-CompatibilityRequirementsAttributes.md"))
{
	outputFile.WriteLine($"|Name|MemberType|Box|Excel|PBI|");
	outputFile.WriteLine($"|-|-|-|-|-|");
	Array.ForEach(members, x => outputFile.WriteLine($"| {x.Name} | {x.MemberType} | {x.Box} | {x.Excel} | {x.PBI} |"));
}