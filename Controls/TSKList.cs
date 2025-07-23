using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.ComponentModel.Design;
using Microsoft.Maui.Animations;

//using System.ComponentModel.DataAnnotations;

namespace TripicsSkControls.Controls;

public class TSKList : TSkBase
{
    public Func<IList<object>, string>? StatusSelector { get; set; }
    #region Enumerations
    public enum DataDisplayMode
    {
        TextOnly,
        CheckBoxes,
        RadioButtons,
        CircleIndicators,
    }
	public enum ColorMode 
	{
		SingleColor,
        AlternatingColors,
        StatusColors // Good, Warning, Danger
    }
	public enum ControlMode
	{
		None,
		ToolBarOnly,
		InlineOnly,
        Combined,
    }
	public enum Controls
			{
		Edit,
		Delete,
		Up,
		Down,
		Save,
        Add,           // Add new item/row
        Refresh,       // Reload data
        Export,        // Export list (CSV, Excel, etc.)
        Import,        // Import data
        Search,        // Search/filter
        Filter,        // Advanced filtering
        Print,         // Print the list
        View,          // View details
        Duplicate,     // Duplicate row/item
        Approve,       // Approve item (workflow)
        Reject,        // Reject item (workflow)
        Lock,          // Lock item/row
        Unlock,        // Unlock item/row
        Info,          // Show info/details
        History,       // View change history
        Settings,      // List or item settings
        Custom1,       // Reserved for custom actions
        Custom2,
    }
    #endregion
    #region Bindable Properties


    public static readonly BindableProperty ListColorModeProperty = BindableProperty.Create(
		nameof(ListColorMode),
		typeof(ColorMode),
		typeof(TSKList),
		defaultValue: ColorMode.SingleColor,
		defaultBindingMode: BindingMode.OneWay
		);
	public static readonly BindableProperty IsInCardProperty = BindableProperty.Create(
		nameof(IsInCard),
		typeof(bool),
		typeof(TSKList),
		defaultValue: false,
		defaultBindingMode: BindingMode.OneWay
		);
	public static readonly BindableProperty CardHeightProperty = BindableProperty.Create(
		nameof(CardHeight),
		typeof(float),
		typeof(TSKList),
		defaultValue: 100f,
		defaultBindingMode: BindingMode.OneWay,
		propertyChanged: CardHeightPropertyChanged
        ); public static readonly BindableProperty ListControlModeProperty = BindableProperty.Create(
		nameof(ListControlMode),
		typeof(ControlMode),
		typeof(TSKList),
	
		defaultBindingMode: BindingMode.OneWay
		);
    public static readonly BindableProperty ListControlsProperty = BindableProperty.Create(
		nameof(ListControls),
		typeof(List<Controls>),
		typeof(TSKList),
		defaultBindingMode: BindingMode.OneWay
		);
	public static readonly BindableProperty InlineControlsProperty = BindableProperty.Create(
		nameof(InlineControls),
		typeof(List<Controls>),
		typeof(TSKList),
		defaultValue: new List<Controls>(),
		defaultBindingMode: BindingMode.OneWay
		);
	public static readonly BindableProperty ToolBarControlsProperty = BindableProperty.Create(
		nameof(ToolBarControls),
		typeof(List<TSKToolbarControl>),
		typeof(TSKList),
		defaultValue: new List<TSKToolbarControl>(),
		defaultBindingMode: BindingMode.OneWay
		);
    public static readonly BindableProperty ListColumnNamesProperty = BindableProperty.Create(
		nameof(ListColumnNames),
		typeof(List<string>),
		typeof(TSKList),
		defaultValue: new List<string>(),
		defaultBindingMode: BindingMode.OneWay,
		propertyChanged: ColumnNamesPropertyChanged
    );


