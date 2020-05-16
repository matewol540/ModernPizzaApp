using Acr.UserDialogs;
using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using MobilePizzaApp.Pages.ReservationPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
        private RezerwacjaModel _rezerwacja { get; set; }
        private List<RezerwacjaModel> PastReservations { get; set; }
        public RezerwacjaPage()
        {
            InitializeComponent();

            
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            DownlaodUserReservations();
        }
        private async void DownlaodUserReservations()
        {
            var TempList = new List<RezerwacjaModel>();

            using (var HttpApiConnector = new HttpApiConnector().GetClient())
            {
                var UserMail = Main.User.Mail;
                var response = await HttpApiConnector.GetAsync(Constants.ConnectionApiUriRezerwacja + UserMail);
                if (response.IsSuccessStatusCode)
                {
                    TempList = JsonConvert.DeserializeObject<List<RezerwacjaModel>>(await response.Content.ReadAsStringAsync());

                    var FutureOrActualReservations = TempList.Where(x => x.KoniecRezerwacji >= DateTime.Now).ToList();
                    if (FutureOrActualReservations.Any())
                        _rezerwacja = FutureOrActualReservations.First();
                    TempList.Remove(_rezerwacja);
                    PastReservations = TempList;
                    RezerwacjaLayout.BindingContext = _rezerwacja;
                    ReservationItemList.BindingContext = PastReservations;
                }
            }
        }
        public async void AddNewRezerwation(object sender, EventArgs e)
        {

            if (_rezerwacja != null)
            {
                var ReservationPage = new CreateReservationPage();
                await Navigation.PushModalAsync(ReservationPage);
                ReservationPage.Disappearing += AddReservation;
            }
            else
                await DisplayAlert("Ostrzezenie", "Mozesz posiadac tylko jedna rezerwacje", "Ok");
        }
        public void AddNewRezerwation2(object sender, EventArgs e)
        {

        }
        private void AddReservation(object sender, EventArgs e)
        {
            var SenderPage = sender as CreateReservationPage;
            _rezerwacja = SenderPage.rezerwacja;
        }
    }
}