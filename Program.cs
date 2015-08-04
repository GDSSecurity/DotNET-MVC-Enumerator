/*
* .Net MVC Enumerator
* revision 1.0  2015-07-30
* author: Priyank Nigam, Gotham Digital Science
* contact: labs@gdssecurity.com
* blog post:
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetMVCEnumerator.source;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace DotNetMVCEnumerator
{
    static class Program
    {
        private static void Main(string[] args)
        {   
            string attribute = "";
            string csvOutputFile = "";
            List<List<String>> resultList = new List<List<string>>();

            try
            {
                var options = new Options();
                bool isFlagValid = options.OptionParser(args, out csvOutputFile, out attribute);
                
                // If Command-line options not set correctly, show default message and exit
                if (!isFlagValid)
                {
                    System.Environment.Exit(0);
                }

                string curDir = Directory.GetCurrentDirectory();
                string[] paths = Directory.GetFiles(@curDir, "*.cs", SearchOption.AllDirectories);

                if (paths.Length > 0)
                {
                    foreach (var path in paths)
                    {
                        using (var stream = File.OpenRead(path))
                        {
                            var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream), path: path);
                            SyntaxNode root = tree.GetRoot();

                            // Check if the Class inherits Apicontroller or Controller and print out all the public entry points
                            ControllerChecker controllerchk = new ControllerChecker();
                            resultList.Add(controllerchk.controllerChecker(root, attribute));
                        }
                    }
                    
                    Boolean isResultListEmpty = true;
                    foreach (var result in resultList)
                    {
                        if (result.Count > 0)
                        {
                            isResultListEmpty = false;
                        }
                    }
                    // Output to CSV if a filename is specified and resultList is not empty

                    if (!String.IsNullOrEmpty(csvOutputFile) && !isResultListEmpty)
                    {
                        var csvExport = new CsvExport();
                        for (int j = 0; j < resultList.Count; j++)
                        {
                            for (int i = 0; i < (resultList[j].Count() - 2); i = i + 3)
                            {
                                csvExport.AddRow();
                                csvExport["Entry point"] = resultList[j].ToArray()[i].ToString();
                                csvExport["Method Supported"] = resultList[j].ToArray()[i + 1].ToString();
                                csvExport[attribute + " Set?"] = resultList[j].ToArray()[i + 2].ToString();
                            }
                        }

                        File.Create(csvOutputFile).Dispose();
                        csvExport.ExportToFile(curDir + Path.DirectorySeparatorChar + csvOutputFile);
                        Console.WriteLine("Results written at " + curDir + Path.DirectorySeparatorChar + csvOutputFile);
                    }
                    if (isResultListEmpty)
                    {
                        Console.WriteLine();
                        TableParser.centerText("No Results to display!");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("Need your results in CSV format? Try " +
                                          Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location) +
                                          " -o <Filename> !");
                    }
                }
                else
                {
                    Console.WriteLine("Your Current Path does not contain in .cs Files");
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
            catch (UnauthorizedAccessException e)
            {
                e.GetBaseException();
                Console.WriteLine("You do not seem to have appropiate Permissions on this direcctory");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("The operating system is Windows CE, which does not have current directory functionality.");
            }
           
        }
    }
}
    
    