	public static readonly BindableProperty ListItemsSourceProperty = BindableProperty.Create(
		nameof(ListItemsSource),
		typeof(List<IList<object>>),
		typeof(TSKList),
		defaultValue: new List<IList<object>>(),
		defaultBindingMode: BindingMode.OneWay
	);
    public static readonly BindableProperty ColumnsProperty = BindableProperty.Create(
    nameof(Columns),
    typeof(List<TSKListColumn>),
    typeof(TSKList),
    defaultValue: new List<TSKListColumn>(),
    defaultBindingMode: BindingMode.OneWay
);
    public static readonly BindableProperty GoodColorProperty = BindableProperty.Create(
		nameof(GoodColor),
		typeof(Color),
		typeof(TSKList),
        defaultValue: Colors.Green,
		defaultBindingMode: BindingMode.OneWay
        );
	public static readonly BindableProperty WarningColorProperty = BindableProperty.Create(
		nameof(WarningColor),
		typeof(Color),
		typeof(TSKList),
        defaultValue: Colors.Yellow,
		defaultBindingMode: BindingMode.OneWay
		);
	public static readonly BindableProperty DangerColorProperty = BindableProperty.Create(
		nameof(DangerColor),
		typeof(Color),
		typeof(TSKList),
        defaultValue: Colors.Red,
		defaultBindingMode: BindingMode.OneWay
		);
	public static readonly BindableProperty PrimaryColorProperty = BindableProperty.Create(
		nameof(PrimaryColor),
		typeof(Color),
		typeof(TSKList),
		defaultValue: Colors.WhiteSmoke,
		defaultBindingMode: BindingMode.OneWay
			);
	public static readonly BindableProperty SecondaryColorProperty = BindableProperty.Create(
		nameof(SecondaryColor),
		typeof(Color),
		typeof(TSKList),
        defaultValue: Colors.LightGray,
		defaultBindingMode: BindingMode.OneWay
		);
	public static readonly BindableProperty ListItemFontSizeProperty = BindableProperty.Create(
		nameof(ListItemFontSize),
		typeof(float),
		typeof(TSKList),
		defaultValue: 12f,
		defaultBindingMode: BindingMode.OneWay
		);
	public static readonly BindableProperty ListHeaderFontSizeProperty = BindableProperty.Create(
		nameof(ListHeaderFontSize),
		typeof(float),
		typeof(TSKList),
		defaultValue: 12f,
		defaultBindingMode: BindingMode.OneWay
		);
    public static readonly BindableProperty DisplayModeProperty = BindableProperty.Create(
        nameof(DisplayMode),
        typeof(TSKList.DataDisplayMode),
        typeof(TSKList),
        defaultValue: TSKList.DataDisplayMode.TextOnly,
        defaultBindingMode: BindingMode.OneWay,
		propertyChanged: DisplaymodeChanged
    );
	public static readonly BindableProperty ListBorderColorProperty = BindableProperty.Create(
		nameof(ListBorderColor),
		typeof(Color),
		typeof(TSKList),
		defaultValue: Colors.Black,
		defaultBindingMode: BindingMode.OneWay
	);
	public static readonly BindableProperty ListBorderWidthProperty = BindableProperty.Create(
        nameof(ListBorderWidth),
        typeof(float),
        typeof(TSKList),
        defaultBindingMode: BindingMode.OneWay
        );
    #endregion

    #region Properties getters and setters
    public ControlMode ListControlMode
	{
		get => (ControlMode)GetValue(ListControlModeProperty);
		set => SetValue(ListControlModeProperty, value);
	}
    public List<Controls> ListControls
	{
		get => (List<Controls>)GetValue(ListControlsProperty);
		set => SetValue(ListControlsProperty, value);
	}
    public List<Controls> InlineControls
	{
		get => (List<Controls>)GetValue(InlineControlsProperty);
		set => SetValue(InlineControlsProperty, value);
	}
    public List<TSKToolbarControl> ToolBarControls
	{
		get => (List<TSKToolbarControl>)GetValue(ToolBarControlsProperty);
		set => SetValue(ToolBarControlsProperty, value);
    }
    public  Color ListBorderColor
		{
			get => (Color)GetValue(ListBorderColorProperty);
			set => SetValue(ListBorderColorProperty, value);
    }
    public SKRect ContentBoundingBox { get; set; }
	
    public ColorMode ListColorMode
		{
			get => (ColorMode)GetValue(ListColorModeProperty);
			set => SetValue(ListColorModeProperty, value);
		}

    public List<TSKListColumn> Columns
		{
			get => (List<TSKListColumn>)GetValue(ColumnsProperty);
			set => SetValue(ColumnsProperty, value);
		}

    public List<string> ListColumnNames
		{
			get => (List<string>)GetValue(ListColumnNamesProperty);
			set => SetValue(ListColumnNamesProperty, value);
		 }

