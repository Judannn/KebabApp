using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopKebab.Models;
using TopKebab.ViewModels;
using TopKebab.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TopKebab.Views
{
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel _viewModel;

        public SettingsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new SettingsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}