// https://notes.mthierba.net/power-bi/analysis-services/list-new-features-in-tom-library#linqpad-script
// https://www.nuget.org/packages/Microsoft.AnalysisServices#versions-body-tab

#r "nuget: Microsoft.AnalysisServices, 19.106.1"

using System.Reflection;
using TOM = Microsoft.AnalysisServices.Tabular;

// --- Data Structure ---
public record CompatibilityInfo(string Name, string MemberType, string Box, string Excel, string PBI);

// --- Helper Functions ---

// Retrieves a property value from an attribute using reflection.
// Assumes the property exists and its value can be converted to string.
static string ReadProperty(string name, object attr) =>
    attr.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance)?.GetValue(attr)?.ToString() ?? "N/A";

// Determines the type category of a given Type.
static string GetMemberType(Type t) => t.IsEnum ? "Enum" : t.IsInterface ? "Interface" : "Class";

// Safely retrieves a custom attribute, handling specific TOM exceptions.
static Attribute GetCustomAttributeSafe(MemberInfo member, Type t)
{
    try
    {
        return member.GetCustomAttribute(t);
    }
    catch (TOM.TomException)
    {
        // This is needed to avoid errors on a few specific attributes containing unsupported expressions - we're simply ignoring those
        return null;
    }
}

// Extracts Box, Excel, and PBI compatibility properties from the attribute.
static (string Box, string Excel, string PBI) ExtractCompatibilityProperties(object attribute)
{
    return (
        ReadProperty("Box", attribute),
        ReadProperty("Excel", attribute),
        ReadProperty("Pbi", attribute)
    );
}

// Checks if a version string represents a version newer than a given threshold.
static bool IsNewerThan(string versionString, int threshold)
{
    if (versionString == "Unsupported") return false;
    if (int.TryParse(versionString, out var version))
    {
        return version > threshold;
    }
    return false; // Treat non-numeric/non-"Unsupported" values as not newer
}

// --- Main Logic ---

var asm = typeof(TOM.Server).Assembly;
var compatibilityRequirementAttributeType = asm.GetType("Microsoft.AnalysisServices.Tabular.CompatibilityRequirementAttribute");

// Common projection logic for both types and members
Func<MemberInfo, Attribute, CompatibilityInfo> projectCompatibilityItem = (memberInfo, attribute) =>
{
    var props = ExtractCompatibilityProperties(attribute);
    return new CompatibilityInfo(
        Name: memberInfo is Type type ? type.FullName : $"{memberInfo.DeclaringType?.FullName}.{memberInfo.Name}",
        MemberType: memberInfo is Type t ? GetMemberType(t) : memberInfo.MemberType.ToString(),
        props.Box,
        props.Excel,
        props.PBI
    );
};

// 1. Get types with CompatibilityRequirementAttribute
var typesWithAttribute = asm.GetTypes()
    .Select(t => new { Item = (MemberInfo)t, Attribute = GetCustomAttributeSafe(t, compatibilityRequirementAttributeType) })
    .Where(x => x.Attribute != null)
    .Select(x => projectCompatibilityItem(x.Item, x.Attribute));

// 2. Get members with CompatibilityRequirementAttribute
var membersWithAttribute = asm.GetTypes()
    .SelectMany(t => t.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
    .Select(m => new { Item = m, Attribute = GetCustomAttributeSafe(m, compatibilityRequirementAttributeType) })
    .Where(x => x.Attribute != null)
    .Select(x => projectCompatibilityItem(x.Item, x.Attribute));

// 3. Combine, filter, and order
var members = typesWithAttribute
    .Union(membersWithAttribute)
    .Where(x => IsNewerThan(x.Box, 1400) || IsNewerThan(x.Excel, 1400) || IsNewerThan(x.PBI, 1400))
    .OrderBy(x => x.Name)
    .ToArray();

// --- Output to Markdown File ---

using (StreamWriter outputFile = new StreamWriter(@".\Management\TOM-Extract-CompatibilityRequirementsAttributes.md"))
{
	outputFile.WriteLine($"|Name|MemberType|Box|Excel|PBI|");
	outputFile.WriteLine($"|-|-|-|-|-|");
	Array.ForEach(members, x => outputFile.WriteLine($"| {x.Name} | {x.MemberType} | {x.Box} | {x.Excel} | {x.PBI} |"));
}