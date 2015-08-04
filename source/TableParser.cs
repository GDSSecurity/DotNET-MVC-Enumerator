using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

/*
* tableParser() - http://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c
* The least popular, but a simple solution!
*/

namespace DotNetMVCEnumerator.source
{
    
    public static class TableParser
  
    {
        public static void  tableParser(DataTable resultsTable,List<string> resultList)
        {
            try
            {
                DataRow _row = resultsTable.NewRow();
                for (int i = 0; i < (resultList.ToArray().Count() - 2); i = i + 3)
                {
                    object[] r = {resultList[i].ToString(), resultList[i + 1].ToString(), resultList[i + 2].ToString()};
                    resultsTable.Rows.Add(r);
                }
                // Print top line
                Console.WriteLine(new string('-', 75));

                // Print col headers
                var colHeaders = resultsTable.Columns.Cast<DataColumn>().Select(arg => arg.ColumnName);
                foreach (String s in colHeaders)
                {
                    Console.Write("| {0,-25}", s);
                }
                Console.WriteLine();

                // Print line below col headers
                Console.WriteLine(new string('-', 75));

                // Print rows
                foreach (DataRow row in resultsTable.Rows)
                {
                    foreach (Object o in row.ItemArray)
                    {
                        Console.Write("| {0,-25}", o.ToString());
                    }
                    Console.WriteLine();
                }

                // Print bottom line
                Console.WriteLine(new string('-', 75));
            }
            catch (Exception)
            {
                Console.WriteLine("Ooops! Something went wrong!");
            }
        }

        public static void centerText(String text)
        {
            Console.Write(new string(' ', (Console.WindowWidth - text.Length) / 2));
            Console.WriteLine(text);
        }

    }
}
