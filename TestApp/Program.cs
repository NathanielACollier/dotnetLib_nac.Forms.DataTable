using System;
using System.Collections.Generic;
using nac.Forms;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = Avalonia.AppBuilder.Configure<App>()
                                .NewForm();

            mainUI(f);
        }
        
        static void mainUI(Form f)
        {
            model.TestEntry selectedTestEntry = null;
            // setup test methods
            var methods = new List<model.TestEntry>
            {
                new model.TestEntry
                {
                    Name = "Test DataTable: Display Observable Collection",
                    CodeToRun = lib.TestFunctions.TestDataTable_DisplayObservableCollection
                }
            };
            f.SimpleDropDown(methods, (i) => {
                try
                {
                    selectedTestEntry = i;
                    i.CodeToRun(f);
                }
                catch (Exception ex)
                {
                    writeLineError($"Error, trying [{i.Name}].  Exception: {ex}");
                }

            })
            .Button("Run", _args =>
            {
                try
                {
                    selectedTestEntry.CodeToRun(f);
                }catch(Exception ex)
                {
                    writeLineError($"Error, manually running {selectedTestEntry?.Name ?? "NULL"}.  Exception: {ex}");
                }
            })
            .Display();
        }
        
        
        private static void writeLineError(string message)
        {
            var originalForeground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = originalForeground;
        }
    }
}