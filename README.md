# TripicsSkControls

A modern, customizable set of .NET MAUI controls for building rich, interactive UIs with SkiaSharp rendering.  
Includes advanced list, card, and toolbar controls, designed for flexibility and MVVM support.

## Features

- **TSKList**: Data-driven list control with customizable columns, color modes, and toolbar/inline controls.
- **TSKCard**: Card-style container for content and lists.
- **Toolbar Controls**: Easily add, style, and bind commands to toolbar buttons.
- **Inline Controls**: (Planned) Support for row-level actions.
- **SkiaSharp Rendering**: High-performance, pixel-perfect drawing.
- **MVVM Friendly**: Bindable properties and ICommand support.

## Getting Started

### 1. Add the Library

Reference the `TripicsSkControls` MAUI library in your project.

### 2. Usage Example
using TripicsSkControls.Controls;
// Create columns var columns = new List<TSKList.TSKListColumn> { new() { Header = "Id", Binding = "Id", DataType = typeof(int) }, new() { Header = "Name", Binding = "Name", DataType = typeof(string) }, new() { Header = "Active", Binding = "Active", DataType = typeof(bool) } };
// Create toolbar controls var toolbarControls = new List<TSKList.TSKToolbarControl> { new() { Control = TSKList.Controls.Edit, tabBackgroundColor = Colors.DeepSkyBlue, tabTextColor = Colors.Yellow }, new() { Control = TSKList.Controls.Delete, tabBackgroundColor = Colors.DarkRed, tabTextColor = Colors.WhiteSmoke } };
// Create the list control var tskList = new TSKList { Columns = columns, ListColumnNames = new List<string> { "User ID", "Full Name", "Is Active?" }, ListItemsSource = new List<IList<object>> { new List<object> { 1, "Alice", true }, new List<object> { 2, "Bob", false } }, ToolBarControls = toolbarControls, ToolbarCommands = new Dictionary<TSKList.Controls, ICommand> { { TSKList.Controls.Edit, EditCommand }, { TSKList.Controls.Delete, DeleteCommand } } };

### 3. See the Samples

Check the `/samples` folder for a full-featured MAUI demo project.

## API Reference

### TSKList

- **Columns**: List of column definitions.
- **ListColumnNames**: Display names for columns.
- **ListItemsSource**: Data rows.
- **ToolBarControls**: List of toolbar button definitions.
- **ToolbarCommands**: Dictionary mapping controls to ICommand.
- **ListColorMode**: SingleColor, AlternatingColors, StatusColors.
- **DisplayMode**: TextOnly, CheckBoxes, RadioButtons, CircleIndicators.

### TSKCard

- Card container for content and lists.

## Customization

- **Colors**: Set primary, secondary, good, warning, and danger colors.
- **Font Sizes**: Customize header and item font sizes.
- **Toolbar**: Style each button individually.

## Roadmap

- Inline controls for row-level actions.
- More card and list features.
- Expanded documentation and samples.

## Contributing

Pull requests and issues are welcome!  
Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

## License

MIT License. See [LICENSE](LICENSE.txt) for details.

---

**For more details, see the samples and code comments. If you need help, open an issue!**
