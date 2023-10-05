using System;
using System.Collections.Generic;
using System.ComponentModel;
using TopKebab.Models;
using TopKebab.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TopKebab.Views
{
    public partial class AddReviewPage : ContentPage
    {
        public Item Item { get; set; }

        public AddReviewPage()
        {
            InitializeComponent();
            BindingContext = new AddReviewPage();
        }
    }
}