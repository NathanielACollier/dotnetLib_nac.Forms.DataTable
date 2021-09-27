using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using nac.Forms;
using nac.Forms.model;
using TestApp.model;

namespace TestApp.lib
{
    public static class TestFunctions
    {
        public static void TestTable_DisplayObservableCollection(Form parentForm)
        {
            parentForm.DisplayChildForm(f =>
            {

                var people = new ObservableCollection<model.Person>
                {
                    new model.Person
                    {
                        First = "George",
                        Last = "Washington"
                    },
                    new model.Person
                    {
                        First = "John",
                        Last = "Adams"
                    }
                };
                f.Model["persons"] = people;
                
                f.Table<model.Person>("persons");
            });
        }


        public static void TestTable_Display2Tables(Form pareForm)
        {
            var p1 = new ObservableCollection<model.Person>
            {
                new model.Person
                {
                    First = "Ringo",
                    Last = "Star"
                },
                new model.Person
                {
                    First = "Paul",
                    Last = "McCarthy"
                }
            };

            var p2 = new ObservableCollection<model.Person>
            {
                new model.Person
                {
                    First = "Grape",
                    Last = "Fruit"
                },
                new model.Person
                {
                    First = "Orange",
                    Last = "Apple"
                }
            };

            pareForm.DisplayChildForm(f =>
            {
                f.Model["p1"] = p1;
                f.Model["p2"] = p2;
                f.VerticalGroup( vg =>
                {
                    vg.Table<model.Person>("p1")
                        .Table<model.Person>("p2");
                }, isSplit:true);
            });
        }


        public static void TestTable_AddEntryToBlankList(Form parentForm)
        {
            var people = new ObservableCollection<model.Person>();
            parentForm.DisplayChildForm(f =>
            {
                f.Model["people"] = people;
                f.Text("Person Editor")
                .VerticalGroup(vg =>
                {
                    vg.VerticalGroup(newEntryEditor =>
                    {
                        newEntryEditor.HorizontalGroup(hg =>
                            {
                                hg.Text("First Name")
                                    .TextBoxFor("firstName");
                            })
                            .HorizontalGroup(hg =>
                            {
                                hg.Text("Last Name")
                                    .TextBoxFor("lastName");
                            })
                            .Button("Add", (args) =>
                            {
                                people.Add(new Person
                                {
                                    First = f.Model["firstName"] as string,
                                    Last = f.Model["lastName"] as string
                                });
                            });
                    }).Table<model.Person>("people");
                }, isSplit:true);


            });
        }

        public static void TestTable_ObservableCollectionOfDictionary(Form parentForm)
        {
            var list = new ObservableCollection<Dictionary<string, object>>();
            
            parentForm.DisplayChildForm(f =>
            {
                f.Model["list"] = list;

                f.HorizontalGroup(hg =>
                    {
                        hg.Text("First Name")
                            .TextBoxFor("firstName");
                    })
                    .Button("Add", (args) =>
                    {
                        list.Add(new Dictionary<string, object>
                        {
                            {"First Name", f.Model["firstName"] as string}
                        });
                    })
                    .Table<Dictionary<string, object>>("list");

            });
        }

        public static void TestTable_ObservableCollectionBindableDictionary(Form parentForm)
        {
            var list = new ObservableCollection<nac.Forms.lib.BindableDynamicDictionary>();
            
            parentForm.DisplayChildForm(f =>
            {
                f.Model["list"] = list;

                var firstItem = new nac.Forms.lib.BindableDynamicDictionary();
                firstItem["firstName"] = "Apple";
                list.Add(firstItem);

                f.HorizontalGroup(hg =>
                    {
                        hg.Text("First Name")
                            .TextBoxFor("firstName");
                    })
                    .Button("Add", (args) =>
                    {
                        dynamic newItem = new nac.Forms.lib.BindableDynamicDictionary();
                        newItem.firstName = f.Model["firstName"] as string;
                        list.Add(newItem);
                    })
                    .Table<nac.Forms.lib.BindableDynamicDictionary>("list",
                        columns: new[]
                        {
                            new Column
                            {
                                Header = "First Name",
                                modelBindingPropertyName = "firstName"
                            }
                        });

            });
        }

        public static void TestTable_BasicSpecifiedColumnBinding(Form parentForm)
        {
            var list = new ObservableCollection<model.Alphabet>();
            
            parentForm.DisplayChildForm(f =>
            {
                f.Model["list"] = list;

                f.HorizontalGroup(hg =>
                    {
                        hg.Text("X")
                            .TextBoxFor("X");
                    })
                    .Button("Add", (args) =>
                    {
                        var newItem = new model.Alphabet();
                        newItem.X = f.Model["X"] as string;
                        list.Add(newItem);
                    })
                    .Table<model.Alphabet>("list",
                        columns: new[]
                        {
                            new Column
                            {
                                Header = "Duplicate of X",
                                modelBindingPropertyName = "X"
                            }
                        });

            });
        }

        public static void TestTable_BasicTemplateColumn_ButtonCounter(Form parentForm)
        {
            var list = new ObservableCollection<model.Alphabet>();
            parentForm.DisplayChildForm(f =>
            {
                f.DebugAvalonia(); // enable this child form to be debugged
                
                f.Model["list"] = list;
                f.HorizontalStack(hs =>
                {
                    hs.Text("Use this to add an item to the list: ")
                        .Button("Add Item", (_args) =>
                        {
                            var newItem = new model.Alphabet();
                            newItem.A = DateTime.Now.ToString();
                            newItem.C = "0";
                            newItem.G = "77"; // model is working if initially set
                            list.Add(newItem);
                        });
                }).Table<model.Alphabet>(itemsModelFieldName: "list", columns: new[]
                {
                    new Column
                    {
                        Header = "My Counter",
                        template = (myColTemplate) =>
                        {
                            myColTemplate.Button("Incriment", (_args) =>
                            {
                                var model = myColTemplate.DataContext as model.Alphabet;

                                var counter = string.IsNullOrWhiteSpace(model.C) ? 0 : Convert.ToInt32(model.C);
                                ++counter;
                                model.C = counter.ToString();
                            });
                        }
                    },
                    new Column
                    {
                        Header = "C (Dupe)",
                        template = (myColTemplate) =>
                        {
                            myColTemplate.TextFor("C");
                        }
                    }
                });
            });
        }

        public static void TestTable_DataContext_Test(Form parentForm)
        {
            parentForm.DisplayChildForm(f =>
            {
                var model = new model.TestDataContextWindowModel();
                f.DataContext = model;

                f.Text("Letters")
                    .HorizontalGroup(h =>
                    {
                        h.Text("A: ")
                            .TextBoxFor("NewLetter.A");
                    })
                    .HorizontalGroup(h =>
                    {
                        h.Text("B: ")
                            .TextBoxFor("NewLetter.B");
                    })
                    .Button("Add", (obj) =>
                    {
                        model.Letters.Add(model.NewLetter);
                        model.NewLetter = new model.Alphabet();
                    })
                    .Table<model.Alphabet>(itemsModelFieldName: "Letters", columns: new[]
                    {
                        new nac.Forms.model.Column
                        {
                            Header = "A",
                            modelBindingPropertyName = "A"
                        },
                        new nac.Forms.model.Column
                        {
                            Header = "B",
                            modelBindingPropertyName = "B"
                        }
                    }, autoGenerateColumns: false);
            }, useIsolatedModelForThisChildForm: true);
        }
    }
}