	public List<IList<object>> ListItemsSource
		{
			get => (List<IList<object>>)GetValue(ListItemsSourceProperty);
			set => SetValue(ListItemsSourceProperty, value);
		}
	public float ListBorderWidth
		{
			get => (float)GetValue(ListBorderWidthProperty);
			set => SetValue(ListBorderWidthProperty, value);
    }
    public Color GoodColor
		{
			get => (Color)GetValue(GoodColorProperty);
			set => SetValue(GoodColorProperty, value);
		}
	public bool IsInCard
	{
		get => (bool)GetValue(IsInCardProperty);
		set => SetValue(IsInCardProperty, value);
	}
	public float CardHeight
	{
		get => (float)GetValue(CardHeightProperty);
		set => SetValue(CardHeightProperty, value);
    }
    public Color WarningColor
		{
			get => (Color)GetValue(WarningColorProperty);
			set => SetValue(WarningColorProperty, value);
		}
	
	public Color DangerColor
		{
			get => (Color)GetValue(DangerColorProperty);
			set => SetValue(DangerColorProperty, value);
		}

	public Color PrimaryColor
		{
			get => (Color)GetValue(PrimaryColorProperty);
			set => SetValue(PrimaryColorProperty, value);
		}

	public Color SecondaryColor
		{
			get => (Color)GetValue(SecondaryColorProperty);
			set => SetValue(SecondaryColorProperty, value);
		}

	public float ListItemFontSize
		{
			get => (float)GetValue(ListItemFontSizeProperty);
			set => SetValue(ListItemFontSizeProperty, value);
		}

	public float ListHeaderFontSize
		{
			get => (float)GetValue(ListHeaderFontSizeProperty);
			set => SetValue(ListHeaderFontSizeProperty, value);
		}

