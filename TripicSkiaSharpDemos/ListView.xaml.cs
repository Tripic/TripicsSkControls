using TripicsSkControls.Controls;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Windows.Input;
namespace TripicSkiaSharpDemos;

public partial class ListView : ContentPage
{
    public List<TSKList.TSKListColumn> Columns { get; set; }
    public List<string> ColumnNames { get; set; }
    public List<IList<object>> Items { get; set; }
    public List<TSKList.TSKListColumn> Columns1 { get; set; }
    public List<string> ColumnNames1 { get; set; }
    public List<IList<object>> Items1 { get; set; }
    public ICommand EditCommand { get; set; } = new Command(() => System.Diagnostics.Debug.WriteLine("Edit command executed"));
    public ICommand DeleteCommand { get; set; } = new Command(() => System.Diagnostics.Debug.WriteLine("Delete command executed"));
    public ICommand AddCommand { get; set; } = new Command(() => System.Diagnostics.Debug.WriteLine("Add command executed"));
    public ICommand SaveCommand { get; set; } = new Command(() => System.Diagnostics.Debug.WriteLine("Save command executed"));

    public ListView()
    {
        InitializeComponent();

        // Define columns (database names)
        Columns = new List<TSKList.TSKListColumn>
        {
            new() { Header = "Id", Binding = "Id", DataType = typeof(int) },
            new() { Header = "Name", Binding = "Name", DataType = typeof(string) },
            new() { Header = "Active", Binding = "Active", DataType = typeof(bool) }
        };
        Columns1 = new List<TSKList.TSKListColumn>
        {
            new() { Header = "Id", Binding = "Id", DataType = typeof(int) },
            new() { Header = "Name", Binding = "Name", DataType = typeof(string) },
            new() { Header = "Active", Binding = "Active", DataType = typeof(bool) }
        };
        System.Diagnostics.Debug.WriteLine("columns1 initialized ");

        // User-friendly column names
        ColumnNames = new List<string> { "User ID", "Full Name", "Is Active?" };
        ColumnNames1 = new List<string> { "User ID", "Full Name", "Is Active?" };
        System.Diagnostics.Debug.WriteLine("ColumnNames1 initialized ");

        // Sample data
        Items = new List<IList<object>>
        {
            new List<object> { 1, "Alice", true },
            new List<object> { 2, "Bob", false },
            new List<object> { 3, "Charlie", true }
        };
        Items1 = new List<IList<object>>
        {
            new List<object> { 1, "Alice", true },
            new List<object> { 2, "Bob", false },
            new List<object> { 3, "Charlie", true }
        };
        var Toolbarcontrols = new List<TSKList.TSKToolbarControl>{
         new TSKList.TSKToolbarControl{Control = TSKList.Controls.Edit, tabBackgroundColor = Colors.DeepSkyBlue , tabTextColor = Colors.Yellow},
         new TSKList.TSKToolbarControl{Control = TSKList.Controls.Delete , tabBackgroundColor = Colors.DarkRed , tabTextColor = Colors.WhiteSmoke},
         new TSKList.TSKToolbarControl{Control = TSKList.Controls.Add, tabBackgroundColor = Colors.DarkCyan , tabTextColor = Colors.White},
         new TSKList.TSKToolbarControl{Control = TSKList.Controls.Save, tabBackgroundColor = Colors.Green , tabTextColor = Colors.Black},
         
        };
        DemoList.ToolbarCommands = new Dictionary<TSKList.Controls, ICommand>
        {
            { TSKList.Controls.Edit, EditCommand},
            { TSKList.Controls.Delete, DeleteCommand},
            { TSKList.Controls.Add, AddCommand },
            { TSKList.Controls.Save, SaveCommand}
        };
        DemoList.ToolBarControls = Toolbarcontrols;
        BindingContext = this;
        // debug with message including which Demo carf is initialized and has data binding contextset
       DemoList3.ContentBoundingBox = Democard1.ContentBoundingBox; // Set the bounding box for DemoList3 to match Democard1
        DemoList3.BindingContext = this;
        if (DemoList3.BindingContext != null
            )
        {
            //DemoList3.BindingContext = this;
            System.Diagnostics.Debug.WriteLine($"Democard {DemoList3.GetType().Name} initialized with data binding context set.");
            //debug Demolist 3 binding context tcontent
            System.Diagnostics.Debug.WriteLine($"DemoList3 BindingContext: {DemoList3.BindingContext}, Content: {DemoList3.Content}");
            DemoList3.SetDataReady(true); System.Diagnostics.Debug.WriteLine("DemoList3 SetDataReady called with true");
           // Democard1.SetDataReady(true); System.Diagnostics.Debug.WriteLine("Democard1 SetDataReady called with true");
        }
        
   
        // Set the BindingContext so XAML bindings work
        

        // Create the TSKList control
        var tskList = new TSKList
        {
            IsContentReady = true,
            
            
            Columns = Columns,
            ListColumnNames = ColumnNames,
            ListItemsSource = Items,
            ListColorMode = TSKList.ColorMode.AlternatingColors,
            DisplayMode = TSKList.DataDisplayMode.CheckBoxes,
            PrimaryColor = Colors.White,
            SecondaryColor = Colors.LightGray,
            GoodColor = Colors.Green,
            WarningColor = Colors.Orange,
            DangerColor = Colors.Red,
            ListHeaderFontSize = 16,
            ListItemFontSize = 14,
            ListBorderColor = Colors.Black,
            
            StatusSelector = row =>
            {
                // Example: Use "Active" column to determine status
                if (row.Count > 2 && row[2] is bool active)
                    return active ? "Good" : "Danger";
                return "Warning";
            }
        };

        // Add to the page
        
    }
}