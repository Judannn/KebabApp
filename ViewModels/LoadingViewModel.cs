using System;
using System.Collections.Generic;
using System.Text;
using TopKebab.Views;
using Xamarin.Forms;

namespace TopKebab.ViewModels
{
    public class LoadingViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoadingViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(MapPage)}");
        }
    }
}
