using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

public partial class TSkBase : ContentView
{
    protected SKCanvasView canvasView;
    protected ContentPresenter contentPresenter;
    protected Grid controlGrid;

    private bool _isDataReady;
    private bool _isReady;

    #region Bindable Properties
    public static readonly BindableProperty IsDataUsedProperty = BindableProperty.Create(
        nameof(IsDataUsed),
        typeof(bool),
        typeof(TSkBase),
        defaultValue: false,
        defaultBindingMode: BindingMode.OneWay,
        propertyChanged: OnIsDataUsedChanged);

    public static readonly BindableProperty IsContentReadyProperty = BindableProperty.Create(
        nameof(IsContentReady),
        typeof(bool),
        typeof(TSkBase),
        defaultValue: false,
        defaultBindingMode: BindingMode.OneWay,
        propertyChanged: OnIsContentReadyChanged);
    public static  readonly BindableProperty CanvasBackgroundColorProperty = BindableProperty.Create(
        nameof(CanvasBackgroundColor),
        typeof(Color),
        typeof(TSkBase),
        // Default color can be set to transparent or any other color
        defaultValue: Colors.Transparent,
        defaultBindingMode: BindingMode.OneWay
        );
    public static readonly BindableProperty IsVisiblePropery = BindableProperty.Create(
        nameof(IsVisible),
        typeof(bool),
        typeof(TSkBase),
        defaultValue: true,
        defaultBindingMode: BindingMode.OneWay
        );
    #endregion

    #region Getters and Setters
    public Color CanvasBackgroundColor
    {
        get => (Color)GetValue(CanvasBackgroundColorProperty);
        set => SetValue(CanvasBackgroundColorProperty, value);
    }
    public bool IsContentReady
    {
        get => (bool)GetValue(IsContentReadyProperty);
        set => SetValue(IsContentReadyProperty, value);
    }
    public bool IsDataUsed
    {
        get => (bool)GetValue(IsDataUsedProperty);
        set => SetValue(IsDataUsedProperty, value);
    }
    #endregion

    public void SetDataReady(bool ready)
    {
        // debug with message inclding child class name
        System.Diagnostics.Debug.WriteLine($"{this.GetType().Name} SetDataReady called with {ready}");
        _isDataReady = ready;
        UpdateContentVisibility();
        canvasView?.InvalidateSurface(); // Ensure redraw when data is ready
    }

    protected void SetReady(bool ready)
    {
        _isReady = ready;
        UpdateContentVisibility();
    }
    //setup touch events for canvasView and contentPresenter
    protected virtual void InitializeCanvas(object sender, TouchEventArgs e)
    {
        controlGrid = new Grid { BackgroundColor = Colors.Transparent };

        canvasView = new SKCanvasView
        {
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill
            
        };

        canvasView.PaintSurface += OnPaintSurface;

        contentPresenter = new ContentPresenter
        {
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            IsVisible = false // Hidden until ready
        };

        controlGrid.Children.Add(canvasView);
        controlGrid.Children.Add(contentPresenter);

        Content = controlGrid;
    }

    protected virtual void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear();
        DrawControl(canvas, e.Info);
    }

    protected virtual void DrawControl(SKCanvas canvas, SKImageInfo info)
    {
        // Override in derived classes
    }

    private void UpdateContentVisibility()
    {
        bool shouldShow = IsDataUsed ? (_isReady && _isDataReady) : _isReady;
        if (contentPresenter != null)
            contentPresenter.IsVisible = shouldShow;
    }

    public TSkBase()
    {
        // Optionally call InitializeCanvas() here or in derived classes
    }

    private static void OnIsDataUsedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TSkBase control)
            control.SetDataReady((bool)newValue);
        // Update visibility based on new value
        
    }

    private static void OnIsContentReadyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TSkBase control)
            control.SetReady((bool)newValue);
    }
    protected static class DrawingHelpers
    {
        public static void DrawRoundedRect(SKCanvas canvas, SKRect rect, float radius, SKPaint paint)
        {
            canvas.DrawRoundRect(rect, radius, radius, paint);
        }

        public static void DrawShadow(SKCanvas canvas, SKRect rect, float radius, SKColor color, float blur)
        {
            // Common shadow drawing logic
        }

        public static SKPaint CreateBasePaint(SKColor color, TSkBase skBase, SKPaintStyle style = SKPaintStyle.Fill)
        {
            return new SKPaint
            {
                Color = skBase.CanvasBackgroundColor.AsSKColor(),
                Style = style,
                IsAntialias = true
            };
        }
    }
}