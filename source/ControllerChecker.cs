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

namespace DotNetMVCEnumerator.source
{
    class ControllerChecker
    {
        List<String> results = new List<string>();

        public bool inheritsFromController(SyntaxNode root, String args)
        {

            bool isValid = false;

            try
            {
                 isValid = root.DescendantNodes().OfType<BaseTypeSyntax>().First().ToString().Equals("ApiController") ||
                   root.DescendantNodes().OfType<BaseTypeSyntax>().First().ToString().Equals("Controller");
            }
            catch (InvalidOperationException)
            {
                isValid = false;   
            }

            return isValid;
        }


        public void enumerateEntrypoints(SyntaxNode root, String attributeToSearch, String negativeSearch, 
            String path, Dictionary<String, List<Result>> resultList)
        {
            try
            {
                ClassDeclarationSyntax controller =
                    root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                
                //Get all the public methods in this class
                IEnumerable<MethodDeclarationSyntax> methods =
                    from m in root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                    where m.Modifiers.ToString().Contains("public")
                    select m;

                List<string> controllerAttrs = CheckAttribute.getControllerAttributes(controller);

                List<Result> resultsForController = new List<Result>();

                foreach (var method in methods)
                {
                    Result result = new Result();

                    // Return all the attributes to list it only in the CSV output
                    List<String> methodAttributes = CheckAttribute.getMethodAttributes(method, controller);
                    
                    // Set attributes set at Controller level
                    CheckAttribute.setMethodAttributesFromController(methodAttributes, controllerAttrs);

                    Boolean addAttributeFlag = true;

                    if (!String.IsNullOrEmpty(attributeToSearch))
                    {
                        String attributeMatched = methodAttributes.FirstOrDefault(s => s.StartsWith(attributeToSearch));

                        // Only add attributes that start with the value 'searched' for - via command line switch
                        if(String.IsNullOrEmpty(attributeMatched))
                        {
                            addAttributeFlag = false;
                        }
                    }


                    // Only add attributes that are missing the 'negative search' passed via command line switch
                    if (!String.IsNullOrEmpty(negativeSearch))
                    {
                        String attributeMatched = methodAttributes.FirstOrDefault(s => s.StartsWith(negativeSearch));
                       
                        if(!String.IsNullOrEmpty(attributeMatched))
                        {
                            addAttributeFlag = false;
                        }
                    }
                    
                   
                    if(addAttributeFlag)
                    {
                        result.MethodName = method.Identifier.ValueText;

                        result.Attributes = methodAttributes;

                        result.setSupportedHTTPMethods(methodAttributes);

                        result.setRoute(methodAttributes);

                        resultsForController.Add(result);
                    }
                    
                }

                if(resultsForController.ToArray().Length > 0)
                {
                    resultList.Add(path, resultsForController);
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Failed to parse: \"" + path + "\", Skipping..");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled Exception Occurred.");
                Console.WriteLine(e.ToString());
            }
        }
    }
}
