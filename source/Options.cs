/*
*  https://github.com/gsscoder/commandline
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConsoleApplication1.source
{
    class Options
    {
        [Option('o', "output", Required = false, HelpText = "Output File to write")]
        public string CsvOutput { get; set; }

        [Option('a', "attribute", Required = false, HelpText = "Specify the Attribute")]
        public string AttrOutputFile { get; set; }

        [Option('h', "help", Required = false, HelpText = "Specify the Attribute")]
        public string Help { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // options without using CommandLine.Text or using HelpText.AutoBuild
            var usage = new StringBuilder();
            usage.AppendLine("\nUsage : " + Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location) + " [-a attribute to search] [-o Output file for results in CSV]");
            return usage.ToString();
        }

        public bool OptionParser(String[] args, out string csvOutputFile, out string attribute)
        {
            csvOutputFile = "";
            attribute = "";
            
            if (CommandLine.Parser.Default.ParseArguments(args, this))
            {
                // consume Options instance properties
                if (!String.IsNullOrEmpty(this.CsvOutput))
                {
                    if (Path.GetExtension(this.CsvOutput).ToLower().Equals("csv"))
                    {
                        csvOutputFile = this.CsvOutput;
                    }
                    else
                    {
                        csvOutputFile = this.CsvOutput + ".csv";
                    }
                }
                if (!String.IsNullOrEmpty(this.AttrOutputFile))
                {
                    attribute = this.AttrOutputFile;
                }
            }
            else
            {
                // Display the default usage information
                //Console.WriteLine();
                return false;
            }
            return true;
        }
    }
}
