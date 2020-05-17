using Acr.UserDialogs;
using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Interface;
using MobilePizzaApp.Models;
using MobilePizzaApp.Pages.ReservationPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RezerwacjaPage : ContentPage
    {
        private RezerwacjaModel _rezerwacja { get; set; }
        private List<RezerwacjaModel> PastReservations { get; set; }
        private List<RezerwacjaModel> FutureReservations { get; set; }

        public RezerwacjaPage()
        {
            InitializeComponent();
            PastReservations = new List<RezerwacjaModel>();
            FutureReservations = new List<RezerwacjaModel>();

        }
        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("token"))
                DownlaodUserReservations();
            else
            {
                await DisplayAlert("Ostrzezenie", "Musisz się zalogować aby przeglądać rezerwacje", "Ok");
                (Application.Current.MainPage as TabbedPage).CurrentPage = (Application.Current.MainPage as TabbedPage).Children[0];
            }
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

                    FutureReservations = TempList.Where(x => DateTime.Compare(x.KoniecRezerwacji, DateTime.Now) > 0).ToList();
                    PastReservations = TempList.Where(x => DateTime.Compare(x.KoniecRezerwacji, DateTime.Now) < 0).ToList();

                    if (FutureReservations.Any())
                    {
                        _rezerwacja = FutureReservations.OrderBy(x => x.StartRezerwacji).First();
                        FutureReservations.Remove(_rezerwacja);
                    }

                    RezerwacjaLayout.BindingContext = _rezerwacja;
                    ReservationItemList.ItemsSource = PastReservations;
                }
            }
        }
        public async void AddNewRezerwation(object sender, EventArgs e)
        {

            if (FutureReservations.Count < 5)
            {
                var ReservationPage = new CreateReservationPage();
                await Navigation.PushModalAsync(ReservationPage);
                ReservationPage.Disappearing += AddReservation;
            }
            else
                await DisplayAlert("Ostrzezenie", "Mozesz posiadac do 5 zaplanowanych rezerwacji", "Ok");
        }
        public async void ActivateReservation(object sender, EventArgs e)
        {
            var scanPage = new ZXingScannerPage();
            scanPage.OnScanResult += (result) =>
             {
                 scanPage.IsScanning = false;

                 Device.BeginInvokeOnMainThread(() =>
                 {
                     Navigation.PopModalAsync();
                     ActivateByTableCode(result.Text);
                 });
             };

            await Navigation.PushModalAsync(scanPage);
        }

        private Task ActivateByTableCode(string result)
        {
            throw new NotImplementedException();
        }

        private void AddReservation(object sender, EventArgs e)
        {
            var SenderPage = sender as CreateReservationPage;
            var tempRezerwacja = SenderPage.rezerwacja;
            if (tempRezerwacja.StartRezerwacji < _rezerwacja.StartRezerwacji)
            {
                FutureReservations.Add(_rezerwacja);
                _rezerwacja = tempRezerwacja;
            }
            else
            {
                FutureReservations.Add(tempRezerwacja);

            }
        }
    }
}