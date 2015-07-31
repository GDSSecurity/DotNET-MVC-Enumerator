/*
* .Net MVC Enumerator
* revision 1.0  2015-07-30
* author: Priyank Nigam, Gotham Digital Science
* contact: labs@gdssecurity.com
* blog post:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace com.gdsssecurity.dotNetMVCEnumerator
{
    class Program
    {
        private static void Main(string[] args)
        {
            var attribute = "";
            List<String> resultList = new List<String>();
            try
            {
                string cur_dir = Directory.GetCurrentDirectory();

                string[] Path =
                    Directory.GetFiles(@cur_dir, " *.cs", SearchOption.AllDirectories);
                if (args.Length == 1)
                {
                    attribute = args[0];
                }
                foreach (var path in Path)
                {
                   using (var stream = File.OpenRead(path))
                    {
                        var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream), path: path);
                        SyntaxNode root = tree.GetRoot();

                        // Check if the Class inherits Apicontroller or Controller and print out all the public entry points
                        ControllerChecker controllerchk = new ControllerChecker();
                        controllerchk.controllerChecker(root, attribute);
                     }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Invalid Path");
            }
            catch (IndexOutOfRangeException)
            {
                //Shoudn't Reach this, but in case
                Console.WriteLine("No Arguments passed");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("File does not seem to a valid C# file, Skipping..");
                
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not seem to have appropiate Permissions on this direcctory");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("The operating system is Windows CE, which does not have current directory functionality.");
            }
            
        }
    }
}
    
    


