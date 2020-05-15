using MobilePizzaApp.Pages.ReservationPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RezerwacjaPage : ContentPage
    {
        public RezerwacjaPage()
        {
            InitializeComponent();
        }

        public async void AddNewRezerwation(object sender,EventArgs e)
        {
            var ReservationPage = new CreateReservationPage();
            await Navigation.PushModalAsync(ReservationPage);
        }
        public void AddNewRezerwation2(object sender,EventArgs e)
        {

        }
    }
}