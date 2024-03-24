using ConsoleApp.Model;

namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataReader
    {
        public void ImportAndPrintData(string fileToImport, bool printData = true)
        {

            var importedObjects = new List<ImportedObject>();

            var streamReader = new StreamReader(fileToImport);

            var importedLines = new List<string>();
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                importedLines.Add(line);
            }
            streamReader.Close();

            foreach (var importedLine in importedLines)
            {
                
                var values = importedLine.Split(';');
                if (values.Length == 7)
                {
                    var importedObject = new ImportedObject
                    {
                        Type = values[0]?.Trim(),
                        Name = values[1]?.Trim(),
                        Schema = values[2]?.Trim(),
                        ParentName = values[3]?.Trim(),
                        ParentType = values[4]?.Trim(),
                        DataType = values[5]?.Trim(),
                        IsNullable = values[6]?.Trim()
                    };
                    importedObjects.Add(importedObject);
                }
            }

            // clear and correct imported data
            foreach (var importedObject in importedObjects)
            {
                importedObject.Type = importedObject.Type.Replace(" ", "").Replace(Environment.NewLine, "").ToUpper();
                importedObject.Name = importedObject.Name.Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.Schema = importedObject.Schema.Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.ParentName = importedObject.ParentName.Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.ParentType = importedObject.ParentType.Replace(" ", "").Replace(Environment.NewLine, "");
            }

            // assign number of children
            for (int i = 0; i < importedObjects.Count(); i++)
            {
                var importedObject = importedObjects.ToArray()[i];
                foreach (var impObj in importedObjects)
                {
                    if (impObj.ParentType == importedObject.Type)
                    {
                        if (impObj.ParentName == importedObject.Name)
                        {
                            importedObject.NumberOfChildren = 1 + importedObject.NumberOfChildren;
                        }
                    }
                }
            }

            foreach (var database in importedObjects)
            {
                if (database.Type == "DATABASE" || database.ParentType == "Database")
                {
                    Console.WriteLine($"Database '{database.Name}' ({database.NumberOfChildren} tables)");

                    // print all database's tables
                    foreach (var table in importedObjects)
                    {
                        if (table.ParentType.ToUpper() == database.Type)
                        {
                            if (table.ParentName == database.Name)
                            {
                                Console.WriteLine($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");

                                // print all table's columns
                                foreach (var column in importedObjects)
                                {
                                    if (column.ParentType.ToUpper() == table.Type)
                                    {
                                        if (column.ParentName == table.Name)
                                        {
                                            Console.WriteLine($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable == "1" ? "accepts nulls" : "with no nulls")}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
