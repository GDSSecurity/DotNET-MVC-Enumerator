/*
*  Command Line Parser Library 
*  https://github.com/gsscoder/commandline
*/

using System;
using System.IO;
using System.Text;
using CommandLine;

namespace DotNetMVCEnumerator.source
{
    class Options
    {
        [Option('o', "output", Required = false, HelpText = "CSV Output file")]
        public string CsvOutput { get; set; }

        [Option('a', "attribute", Required = false, HelpText = "Only Return Controller Methods Set With Specified Attribute")]
        public string AttributeSearch { get; set; }

        [Option('n', "negative", Required = false, HelpText = "Only Return Controller Methods Not Set With Specified Attribute")]
        public string NegativeSearch { get; set; }

        [Option('h', "help", Required = false, HelpText = "Display available options")]
        public string Help { get; set; }

        [Option('d', "directory", Required = true, HelpText = "Directories to scan")]
        public string Directory { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            // options without using CommandLine.Text or using HelpText.AutoBuild
            var usage = new StringBuilder();
            usage.AppendLine("\nUsage: " + Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().Location) + 
                "\n\n[-d Directory To Scan  *Required]\n[-a Attribute Name to Search]\n"+
                "[-n Attribute Name To Perform Negative Search]\n"+
                "[-o File Name to Output Results as CSV]\n[-h Display Usage Help Text]\n");
            return usage.ToString();
        }

        public bool OptionParser(String[] args, out string csvOutputFile, out string attributeSearch, 
            out string directoryToScan, out string negativeSearch)
        {
            csvOutputFile = "";
            attributeSearch = "";
            directoryToScan = "";
            negativeSearch = "";
            
            if (CommandLine.Parser.Default.ParseArguments(args, this))
            {
                // consume Options instance properties
                if (!String.IsNullOrEmpty(this.CsvOutput))
                {
                    if (Path.GetExtension(this.CsvOutput).ToLower().Equals(".csv"))
                    {
                        csvOutputFile = this.CsvOutput;
                    }
                    else
                    {
                        csvOutputFile = this.CsvOutput + ".csv";
                    }
                }
                if( !String.IsNullOrEmpty(this.AttributeSearch))
                {
                    attributeSearch = this.AttributeSearch;
                }

                if( !String.IsNullOrEmpty(this.Directory) )
                {
                    directoryToScan = this.Directory;
                }

                if( !String.IsNullOrEmpty(this.NegativeSearch))
                {
                    negativeSearch = this.NegativeSearch;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
