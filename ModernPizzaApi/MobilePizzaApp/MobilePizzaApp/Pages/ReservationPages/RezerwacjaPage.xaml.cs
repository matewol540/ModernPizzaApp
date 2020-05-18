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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            {
                LoadingIndicator.IsRunning = true;
                DownlaodUserReservations();
                LoadingIndicator.IsRunning = false;
            }
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
                    ReservationItemList.ItemsSource = FutureReservations;
                    ActivationButton.IsEnabled = _rezerwacja.StartRezerwacji.AddMinutes(5.0) >= DateTime.Now;
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

        private async void ActivateByTableCode(string result)
        {
            _rezerwacja.Status = "Active";
            using (var HttpApiConnector = new HttpApiConnector().GetClient())
            {
                var Content = JsonConvert.SerializeObject(_rezerwacja);
                var bytes = Encoding.UTF8.GetBytes(Content);
                var byteContent = new ByteArrayContent(bytes);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpApiConnector.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"].ToString());
                var response = await HttpApiConnector.PutAsync(Constants.ConnectionApiUriRezerwacja, byteContent);
                if (response.IsSuccessStatusCode)
                {
                    ActivationButton.IsEnabled = false;
                    ActivationButton.BackgroundColor = Color.FromHex("#66ff66");
                    ActivationButton.TextColor= Color.FromHex("#074a07");
                }
            }
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
        private async void ShowHistory(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ReservationHisotryPage(PastReservations));
        }
        private async void ShowOnMap(object sender, EventArgs e)
        {

            var restaurant = await DownlaodRestaurnatOnReservationCode(_rezerwacja);
            if (restaurant == null)
                return;
            double latitud = restaurant.XGeoLocalization;
            double longitud = restaurant.YGeoLocalization;
            string placeName = "Pizzeria";

            var supportsUri = await Launcher.CanOpenAsync("comgooglemaps://");

            if (supportsUri)
                await Launcher.OpenAsync($"comgooglemaps://?q={latitud},{longitud}({placeName})");

            else
                await Map.OpenAsync(latitud, longitud, new MapLaunchOptions { Name = "Test" });
        }
        private async Task<RestauracjaModel> DownlaodRestaurnatOnReservationCode(RezerwacjaModel rezerwacja)
        {
            RestauracjaModel tempRestaurantModel = null;
            try
            {

                using (var HttpConnector = new HttpApiConnector().GetClient())
                {
                    var result = await HttpConnector.GetAsync(Constants.ConnectionApiUriRestauracja);
                    if (result.IsSuccessStatusCode)
                    {
                        var Content = await result.Content.ReadAsStringAsync();
                        tempRestaurantModel = JsonConvert.DeserializeObject<List<RestauracjaModel>>(Content).ToList().First(x => x.KodRestauracji == rezerwacja.Stolik.KodRestauracji);
                    }
                }
            }
            catch (Exception er)
            {
                await DisplayAlert("Błąd", "Nie mozna pobrac miejsca docelowego", "Ok");
            }
            return tempRestaurantModel;
        }
    }
}