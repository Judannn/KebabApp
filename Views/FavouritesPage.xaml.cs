using System.ComponentModel;
using TopKebab.ViewModels;
using Xamarin.Forms;

namespace TopKebab.Views
{
    public partial class FavouritesPage : ContentPage
    {
        public FavouritesPage()
        {
            InitializeComponent();
            BindingContext = new FavouritesViewModel();
        }
    }
}