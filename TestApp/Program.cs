using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Logging;
using nac.Forms;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = Avalonia.AppBuilder.Configure<App>()
                .NewForm(beforeAppBuilderInit: (appBUilder) =>
                {
                    appBUilder.LogToTrace(LogEventLevel.Debug);
                })
                .DebugAvalonia();

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
                    Name = "Test Table: Display Observable Collection",
                    CodeToRun = lib.TestFunctions.TestTable_DisplayObservableCollection
                },
                new model.TestEntry
                {
                    Name = "Test Table: Display 2 Tables",
                    CodeToRun = lib.TestFunctions.TestTable_Display2Tables
                },
                new model.TestEntry
                {
                    Name = "Test Table: Add entry to blank list",
                    CodeToRun = lib.TestFunctions.TestTable_AddEntryToBlankList
                },
                new model.TestEntry
                {
                    Name = "Test Table: Observable Collection of Dictionary",
                    CodeToRun = lib.TestFunctions.TestTable_ObservableCollectionOfDictionary
                },
                new model.TestEntry
                {
                    Name = "Test Table: Display Observable Collection of Bindable Dictionary",
                    CodeToRun = lib.TestFunctions.TestTable_ObservableCollectionBindableDictionary
                },
                new model.TestEntry
                {
                    Name = "Test Table: Basic Specified Column Binding",
                    CodeToRun = lib.TestFunctions.TestTable_BasicSpecifiedColumnBinding
                },
                new model.TestEntry()
                {
                    Name = "Test Table: Basic Template Column (Button Counter)",
                    CodeToRun = lib.TestFunctions.TestTable_BasicTemplateColumn_ButtonCounter
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