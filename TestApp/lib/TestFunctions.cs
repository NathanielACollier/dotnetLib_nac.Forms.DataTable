using System.Collections.ObjectModel;
using nac.Forms;

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
                
                f.Table<ObservableCollection<model.Person>>("persons");
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
                f.VerticalGroupSplit(vg =>
                {
                    vg.Table<ObservableCollection<model.Person>>("p1")
                        .Table<ObservableCollection<model.Person>>("p2");
                });
            });
        }
        
        
        
        
        
        
        
    }
}