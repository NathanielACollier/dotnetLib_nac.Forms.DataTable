using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Templates;
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
                    else
                    {
                        var col = new Avalonia.Controls.DataGridTemplateColumn();
                        col.Header = c.Header;
                        col.CellTemplate = new FuncDataTemplate<object>((itemModel, nameScope) =>
                        {
                            nac.Forms.Form rowForm = null;
                            f._Extend_AccessApp(app =>
                            {
                                rowForm = new Form(__app: app, _model: new lib.BindableDynamicDictionary());
                            });
                            
                            // this has to have a unique model
                            rowForm.Model[nac.Forms.model.SpecialModelKeys.DataContext] = itemModel;
                            c.template(rowForm);

                            // get access to host via extend
                            Avalonia.Controls.Grid host = null;
                            f._Extend_AccessHost(_host =>
                            {
                                host = _host;
                                host.DataContext = itemModel;
                            });

                            return host;
                        });
                        dg.Columns.Add(col);
                    }
                }
            }

            if (!(f.Model[itemsModelFieldName] is IEnumerable<T>))
            {
                throw new Exception(
                    $"Model Items source property specified by name [{itemsModelFieldName}] must be IEnumerable<T>");
            }
            
            f._Extend_AddBinding<IEnumerable<T>>(itemsModelFieldName, dg, Avalonia.Controls.DataGrid.ItemsProperty, 
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