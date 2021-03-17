using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Styling;

namespace nac.Forms
{
    public static class Extensions
    {

        public static Form Table<T>(this Form f,
                                string itemsModelFieldName,
                                IEnumerable<model.Column> columns = null,
                                bool autoGenerateColumns = true)
        {
            f._Extend_AccessApp(app =>
            {
                if (!isDataGridStyleInApp(app))
                {
                    addDataGridStyleToApp(app);
                }
            });
            
            var dg = new Avalonia.Controls.DataGrid();
            dg.AutoGenerateColumns = autoGenerateColumns;

            if (columns != null)
            {
                foreach (var c in columns)
                {
                    if (c.template == null)
                    {
                        var dgCol = new Avalonia.Controls.DataGridTextColumn();
                        dgCol.Header = c.Header;
                        dgCol.Binding = new Binding
                        {
                            Path = c.modelBindingPropertyName
                        };
                        dg.Columns.Add(dgCol);
                    }
                }
            }
            
            f._Extend_AddBinding<T>(itemsModelFieldName, dg, Avalonia.Controls.DataGrid.ItemsProperty, 
                isTwoWayDataBinding: true);
            f._Extend_AddRowToHost(dg, rowAutoHeight: false);

            return f;
        }

        private static void addDataGridStyleToApp(Application app)
        {
            // there is a bug in avalonia.  see: https://github.com/AvaloniaUI/Avalonia/issues/3788
            var datagridStyleUri = new Uri("avares://Avalonia.Controls.DataGrid/Themes/Default.xaml");
            var _style = new StyleInclude(datagridStyleUri) {
                Source = datagridStyleUri
            };
            app.Styles.Add(_style);
        }

        private static bool isDataGridStyleInApp(Application app)
        {
            var datagridStyleQuery = app.Styles
                .OfType<StyleInclude>()
                .Where(s => (s?.Source?.ToString() ?? "")
                            .IndexOf("/Avalonia.Controls.DataGrid/", StringComparison.OrdinalIgnoreCase) >=
                            0);

            return datagridStyleQuery.Any();
        }
    }
}