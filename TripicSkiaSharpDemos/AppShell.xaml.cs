﻿namespace TripicSkiaSharpDemos
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ListView), typeof(ListView));
        }
    }
}
