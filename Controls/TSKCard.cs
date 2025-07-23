using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
namespace TripicsSkControls.Controls;

public partial class TSKCard : TSkBase
{
    #region Bindable Properties
    public static readonly BindableProperty CardBackGroundColorProperty = BindableProperty.Create(
        nameof(CardBGColor), typeof(Color), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: Colors.Transparent, propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardBorderColorProperty = BindableProperty.Create(
        nameof(CardBorderColor), typeof(Color), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: Colors.Transparent, propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardBorderWidthProperty = BindableProperty.Create(
        nameof(CardBorderWidth), typeof(float), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: 0f, propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardBorderRadiusProperty = BindableProperty.Create(
        nameof(CardBorderRadius), typeof(float), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: 0f, propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardShadowColorProperty = BindableProperty.Create(
        nameof(CardShadowColor), typeof(Color), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: Colors.Transparent, propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardShadowOffsetProperty = BindableProperty.Create(
        nameof(CardShadowOffset), typeof(Point), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: new Point(0, 0), propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardShadowRadiusProperty = BindableProperty.Create(
        nameof(CardShadowRadius), typeof(float), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: 0f, propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardShadowOpacityProperty = BindableProperty.Create(
        nameof(CardShadowOpacity), typeof(float), typeof(TSKCard), defaultValue: 0.5f, defaultBindingMode: BindingMode.OneWay, propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardWidthRequestProperty = BindableProperty.Create(
        nameof(CardWidthRequest), typeof(double), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: -1.0, propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardHeightRequestProperty = BindableProperty.Create(
        nameof(CardHeightRequest), typeof(double), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: -1.0, propertyChanged: OnAllChanges);
    public static readonly BindableProperty CardShadowBlurProperty = BindableProperty.Create(
        nameof(CardShadowBlur), typeof(float), typeof(TSKCard), defaultBindingMode: BindingMode.OneWay, defaultValue: 0f, propertyChanged: OnAllChanges);
    #endregion
    #region Getters and Setters
    public Color CardBGColor
    {
        get => (Color)GetValue(CardBackGroundColorProperty);
        set => SetValue(CardBackGroundColorProperty, value);
    }
    public Color CardBorderColor
    {
        get => (Color)GetValue(CardBorderColorProperty);
        set => SetValue(CardBorderColorProperty, value);
    }
    public float CardBorderWidth
    {
        get => (float)GetValue(CardBorderWidthProperty);
        set => SetValue(CardBorderWidthProperty, value);
    }
    public float CardBorderRadius
    {
        get => (float)GetValue(CardBorderRadiusProperty);
        set => SetValue(CardBorderRadiusProperty, value);
    }
    public Color CardShadowColor
    {
        get => (Color)GetValue(CardShadowColorProperty);
        set => SetValue(CardShadowColorProperty, value);
    }
    public Point CardShadowOffset
    {
        get => (Point)GetValue(CardShadowOffsetProperty);
        set => SetValue(CardShadowOffsetProperty, value);
    }
    public float CardShadowRadius
    {
        get => (float)GetValue(CardShadowRadiusProperty);
        set => SetValue(CardShadowRadiusProperty, value);
    }
    public float CardShadowOpacity
    {
        get => (float)GetValue(CardShadowOpacityProperty);
        set => SetValue(CardShadowOpacityProperty, value);
    }
    public double CardWidthRequest
    {
        get => (double)GetValue(CardWidthRequestProperty);
        set => SetValue(CardWidthRequestProperty, value);
    }
    public double CardHeightRequest
    {
        get => (double)GetValue(CardHeightRequestProperty);
        set => SetValue(CardHeightRequestProperty, value);
    }
    public float CardShadowBlur
    {
        get => (float)GetValue(CardShadowBlurProperty);
        set => SetValue(CardShadowBlurProperty, value);
    }
    #endregion
    #region Constructors
    public TSKCard()
    {
        InitializeCanvas(null,null);
        UpdateInternalPadding();

        this.SizeChanged += (s, e) =>
        {
            UpdateCardAndCanvasSize();
        };
    }
    #endregion
    #region Fields
    #endregion
    #region Properties
    private const double ExtraContentPadding = 5.0;
    #endregion
    #region Methods
    #endregion
    #region Private Methods
    private void UpdateInternalPadding()
    {
        // Padding = border + shadow blur + shadow radius + max offset + extra content padding
        double shadowPadding = CardShadowBlur + CardShadowRadius + Math.Max(Math.Abs(CardShadowOffset.X), Math.Abs(CardShadowOffset.Y));
        double totalPadding = Math.Max(CardBorderWidth, 0) + shadowPadding + ExtraContentPadding;
        controlGrid.Padding = new Thickness(totalPadding);
        System.Diagnostics.Debug.WriteLine($"[TSKCard] Padding updated to {controlGrid.Padding}");
        UpdateCardAndCanvasSize();
    }

    private void UpdateCardAndCanvasSize()
    {
        double shadowPadding = CardShadowBlur + CardShadowRadius + Math.Max(Math.Abs(CardShadowOffset.X), Math.Abs(CardShadowOffset.Y));
        double totalPadding = Math.Max(CardBorderWidth, 0) + shadowPadding;

        // The outer size is CardWidth/Height, the inner grid/canvas size is reduced by padding
        double width = CardWidthRequest > 0 ? CardWidthRequest : this.Width;
        double height = CardHeightRequest > 0 ? CardHeightRequest : this.Height;

        this.WidthRequest = width + totalPadding * 2;
        this.HeightRequest = height + totalPadding * 2;
        controlGrid.WidthRequest = width + totalPadding * 2;
        controlGrid.HeightRequest = height + totalPadding * 2;
        canvasView.WidthRequest = width + totalPadding * 2;
        canvasView.HeightRequest = height + totalPadding * 2;
        System.Diagnostics.Debug.WriteLine($"[TSKCard] WidthRequest updated to {this.WidthRequest}, HeightRequest updated to {this.HeightRequest}");
    }
    public SKRect ContentBoundingBox
    {
        get
        {
            double shadowPadding = CardShadowBlur + CardShadowRadius + Math.Max(Math.Abs(CardShadowOffset.X), Math.Abs(CardShadowOffset.Y));
            double totalPadding = Math.Max(CardBorderWidth, 0) + shadowPadding;
            // Calculate total padding for border/shadow/extra
            double borderPadding = Math.Max(CardBorderWidth, 0) + shadowPadding + ExtraContentPadding;
            float shadowBlur = CardShadowBlur;
            float shadowRadius = CardShadowRadius;
            float shadowOffsetX = (float)CardShadowOffset.X;
            float shadowOffsetY = (float)CardShadowOffset.Y;
            
            float leftPadding = (float)(borderPadding  + Math.Max(0, -shadowOffsetX));
            float topPadding = (float)(borderPadding  + Math.Max(0, -shadowOffsetY));
            float rightPadding = (float)(borderPadding + Math.Max(0, shadowOffsetX));
            float bottomPadding = (float)(borderPadding + Math.Max(0, shadowOffsetY));

            // Use the actual rendered size, not just the requested size
            float actualWidth = (float)(this.WidthRequest > 0 ? this.WidthRequest : this.Width);
            float actualHeight = (float)(this.HeightRequest > 0 ? this.HeightRequest : this.Height);

            float left = leftPadding;
            float top = topPadding;
            float right = actualWidth - rightPadding;
            float bottom = actualHeight - bottomPadding;

            return new SKRect(left, top, right, bottom);
        }
    }
    #endregion
    #region Protected Methods
    protected virtual void OnAllChanges(object oldValue, object newValue)
    {
        UpdateInternalPadding();
        InvalidateSurface();
        InvalidateMeasure();
    }
    protected override void OnPaintSurface(Object sender, SKPaintSurfaceEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"[TSKCard] ContentBoundingBox:Left {ContentBoundingBox.Left}, Top {ContentBoundingBox.Top}, Right {ContentBoundingBox.Right}, Bottom {ContentBoundingBox.Bottom}");
        var canvas = e.Surface.Canvas;
        canvas.Clear();

        // Calculate total padding for shadow and border
        float shadowPadding = CardShadowBlur + CardShadowRadius + (float)Math.Max(Math.Abs(CardShadowOffset.X), Math.Abs(CardShadowOffset.Y));
        float totalPadding = Math.Max(CardBorderWidth, 0) + shadowPadding;

        // Draw shadow first, using the full canvas size
        var shadowRect = new SKRect(
            totalPadding,
            totalPadding,
            e.Info.Width - totalPadding,
            e.Info.Height - totalPadding
        );
        shadowRect.Offset((float)CardShadowOffset.X, (float)CardShadowOffset.Y);

        byte shadowAlpha = (byte)(Math.Clamp(CardShadowOpacity, 0.0, 1.0) * 255);
        using var shadowPaint = new SKPaint
        {
            IsAntialias = true,
            Color = CardShadowColor.AsSKColor().WithAlpha(shadowAlpha),
            ImageFilter = SKImageFilter.CreateDropShadow(
                dx: (float)CardShadowOffset.X,
                dy: (float)CardShadowOffset.Y,
                sigmaX: CardShadowBlur,
                sigmaY: CardShadowBlur,
                color: CardShadowColor.AsSKColor().WithAlpha(shadowAlpha)
            ),
            Style = SKPaintStyle.Fill
        };
        canvas.DrawRoundRect(shadowRect, CardBorderRadius, CardBorderRadius, shadowPaint);

        // Draw card background and border inside the padding
        var cardRect = new SKRect(
            totalPadding,
            totalPadding,
            e.Info.Width - totalPadding,
            e.Info.Height - totalPadding
        );

        using var backgroundPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = CardBGColor.AsSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(cardRect, CardBorderRadius, CardBorderRadius, backgroundPaint);

        using var borderPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            StrokeWidth = CardBorderWidth,
            Color = CardBorderColor.AsSKColor(),
            IsAntialias = true
        };
        canvas.DrawRoundRect(cardRect, CardBorderRadius, CardBorderRadius, borderPaint);
    }
    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        if (propertyName == nameof(Content))
        {
            // Set the content of the presenter to the base ContentView's Content
            if (base.Content != controlGrid)
            {
                contentPresenter.Content = base.Content;
                base.Content = controlGrid;
                System.Diagnostics.Debug.WriteLine($"[TitanCard] Content set to {contentPresenter.Content?.GetType().Name}");
            }
            
            UpdateInternalPadding();
            InvalidateSurface();
            InvalidateMeasure();
        }
    }
    #endregion
    #region Public Methods
    public void SetCardContent(View view)
    {
        contentPresenter.Content = view;
    }
    public static void OnAllChanges(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TSKCard card)
        {
            card.InvalidateMeasure();
            card.UpdateInternalPadding();
            card.InvalidateSurface();
        }
    }
    public void InvalidateSurface()
    {
        // If you have a SKCanvasView, call InvalidateSurface on it
        canvasView?.InvalidateSurface();

        System.Diagnostics.Debug.WriteLine("InvalidateSurface called");
    }
    #endregion
}