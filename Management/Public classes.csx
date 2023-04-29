#load ".\Common Library.csx"
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
using Microsoft.VisualBasic;

// *** The below public classes are common across scripts ***

public static class Fx
{

    // In TE2 (at least up to 2.17.2) any method that accesses or modifies the model needs a reference to the model 
    // the following is an example method where you can build extra logic
    public static Table CreateCalcTable(Model model, string tableName, string tableExpression)
    {
        return model.AddCalculatedTable(name: tableName, expression: tableExpression);
    }

    public static Table SelectTableExt(Model model, string possibleName = null, string annotationName = null, string annotationValue = null,
        Func<Table, bool> lambdaExpression = null, string label = "Select Table", bool skipDialogIfSingleMatch = true, bool showOnlyMatchingTables = true,
        IEnumerable<Table> candidateTables = null, bool showErrorIfNoTablesFound = false, string errorMessage = "No tables found", bool selectFirst = false,
        bool showErrorIfNoSelection = true, string noSelectionErrorMessage = "No table was selected", bool excludeCalcGroups = false, bool returnNullIfNoTablesFound = false)
    {

        Table table = null as Table;

        //Output("lambda expression is null = " + lambdaExpression == null);
        //Output(annotationName + " " + annotationValue);

        if (lambdaExpression == null)
        {
            if (possibleName != null)
            {
                lambdaExpression = (t) => t.Name == possibleName;
            }
            else if (annotationName != null && annotationValue != null)
            {
                lambdaExpression = (t) => t.GetAnnotation(annotationName) == annotationValue;
            }
            else
            {
                lambdaExpression = (t) => true; //no filtering
            }
        }

        //use candidateTables if passed as argument
        IEnumerable<Table> tables = null as IEnumerable<Table>;

        if (candidateTables != null)
        {
            tables = candidateTables;
        }
        else
        {
            tables = model.Tables;
        }

        //Output("Step 10");
        //Output(tables);

        if (lambdaExpression != null)
        {
            tables = tables.Where(lambdaExpression);
        }

        //Output("Step 20");
        //Output(tables);

        if (excludeCalcGroups)
        {
            tables = tables.Where(t => t.ObjectType != ObjectType.CalculationGroupTable);
        }

        //Output("Step 30");
        //Output(tables);

        //none found, let the user choose from all tables
        if (tables.Count() == 0)
        {
            if (returnNullIfNoTablesFound)
            {
                if (showErrorIfNoTablesFound) ScriptHelper.Error(errorMessage);
                ScriptHelper.Output("No tables found");
                return table;
            }
            else
            {
                ScriptHelper.Output("returnNullIfNoTablesFound is false");
                table = ScriptHelper.SelectTable(tables: model.Tables, label: label);
            }
        }
        else if (tables.Count() == 1 && !skipDialogIfSingleMatch)
        {
            ScriptHelper.Output("tables.Count() == 1 && !skipDialogIfSingleMatch");
            table = ScriptHelper.SelectTable(tables: model.Tables, preselect: tables.First(), label: label);
        }
        else if (tables.Count() == 1 && skipDialogIfSingleMatch)
        {
            table = tables.First();
        }
        else if (tables.Count() > 1)
        {
            if (selectFirst)
            {
                table = tables.First();
            }
            else if (showOnlyMatchingTables)
            {
                ScriptHelper.Output("showOnlyMatchingTables");
                table = ScriptHelper.SelectTable(tables: tables, preselect: tables.First(), label: label);
            }
            else
            {
                ScriptHelper.Output("else");
                table = ScriptHelper.SelectTable(tables: model.Tables, preselect: tables.First(), label: label);
            }
        }
        else
        {
            ScriptHelper.Error(@"Unexpected logic in ""SelectTableExt""");
            return null;
        }

        if (showErrorIfNoSelection && table == null)
        {
            ScriptHelper.Error(noSelectionErrorMessage);
        }

        return table;

    }

