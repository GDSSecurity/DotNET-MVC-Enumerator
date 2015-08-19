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
using System.Data;

namespace DotNetMVCEnumerator
{
    static class Program
    {
        private static void Main(string[] args)
        {
            string attribute = "";
            string csvOutputFile = "";
            string directoryToScan = "";
            string negativeSearch = "";
            Dictionary<String, List<Result>> results = new Dictionary<string, List<Result>>();

            try
            {
                var options = new Options();
                bool isFlagValid = options.OptionParser(args, out csvOutputFile, out attribute, out directoryToScan, out negativeSearch);
                
                // If Command-line options not set correctly, show default message and exit
                if (!isFlagValid)
                {
                    System.Environment.Exit(0);
                }

                if(String.IsNullOrEmpty(csvOutputFile))
                {
                    DateTime dt = DateTime.Now;
                    String timestamp = dt.ToString("yyyyMMddHHmmss");
                    csvOutputFile = "enumerated_controllers_"+timestamp+".csv";
                }

                string curDir = Directory.GetCurrentDirectory();
                if (String.IsNullOrEmpty(directoryToScan))
                {
                    directoryToScan = curDir;
                }
                string[] paths = Directory.GetFiles(directoryToScan, "*.cs", SearchOption.AllDirectories);

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
                            if (controllerchk.inheritsFromController(root, attribute))
                            {
                                controllerchk.enumerateEntrypoints(root, attribute, negativeSearch, path, results);
                            }
                        }
                    }

                    string[] controllerPaths = results.Keys.ToArray();
                    String pathToTrim = getPathToTrim(controllerPaths);

                    if (!String.IsNullOrEmpty(attribute) || !String.IsNullOrEmpty(negativeSearch))
                    {
                        printCommandLineResults(results, pathToTrim);
                    }
                   
                    printCSVResults(results, csvOutputFile, pathToTrim);
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
            catch (UnauthorizedAccessException e)
            {
                e.GetBaseException();
                Console.WriteLine("You do not seem to have appropiate Permissions on this direcctory");
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("The operating system is Windows CE, which does not have current directory functionality.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Illegal characters passed as arguments! ");
            }
            catch(Exception)
            {
                Console.WriteLine("Unexpected error");
            }
        }

        public static void printCommandLineResults(Dictionary<String, List<Result>> results, String pathToTrim)
        {
            foreach( KeyValuePair<String, List<Result>> pair in results)
            {
                Console.WriteLine("Controller: \n" + pair.Key.Replace(pathToTrim, ""));
                Console.WriteLine("\nMethods: ");
                foreach(Result result in pair.Value)
                {
                    Console.WriteLine(result.MethodName);
                }
                    
                Console.WriteLine("\n======================\n");
            }
        }

        public static void printCSVResults(Dictionary<String, List<Result>> results, String filename, String pathToTrim)
        {
            var csvExport = new CsvExport();

            foreach (KeyValuePair<String, List<Result>> pair in results)
            {
                foreach (Result result in pair.Value)
                {
                    csvExport.AddRow();
                    csvExport["Controller"] = pair.Key.Replace(pathToTrim, "");
                    csvExport["Method Name"] = result.MethodName;
                    csvExport["Route"] = result.Route;
                    csvExport["HTTP Method"] = string.Join(", ", result.HttpMethods.ToArray());
                    csvExport["Attributes"] = string.Join(", ", result.Attributes.ToArray());
                }
            }

            File.Create(filename).Dispose();
            csvExport.ExportToFile(filename);
            Console.WriteLine("CSV output written to: " + filename);
        }


        public static String getPathToTrim(String[] paths)
        {
            string pathToTrim = "";

            try
            {
                String aPath = paths.First();
                
                string[] dirsInFilePath = aPath.Split('\\');
                int highestMatchingIndex = 0;

                for (int i = 0; i < dirsInFilePath.Length; i++)
                {

                    for (int j = 0; j < paths.Length; j++)
                    {
                        string[] splitPath = paths[j].Split('\\');
                        if (!dirsInFilePath[i].Equals(splitPath[i]))
                        {
                            highestMatchingIndex = i - 1;
                            break;
                        }

                    }
                }

                if (highestMatchingIndex == 0)
                {
                    pathToTrim =  "." + "\\";
                }
                else
                {
                    pathToTrim = string.Join("\\", dirsInFilePath, 0, highestMatchingIndex);
                }

            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("No Entrypoints with the specified search parameters found.");
            }

            catch( Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return pathToTrim;
           
        }
    }

   
}
    
    