    public DataDisplayMode DisplayMode
		{
			get => (DataDisplayMode)GetValue(DisplayModeProperty);
			set => SetValue(DisplayModeProperty, value);
		}
	private List<(SKRect rect,TSKToolbarControl control)> _toolbarRects = new ();
	#endregion
	#region Property Changed Handlers
	private static void CardHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is TSKList list)
		{
			list.ContentBoundingBox = new SKRect(0, 0, 0, (float)newValue);
			list.InvalidateMeasure();
		}
    }
    #endregion
    private void OnTabTouched(TSKList.Controls control)
    {
        if (ToolbarCommands.TryGetValue(control, out var command) && command?.CanExecute(null) == true)
            command.Execute(null);
    }
    private void CanvasView_Touch(object sender, SKTouchEventArgs e)
    {
        if (e.ActionType != SKTouchAction.Pressed)
            return;
        if (ListControlMode == ControlMode.ToolBarOnly || ListControlMode == ControlMode.Combined)
        {
            foreach (var (rect, control) in _toolbarRects)
            {
                if (rect.Contains(e.Location) )
                {
                    OnTabTouched(control.Control); // Pass the enum value
                    e.Handled = true;
                    break;
                }
            }
        }
    }
    public TSKList()
	{
		System.Diagnostics.Debug.WriteLine("TSKList constructor called");
		InitializeCanvas( null,null);
        
		// oend the size request to allow the control to expand
		if (canvasView != null)
		{
			canvasView.EnableTouchEvents = true;
            canvasView.Touch += CanvasView_Touch;
		}
		System.Diagnostics.Debug.WriteLine("TSKList canvas initialized");
    }
	public class TSKToolbarControl
	{
		public TSKList.Controls Control { get; set; }
		public Color tabBackgroundColor { get; set; } = Colors.WhiteSmoke;
		public Color tabTextColor { get; set; } = Colors.Black;
    }

    public Dictionary<TSKList.Controls, ICommand> ToolbarCommands { get; set; } = new();


    protected override void DrawControl(SKCanvas canvas, SKImageInfo info)
	{
		
		var header = ListColumnNames.ToList();
      
		
		var rows = ListItemsSource.Select(row => Columns.Select((col, colIndex)=>
		{
			object value = row.Count > colIndex ? row[colIndex] : null;
			return new TSKListCell
			{
				Value = value,
				DataType = col.DataType,
				Binding = col.Binding,
				

			};
		}).ToList()
		).ToList();

        int cols = header.Count;
        int rowCount = rows.Count;
        // Prepare paint
        using var textPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = ListItemFontSize,
            IsAntialias = true
        };
        using var textFont = new SKFont
        {
            Size = ListItemFontSize,
            Typeface = SKTypeface.Default
        };
		using var TrueCirclePaint = new SKPaint
        {
            Color = SKColors.Green,
            Style = SKPaintStyle.Fill
        };
        using var headerPaint = new SKPaint
        {
            Color = SKColors.Red,
            TextSize = ListHeaderFontSize,
            IsAntialias = true,
            FakeBoldText = true
        };
        using var headerFont = new SKFont
        {
            Size = ListHeaderFontSize,
            Typeface = SKTypeface.Default
        };
		using var FalseCirclePaint = new SKPaint
		{
			Color = SKColors.Red,
			Style = SKPaintStyle.Fill
		};
        using var cellPaint = new SKPaint
        {
            Color = SecondaryColor.AsSKColor(),
            Style = SKPaintStyle.Fill
        };
		using var RadioButtonPaint = new SKPaint
		{
			Color = SKColors.Black,
			Style = SKPaintStyle.Stroke,
			StrokeWidth = 2 // Adjust stroke width as needed
                            //THIS IS ANEMPTY PAINT FOR RADIO BUTTONS, IT WILL BE USED TO DRAW THE CIRCLES FOR RADIO BUTTONS 
        };
		using  var TrueRadioButtonPaint = new SKPaint
		{
			Color = SKColors.Green,
			Style = SKPaintStyle.Fill
		};
		using var FalseRadioButtonPaint = new SKPaint
		{
			Color = SKColors.Red,
			Style = SKPaintStyle.Fill
		};
		using var Empty = new SKPaint
		{
			Color = SKColors.White,
			Style = SKPaintStyle.Fill
		};
				using var CheckMarkPaint = new SKPaint
		{
			Color = SKColors.Green,
			Style = SKPaintStyle.Stroke,
			StrokeWidth = 2
                };
        // 1. Calculate max width for each column
        float[] colWidths = new float[cols];
        for (int c = 0; c < cols; c++)
        {
            float maxWidth = header.Count > c ? headerFont.MeasureText(header[c]) : 0;
            float cellHeight2 = ListItemFontSize * 2;
            foreach (var row in rows)
            {
                float cellContentWidth = 0;
                if (row.Count > c)
                {
                    if (DisplayMode == DataDisplayMode.TextOnly)
                    {
                        string text = row[c]?.Value?.ToString() ?? "";
                        cellContentWidth = textFont.MeasureText(text);
                    }
                    else
                    {
                        // Use indicator size (cellHeight / 2) as minimum width
                        float indicatorSize = cellHeight2 / 2;
                        cellContentWidth = indicatorSize;

                        // If you want to support mixed content, also check for text
                        string text = row[c]?.Value?.ToString() ?? "";
                        cellContentWidth = Math.Max(cellContentWidth, textFont.MeasureText(text));
                    }
                }
                if (cellContentWidth > maxWidth)
                    maxWidth = cellContentWidth;
            }
            // Add padding (e.g., 20)
            colWidths[c] = maxWidth + 20;
        }
		System.Diagnostics.Debug.WriteLine($"Max Width: {string.Join(", ", colWidths)}");
        // 2. Calculate total width and cell heights
        float totalWidth = colWidths.Sum();
				System.Diagnostics.Debug.WriteLine($"Total Width: {totalWidth}");
        float cellHeight1 = ListItemFontSize * 2;
        float headerHeight = ListHeaderFontSize * 2;
		float contentHeight = rowCount * cellHeight1+ headerHeight +10;
		float DisplayHeight = DisplayMode switch
		{
			DataDisplayMode.TextOnly => cellHeight1,
			DataDisplayMode.CheckBoxes => cellHeight1,
			DataDisplayMode.RadioButtons => cellHeight1,
			DataDisplayMode.CircleIndicators => cellHeight1,
			_ => cellHeight1
		};
		
		float ListBorderWidth = 2; // Default border width
		if (ListBorderWidth <= 0)
			ListBorderWidth = 1; // Ensure minimum border width
								 // Calculate canvas height based on row count and header height
		//float canvasheight;
        // Calculate total height with header and content
        float totalHeight = contentHeight + 10;
		float toalWidthWithBorder = totalWidth + ListBorderWidth +ListBorderWidth; // Add padding for border
        float startX = (info.Width - totalWidth) / 2f;
        float startY = (info.Height - totalHeight ) / 2f +10;
       
       

        // create padding around the List Background 

        // Draw the List Background and Border
        using var ListBackgroundPaint = new SKPaint
        {
            Color = PrimaryColor.AsSKColor(),
            Style = SKPaintStyle.Fill

        };
		using var ListBorderPaint = new SKPaint
		{
			Color = ListBorderColor.AsSKColor(),
			Style = SKPaintStyle.Stroke,
			StrokeWidth = ListBorderWidth
		};
        float canvasheight = DisplayHeight * rowCount + headerHeight + ListBorderWidth + ListBorderWidth;
        float x = startX - 5, y = startY -20;
		float cannvasX = info.Width / 2f  - toalWidthWithBorder / 2f;
		float canvasY = info.Height /2f - (totalHeight +headerHeight +headerHeight +ListBorderWidth +ListBorderWidth ) / 2f  ; // Adjust for header height and padding
		var BoundingBox = ContentBoundingBox;
		System.Diagnostics.Debug.WriteLine($"BoundingBox top {BoundingBox.Top}, left {BoundingBox.Left}, right {BoundingBox.Right}, bottom {BoundingBox.Bottom}");
        if (IsInCard == true && ContentBoundingBox != null)
		{ System.Diagnostics.Debug.WriteLine($"StartY {BoundingBox.Top}, StartX {BoundingBox.Left}, EndY {BoundingBox.Bottom}, EndX {BoundingBox.Right}");
            float CardContetHeight = BoundingBox.Height ;
			float CardContentTop = BoundingBox.Top;
			float CardContentBottom = BoundingBox.Bottom;
			System.Diagnostics.Debug.WriteLine($"Card Content Height: {CardContetHeight}");
			System.Diagnostics.Debug.WriteLine($"Card Content Top: {CardContentTop}");
			System.Diagnostics.Debug.WriteLine($"CanvasHeight: {canvasheight}");
            System.Diagnostics.Debug.WriteLine($"CanvasY before adjustment: {canvasY}");
            float cardAdjustment = (CardContetHeight  + canvasheight ) /4 ;
			System.Diagnostics.Debug.WriteLine($"Diference in Height: {cardAdjustment}");
            float YAdjustment = (cardAdjustment /2) ;
			System.Diagnostics.Debug.WriteLine($"Card AdjustmentSplittling the value : {YAdjustment}");
            canvasY += YAdjustment ;
            // Adjust canvasY to center the list within the card
	
            System.Diagnostics.Debug.WriteLine($"CanvasY: After {canvasY}");


        }
            if (ListControlMode == ControlMode.ToolBarOnly || ListControlMode == ControlMode.Combined)
        {
			// Add space for toolbar if applicable
			canvasheight += headerHeight;
        }
    
        //draw rounded rectangle
        //float toalWidthWithBorder = totalWidth + 10; // Add padding for border
        canvas.DrawRoundRect(new SKRect(cannvasX , canvasY , cannvasX + toalWidthWithBorder, canvasY + canvasheight  ), 10, 10, ListBackgroundPaint);
        canvas.DrawRoundRect(new SKRect(cannvasX, canvasY, cannvasX + toalWidthWithBorder, canvasY + canvasheight), 10, 10, ListBorderPaint);
        
        #region controls drawing Logic 
        // Draw controls based on ListControlMode
        if (ListControlMode == ControlMode.ToolBarOnly || ListControlMode == ControlMode.Combined)
		{
            float toolbarPadding = 2; // Padding around toolbar
            float Toolstart = canvasY + ListBorderWidth +ListBorderWidth;
            float tooly = Toolstart + toolbarPadding ;
            foreach (var control in ToolBarControls)
			{
				var controls = ToolBarControls ?? new List<TSKToolbarControl>();
				int controlCount = controls.Count;
				float toolbarHeight = headerHeight; // Use header height for toolbar
			
			
                float availableWidth = totalWidth -2 * toolbarPadding;
				float minControlWidth = 48; // Minimum width for each control
                int buttonsPerRow = controlCount == 0 ? 1 : Math.Max(1, (int)Math.Floor(availableWidth / minControlWidth));
				int toolrowCount = controlCount == 0 ? 1 : Math.Max(1, (int)Math.Ceiling((double)controlCount / buttonsPerRow));
				float tabHeight = toolbarHeight;
				_toolbarRects.Clear(); // Clear previous toolbar rects
                for (int row = 0; row < toolrowCount; row++)
				{
					int startIndex = row * buttonsPerRow;
					int endIndex = Math.Min(startIndex + buttonsPerRow, controlCount);
					int buttonsInRow = endIndex - startIndex;
					float buttonWidth = availableWidth / buttonsInRow;// Calculate width for each button in the row
					float buttonX =cannvasX + ListBorderWidth + toolbarPadding;
					
                    for (int i = startIndex; i < endIndex; i++)
					{
						var Tcontrol = controls[i];
						var rect = new SKRect(buttonX, tooly, buttonX + buttonWidth, tooly + tabHeight);
						_toolbarRects.Add((rect, Tcontrol));
                        using var TabPaint = new SKPaint
						{
							Color = Tcontrol.tabBackgroundColor.AsSKColor(),
							Style = SKPaintStyle.Fill
						};
						using var TabBorderPaint = new SKPaint
						{
							Color = ListBorderColor.AsSKColor(),
							Style = SKPaintStyle.Stroke,
							StrokeWidth = 2
						};
                        using var TabTextPaint = new SKPaint
                        {
                            Color = Tcontrol.tabTextColor.AsSKColor(),
                            TextSize = ListHeaderFontSize,
                            IsAntialias = true,
                            FakeBoldText = true
                        };
                        canvas.DrawRoundRect(rect, 10, 10, TabPaint);
						canvas.DrawRoundRect(rect, 10, 10, TabBorderPaint);
						var label = Tcontrol.Control.ToString(); // Use the enum name as label
                        var textBounds = new SKRect();
						headerPaint.MeasureText(label, ref textBounds);
						float textX = buttonX + (buttonWidth - TabTextPaint.MeasureText(label)) / 2;
                        float textY = tooly + (tabHeight + textBounds.Height) / 2;
						canvas.DrawText(label, textX, textY, TabTextPaint);
						buttonX += buttonWidth;
                        
                    }
                    

                    System.Diagnostics.Debug.WriteLine($"Drawing toolbar row {row + 1}/{toolrowCount} at Y: {startY} with {buttonsInRow} buttons. Buttons include {string.Join(", ", controls.Skip(startIndex).Take(buttonsInRow))}");
                    startY = tooly + tabHeight - (tabHeight /8) ;
                }

                    
			}
        }
		#endregion
		// draw the Background and border of the list
		if (ListControlMode == ControlMode.InlineOnly || ListControlMode == ControlMode.None)
		{
			startY = canvasY + ListBorderWidth + ListBorderWidth;
		}

            // 3. Draw header
            x = startX;
        for (int c = 0; c < cols; c++)
        {
            var rect = new SKRect(x, startY, x + colWidths[c], startY + headerHeight);
            canvas.DrawRect(rect, cellPaint);
            if (header.Count > c)
            {
                float textX = x + 10;
                float textY = startY + headerHeight / 2 + ListHeaderFontSize / 2 - headerPaint.FontMetrics.Descent / 2;
                canvas.DrawText(header[c], textX, textY - 3, headerPaint);
            }
            x += colWidths[c];
        }
        // Draw header border
		using var headerBorderPaint = new SKPaint
					{
			Color = ListBorderColor.AsSKColor(),
			StrokeWidth = 1,
			Style = SKPaintStyle.Stroke
		};
		//calculate border for header row
		
		float headerendx =  colWidths.Sum();

        float cellHeight = ListItemFontSize * 2;
        // Draw rows with background colors
        for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
		{
			float rowy = startY  + headerHeight + rowIndex * cellHeight;
			var row = rows[rowIndex];
			for (int colIndex = 0; colIndex < cols; colIndex++)
			{
				float x1 = startX + colWidths.Take(colIndex).Sum();
				var cell = row[colIndex];

				// Determine row background color based on ListColorMode
				SKColor rowColor = PrimaryColor.AsSKColor(); // Default
				switch (ListColorMode)
				{
					case ColorMode.SingleColor:
						rowColor = PrimaryColor.AsSKColor();
						break;
					case ColorMode.AlternatingColors:
						rowColor = (rowIndex % 2 == 0) ? PrimaryColor.AsSKColor() : SecondaryColor.AsSKColor();
						break;
					case ColorMode.StatusColors:
						if (StatusSelector != null)
						{
							var status = StatusSelector(ListItemsSource[rowIndex]);
							rowColor = status switch
							{
								"Good" => GoodColor.AsSKColor(),
								"Warning" => WarningColor.AsSKColor(),
								"Danger" => DangerColor.AsSKColor(),
								_ => SecondaryColor.AsSKColor()
							};
						}
						break;
				}

				using var rowPaint = new SKPaint { Color = rowColor, Style = SKPaintStyle.Fill };
				float rowY = startY + headerHeight - 10 +  rowIndex * cellHeight;
				float rowX = startX;
				float rowWidth = colWidths.Sum();
				using var rowBorderPaint = new SKPaint
				{
					Color = ListBorderColor.AsSKColor(),
					StrokeWidth = 1,
					Style = SKPaintStyle.Stroke
				};
				// Draw row background
				canvas.DrawRect(new SKRect(rowX, rowY, rowX + rowWidth, rowY + cellHeight), rowPaint);
				canvas.DrawRect(new SKRect(rowX, rowY, rowX + rowWidth, rowY + cellHeight), rowBorderPaint);
			}
		}
        canvas.DrawRect(new SKRect(startX, startY - headerHeight / 2 + 19, startX + colWidths.Sum(), startY + headerHeight - 10), headerBorderPaint);

        for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            float rowy = startY + headerHeight + rowIndex * cellHeight;
            var row = rows[rowIndex];
            for (int colIndex = 0; colIndex < cols; colIndex++)
            {
                float x1 = startX + colWidths.Take(colIndex).Sum();
                var cell = row[colIndex];


				float diameter = cellHeight / 2;
                float height = cellHeight / 2;
                float width = colWidths[colIndex] / 2;
				float boxheight1 = cellHeight / 4;
                float boxRight = x1 + width + boxheight1;
                float boxLeft = x1 + width - boxheight1 ;
				float boxBottom = (rowy + (boxheight1 / 2)) + boxheight1 ;
                float boxWidth = boxRight - boxLeft;
                float boxTop = (rowy + (boxheight1 /2)) - boxheight1 ;
				float boxHeight = cellHeight/2;
				System.Diagnostics.Debug.WriteLine($"Row {rowIndex}, Col {colIndex}, Value: {cell.Value}, BoxLeft: {boxLeft}, BoxRight: {boxRight}, BoxTop: {boxTop}, BoxBottom: {boxBottom}, BoxWidth: {boxWidth}, BoxHeight: {boxHeight}, rowy:{rowy}" );
                switch (DisplayMode)
						{
							case DataDisplayMode.TextOnly:
								string text = cell.Value?.ToString() ?? "";
								canvas.DrawText(text, x1, rowy + cellHeight / 2 , textPaint);
								break;
							case DataDisplayMode.CircleIndicators:
								if (cell.Value is bool b && b == true)
								{
									canvas.DrawCircle(x1  , rowy + diameter / 2, diameter  / 2, TrueCirclePaint);
								}
								else if (cell.Value is bool bf && bf == false)
								{
									canvas.DrawCircle(x1  , rowy + diameter / 2 , diameter / 2, FalseCirclePaint);
								}
								else
								{ // Handle other data types or null values}
									string cellText = cell.Value?.ToString() ?? "";
									canvas.DrawText(cellText, x1, rowy + diameter / 2 + ListItemFontSize / 2, textPaint);
								}
								break;
							case DataDisplayMode.RadioButtons:
								if (cell.Value is bool br && br == true)
								{
									 canvas.DrawCircle(x1 + colWidths[colIndex] / 2, rowy + diameter / 3, diameter / 2, TrueRadioButtonPaint);
									 canvas.DrawCircle(x1 + colWidths[colIndex] / 2, rowy + diameter / 3, diameter / 2, RadioButtonPaint);//draw a black outline around the green circle
								}
								else if (cell.Value is bool bf && bf == false)
								{
									canvas.DrawCircle(x1 + colWidths[colIndex] /2, rowy + diameter / 3, diameter / 2, FalseRadioButtonPaint);
									canvas.DrawCircle(x1 + colWidths[colIndex] /2, rowy + diameter / 3, diameter / 2, RadioButtonPaint);//draw a black outline around the red circle
                        }
								else
								{ // Handle other data types or null values}
									string cellText = cell.Value?.ToString() ?? "";
									canvas.DrawText(cellText, x1 , rowy + diameter / 2 + ListItemFontSize / 2, textPaint);
                        }
								break;
							case DataDisplayMode.CheckBoxes:
								if (cell.Value is bool bc && bc == true)
								{
	
                            canvas.DrawRect(new SKRect(boxLeft , boxTop ,boxRight, boxBottom), Empty);
									canvas.DrawRect(new SKRect(boxLeft , boxTop ,boxRight, boxBottom), RadioButtonPaint);

							float starCheckx = boxLeft + boxWidth * 0.2f;
							float starChecky = boxTop + boxHeight * 0.6f;
							float midX = boxLeft + boxWidth * 0.4f;
							float midY = boxTop + boxHeight * 0.8f;
							float endX = boxLeft + boxWidth * 0.8f;
							float endY = boxTop + boxHeight * 0.2f;

                          
                            using (var path = new SKPath())
                            {
                                path.MoveTo(starCheckx, starChecky); // Start point
                                path.LineTo(midX, midY);    // Midpoint
                                path.LineTo(endX, endY);    // End point

                                canvas.DrawPath(path, CheckMarkPaint);
                            }
                        }
								else if (cell.Value is bool bcf && bcf == false)
								{
                            canvas.DrawRect(new SKRect(boxLeft, boxTop, boxRight, boxBottom), Empty);
                            canvas.DrawRect(new SKRect(boxLeft, boxTop, boxRight, boxBottom), RadioButtonPaint);
                        }
								else
								{ // Handle other data types or null values}
									string cellText = cell.Value?.ToString() ?? "";
								 canvas.DrawText(cellText, x1 , rowy + diameter  / 2 + ListItemFontSize / 2, textPaint);
                        }
								break;
						}
				System.Diagnostics.Debug.WriteLine($"Row {rowIndex}, Col {colIndex}, Value: {cell.Value}, X: {x1}, Y: {rowy},Mode:{DisplayMode}");
            } 
                }
		    }
        
	
   
    


    public class TSKListColumn
    {
        public string Header { get; set; }
        public string Binding { get; set; } // Property name or index as string
        public Type DataType { get; set; }

    }
