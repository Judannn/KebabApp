using System;
using System.Collections.Generic;
using TopKebab.ViewModels;
using TopKebab.Views;
using Xamarin.Forms;

namespace TopKebab
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(FavouritesPage), typeof(FavouritesPage));
            Routing.RegisterRoute(nameof(Views.AddReviewPage), typeof(Views.AddReviewPage));
            SetNavBarIsVisible(this, false);
        }

    }
}
