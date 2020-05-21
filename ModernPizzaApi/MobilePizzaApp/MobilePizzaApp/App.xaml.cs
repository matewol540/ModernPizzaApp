using MobilePizzaApp.Pages;
using System;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Main());
        }

        protected override void OnStart()
        {
            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
