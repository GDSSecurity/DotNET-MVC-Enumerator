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
                //if (a.ToString().Contains(attr)) return true;
            }
            return isSet;
        }
    }
}
