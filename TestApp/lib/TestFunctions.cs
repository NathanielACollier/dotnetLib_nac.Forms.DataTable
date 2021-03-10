using System.Collections.ObjectModel;
using nac.Forms;

namespace TestApp.lib
{
    public static class TestFunctions
    {
        public static void TestDataTable_DisplayObservableCollection(Form parentForm)
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
        
        
        
        
        
    }
}