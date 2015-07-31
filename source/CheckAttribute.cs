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
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace com.gdsssecurity.dotNetMVCEnumerator
{
    class CheckAttribute
    {
        public static Boolean checkAttribute(IEnumerable<MethodDeclarationSyntax> methods, String attr, ClassDeclarationSyntax c)
        {
            //Check at class level
            foreach (var attribute in c.AttributeLists)
            {
                foreach (var name in attribute.Attributes)
                {
                    if (name.Name.ToString().Contains(attr))
                    {
                        return true;
                    }
                }       
            }
            //If it's not at Class level, check the methods
            foreach (var a in methods)
            {
                if (a.ToString().Contains(attr))
                {
                    //Console.Write("EntryPoint " + a.Identifier.ValueText + " supports ");
                    //Console.WriteLine(attr + " on method level");
                    return true;
                }
            }
            return false;
        }
    }
}