    public static CalculationGroupTable SelectCalculationGroup(Model model, string possibleName = null, string annotationName = null, string annotationValue = null,
        Func<Table, bool> lambdaExpression = null, string label = "Select Table", bool skipDialogIfSingleMatch = true, bool showOnlyMatchingTables = true,
        bool showErrorIfNoTablesFound = true, string errorMessage = "No calculation groups found", bool selectFirst = false,
        bool showErrorIfNoSelection = true, string noSelectionErrorMessage = "No calculation group was selected", bool returnNullIfNoTablesFound = false)
    {

        CalculationGroupTable calculationGroupTable = null as CalculationGroupTable;

        Func<Table, bool> lambda = (x) => x.ObjectType == ObjectType.CalculationGroupTable;
        if (!model.Tables.Any(lambda)) return calculationGroupTable;

        IEnumerable<Table> tables = model.Tables.Where(lambda);

        //Output(tables.Select(x => x.Name));
        //Output(annotationName + " " + annotationValue);

        Table table = Fx.SelectTableExt(
            model: model,
            possibleName: possibleName,
            annotationName: annotationName,
            annotationValue: annotationValue,
            lambdaExpression: lambdaExpression,
            label: label,
            skipDialogIfSingleMatch: skipDialogIfSingleMatch,
            showOnlyMatchingTables: showOnlyMatchingTables,
            showErrorIfNoTablesFound: showErrorIfNoTablesFound,
            errorMessage: errorMessage,
            selectFirst: selectFirst,
            showErrorIfNoSelection: showErrorIfNoSelection,
            noSelectionErrorMessage: noSelectionErrorMessage,
            returnNullIfNoTablesFound: returnNullIfNoTablesFound,
            candidateTables: tables);

        if (table == null) return calculationGroupTable;

        calculationGroupTable = table as CalculationGroupTable;

        return calculationGroupTable;

    }

    public static CalculationGroupTable AddCalculationGroupExt(Model model, out bool calcGroupWasCreated, string defaultName = "New Calculation Group",
        string annotationName = null, string annotationValue = null, bool createOnlyIfNotFound = true,
        string prompt = "Name", string Title = "Provide a name for the Calculation Group", bool customCalcGroupName = true)
    {

        Func<Table, bool> lambda = null as Func<Table, bool>;
        CalculationGroupTable cg = null as CalculationGroupTable;
        calcGroupWasCreated = false;
        string calcGroupName = String.Empty;

        if (createOnlyIfNotFound)
        {

            if (annotationName == null && annotationValue == null)
            {

                if (customCalcGroupName)
                {
                    calcGroupName = Interaction.InputBox(Prompt: "Name", Title: "Provide a name for the Calculation Group");
                }
                else
                {
                    calcGroupName = defaultName;
                }

                cg = Fx.SelectCalculationGroup(model: model, possibleName: calcGroupName, showErrorIfNoTablesFound: false, selectFirst: true);

            }
            else
            {
                //Output("With annotations");
                cg = Fx.SelectCalculationGroup(model: model,
                    showErrorIfNoTablesFound: false,
                    annotationName: annotationName,
                    annotationValue: annotationValue,
                    returnNullIfNoTablesFound: true);
            }

            if (cg != null) return cg;
        }

        if (calcGroupName == String.Empty)
        {
            if (customCalcGroupName)
            {
                calcGroupName = Interaction.InputBox(Prompt: "Name", Title: "Provide a name for the Calculation Group");
            }
            else
            {
                calcGroupName = defaultName;
            }
        }

        cg = model.AddCalculationGroup(name: calcGroupName);

        if (annotationName != null && annotationValue != null)
        {
            cg.SetAnnotation(annotationName, annotationValue);
        }

        calcGroupWasCreated = true;

        return cg;

    }

    public static CalculationItem AddCalculationItemExt(CalculationGroupTable cg, string calcItemName, string valueExpression = "SELECTEDMEASURE()",
        string formatStringExpression = "", bool createOnlyIfNotFound = true, bool rewriteIfFound = false)
    {

        CalculationItem calcItem = null as CalculationItem;

        Func<CalculationItem, bool> lambda = (ci) => ci.Name == calcItemName;

        if (createOnlyIfNotFound)
        {

            if (cg.CalculationItems.Any(lambda))
            {

                calcItem = cg.CalculationItems.Where(lambda).FirstOrDefault();

                if (!rewriteIfFound)
                {
                    return calcItem;
                }

            }

        }

        if (calcItem == null)
        {
            calcItem = cg.AddCalculationItem(name: calcItemName, expression: valueExpression);
        }
        else
        {
            //rewrite the found calcItem
            calcItem.Expression = valueExpression;
        }

        if (formatStringExpression != String.Empty)
        {
            calcItem.FormatStringExpression = formatStringExpression;
        }

        return calcItem;

    }

}