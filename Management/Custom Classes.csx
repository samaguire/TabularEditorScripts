#load ".\Common Library.csx"
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

// *** The below public classes are common across scripts ***
public static class CustomClass
{

    // this is an example one
    public static Table CreateCT(string tableName, string tableExpression)
    {

        return Model.AddCalculatedTable(
            name: tableName,
            expression: tableExpression
            );

    }

}