public class TSKListCell
{
    public object Value { get; set; }
    public Type DataType { get; set; }
    public string Binding { get; set; }

}
//private static void CardHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	//{
		//if (bindable is TSKList tskList && newValue is float newCardHeight)
		//{
			//tskList.CardHeight = newCardHeight;
			//tskList.InvalidateSurface(); // Redraw the control when card height changes
		//}
   // }
    private static void DisplaymodeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is TSKList tskList && newValue is DataDisplayMode newDisplayMode)
		{
			tskList.DisplayMode = newDisplayMode;
			tskList.InvalidateSurface(); // Redraw the control when display mode changes
		}
	
    }
    private static void ColumnNamesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{

		if (bindable is TSKList tskList && newValue is List<string> newColumnNames)
		{
			tskList.ListColumnNames = newColumnNames;
          tskList.InvalidateSurface(); // Redraw the control when column names change

        }
	
    }
    public void InvalidateSurface()
    {
        // If you have a SKCanvasView, call InvalidateSurface on it
        canvasView?.InvalidateSurface();

        System.Diagnostics.Debug.WriteLine("InvalidateSurface called");
    }
    protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
    { 
		System.Diagnostics.Debug.WriteLine("OnMeasure called");
        // Calculate desired size based on your content, or set a default
        double width = 400;  // or calculate based on columns
        double height = 300; // or calculate based on rows
        System.Diagnostics.Debug.WriteLine($"OnMeasure returning SizeRequest: {width}x{height}");
        return new SizeRequest(new Size(width, height));

		
    }
}