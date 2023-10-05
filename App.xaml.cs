using System;
using TopKebab.Services;
using TopKebab.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TopKebab
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
