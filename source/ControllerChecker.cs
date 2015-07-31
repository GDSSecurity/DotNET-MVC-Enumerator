/*
* .Net MVC Enumerator
* revision 1.0  2015-07-30
* author: Priyank Nigam, Gotham Digital Science
* contact: labs@gdssecurity.com
* blog post:
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace com.gdsssecurity.dotNetMVCEnumerator
{
    class ControllerChecker
    {
        Boolean _isAttrSet;
        List<String> results = new List<string>();
        public void controllerChecker(SyntaxNode root, String args)
        {
            /*
                 Check if the Class inherits Apicontroller or Controller
            */
            try
            {
                if (root.DescendantNodes().OfType<BaseTypeSyntax>().First().ToString().Equals("ApiController") ||
                    root.DescendantNodes().OfType<BaseTypeSyntax>().First().ToString().Equals("Controller"))
                {
                    ClassDeclarationSyntax isController =
                        root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                    /*
                    Get all the public methods in this class
                     */
                    IEnumerable<MethodDeclarationSyntax> methods =
                        from m in root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                        where m.Modifiers.ToString().Contains("public")
                        select m;

                    Boolean isValid = false;
                    foreach (var a in methods)
                    {
                        //isValid = false;
                        /*
                        Check the presence of attribute (passsed as command line argument)
                        */

                        if (!_isAttrSet && !String.IsNullOrEmpty(args))
                        {
                            _isAttrSet = CheckAttribute.checkAttribute(methods, args, isController);
                            isValid = true;
                        }

                        if (a.ToString().Contains("HttpPost"))
                        {
                            results.Add(a.Identifier.ValueText);
                            results.Add("HTTP POST");
                            results.Add(!String.IsNullOrEmpty(args)?_isAttrSet.ToString() : "");
                            isValid = true;
                        }
                        if (a.ToString().Contains("HttpGet"))
                        {
                            results.Add(a.Identifier.ValueText);
                            results.Add("HTTP GET");
                            results.Add(!String.IsNullOrEmpty(args) ? _isAttrSet.ToString() : "");
                            isValid = true;
                        }
                    }
                    //Console.WriteLine(isController.Identifier.ToString() + " contains " + methods.Count() + " entry methods" + (isValid ? " and they are as follows" : " but they do not meet the criteria"));
                    if (isValid)
                    {
                        //DataTables hack!
                        Console.WriteLine("For " + isController.Identifier.ToString());
                        DataTable resultsTable = new DataTable("Entry Points Enumerator Output");
                        resultsTable.Clear();
                        resultsTable.Columns.Add("Entry point");
                        resultsTable.Columns.Add("Method Supported");
                        resultsTable.Columns.Add("Attribute " + args + " Set?");

                        TableParser.tableParser(resultsTable, results);
                    }
                }
                else
                {
                    Console.WriteLine("There are no Defined Entrypoints in this file");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Ooops! Something went wrong!");
            }
        
        }
    }
}
