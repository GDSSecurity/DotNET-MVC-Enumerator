/*
* .Net MVC Enumerator
* revision 1.0  2015-07-30
* author: Priyank Nigam, Gotham Digital Science
* contact: labs@gdssecurity.com
* blog post:
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetMVCEnumerator.source
{
    class CheckAttribute
    {
        public static Boolean checkAttribute(IEnumerable<MethodDeclarationSyntax> methods, String attr, ClassDeclarationSyntax c)
        {
            Boolean isSet = false;
            //Check at class level, digging two levels deep for class-level attributes in ClassDeclarationSyntax Type
            foreach (var attribute in c.AttributeLists)
            {
                foreach (var name in attribute.Attributes)
                {
                    if (name.Name.ToString().Equals(attr))
                    {
                        isSet = true;
                    }
                }       
            }
            /* If not found at Class level, check the methods
            * Unfortunately MethodDeclarationSyntax is of Type IEnumerable, so we dig three levels for the Atttribute Lists
            */
            foreach (var methodList in methods)
            {
                foreach (var methodLevelAttirbutes in methodList.AttributeLists)
                {
                    foreach (var methodAttr in methodLevelAttirbutes.Attributes)
                    {
                        if (methodAttr.ToString().Equals(attr))
                        {
                            isSet = true;
                        }
                    }
                }
            }
            return isSet;
        }

        public static List<string> getControllerAttributes(ClassDeclarationSyntax controller)
        {
            List<string> attributes = new List<string>();

            foreach (var attribute in controller.AttributeLists)
            {
                foreach (var name in attribute.Attributes)
                {
                    attributes.Add(name.ToString());
                }
            }

            return attributes;
        }

        public static List<String> getMethodAttributes(MethodDeclarationSyntax method, ClassDeclarationSyntax c)
        {
            List<String> attributes = new List<string>();

            foreach (var methodLevelAttributes in method.AttributeLists)
            {
                foreach (var methodAttr in methodLevelAttributes.Attributes)
                {
                    attributes.Add(methodAttr.ToString());
                }
            }

            return attributes;

        }

        public static void setMethodAttributesFromController(List<String> methodAttributes, List<String> controllerAttributes)
        {
            foreach(String controllerAttr in controllerAttributes)
            {
                if(!methodAttributes.Contains(controllerAttr))
                {
                    // Some Attributes to Ignore
                    if(!controllerAttr.StartsWith("Route") && !isHttpMethodAttribute(controllerAttr) && 
                        !controllerAttr.StartsWith("AddVerbs"))
                    {
                        methodAttributes.Add(controllerAttr);
                    }
                }
            }
        }

        public static Boolean isHttpMethodAttribute(String attribute)
        {
            String[] httpmethods = new String[] { "HttpPost", "HttpGet", "HttpPut", "HttpOptions", "HttpPatch", "HttpDelete", "HttpHead" };

            return httpmethods.Contains(attribute);
        }
    }
}
