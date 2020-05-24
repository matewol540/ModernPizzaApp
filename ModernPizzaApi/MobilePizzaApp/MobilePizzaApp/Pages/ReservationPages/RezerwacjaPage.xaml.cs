using Acr.UserDialogs;
using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Interface;
using MobilePizzaApp.Models;
using MobilePizzaApp.Pages.ReservationPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<RezerwacjaModel> FutureReservations { get; set; }

        public RezerwacjaPage()
        {
            InitializeComponent();
            PastReservations = new List<RezerwacjaModel>();
            FutureReservations = new ObservableCollection<RezerwacjaModel>();

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
                ((Application.Current.MainPage as NavigationPage).RootPage as TabbedPage).CurrentPage = ((Application.Current.MainPage as NavigationPage).RootPage as TabbedPage).Children[0];
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

                    FutureReservations = new ObservableCollection<RezerwacjaModel>(TempList.Where(x => (DateTime.Compare(x.KoniecRezerwacji, DateTime.Now.AddMinutes(5.0F)) > 0 && x.Status == "Planned") || x.Status == "Active").ToList());
                    PastReservations = TempList.Where(x => DateTime.Compare(x.KoniecRezerwacji, DateTime.Now) < 0).ToList();

                    if (FutureReservations.Any())
                    {
                        _rezerwacja = FutureReservations.OrderBy(x => x.StartRezerwacji).First();
                        FutureReservations.Remove(_rezerwacja);
                        RezerwacjaLayout.BindingContext = _rezerwacja;
                        ReservationItemList.ItemsSource = FutureReservations;
                        if (_rezerwacja.Status == "Planned")
                        {
                            var da = DateTime.Now;
                            var daw = _rezerwacja.StartRezerwacji.AddMinutes(-5.0F);
                            ActivationButton.IsEnabled = da >= daw;
                        }
                        else if (_rezerwacja.Status == "Active")
                        {
                            ActivationButton.Text = "Aktywna";
                            ActivationButton.IsEnabled = false;

                        }
                    }
                    else
                        ActivationButton.IsEnabled = false;

                }
            }
        }
        public async void AddNewRezerwation(object sender, EventArgs e)
        {

            if (FutureReservations.Count < 5)
            {
                var ReservationPage = new CreateReservationPage();
                ReservationPage.Disappearing += AddReservation;
                await Navigation.PushModalAsync(ReservationPage);
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
                 Device.BeginInvokeOnMainThread(async () =>
                 {
                     await Navigation.PopAsync();
                     try
                     {
                         await DisplayAlert("Read rsponse ", $"{result.Text}", "Ok");
                         ActivateByTableCode(result.Text);
                     }
                     catch (Exception err)
                     {
                         await DisplayAlert("Error", $"{err.StackTrace}", "Ok");
                     }
                 });
             };
            scanPage.Disappearing += (send, aqwe) =>
             {
                 scanPage.IsScanning = false;
                 Device.BeginInvokeOnMainThread(async () =>
                 {
                     await Navigation.PopAsync();
                     try
                     {
                         await DisplayAlert("Read rsponse ", $"dg01.1", "Ok");
                         ActivateByTableCode("dg01.1");
                     }
                     catch (Exception err)
                     {
                         await DisplayAlert("Error", $"{err.StackTrace}", "Ok");
                     }
                 });
             };

            await Navigation.PushAsync(scanPage);
        }

        private async void ActivateByTableCode(string result)
        {
            await DisplayAlert("Read rsponse ", $"{result}", "Ok");
            if (result.Contains('.'))
            {
                var Restaurant = result.Split('.')[0];
                var Table = result.Split('.')[1];

                if (_rezerwacja.Stolik.KodRestauracji != Restaurant)
                {
                    await DisplayAlert("Błąd", "Rezerwacja dotyczy innej restauracji", "Ok");
                    return;
                }
                if (_rezerwacja.Stolik.NumerStolika != Int32.Parse(Table))
                {
                    await DisplayAlert("Błąd", "Rezerwacja dotyczy innego stolika", "Ok");
                    return;
                }

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
                        ActivationButton.Text = "Aktywna";
                        ActivationButton.BackgroundColor = Color.FromHex("#66ff66");
                        ActivationButton.TextColor = Color.FromHex("#074a07");
                    }
                }
            }
        }

        private void AddReservation(object sender, EventArgs e)
        {
            var SenderPage = sender as CreateReservationPage;
            var tempRezerwacja = SenderPage.rezerwacja;
            if (tempRezerwacja == null)
                return;
            if (_rezerwacja == null)
            {
                FutureReservations.Add(tempRezerwacja);
            }
            else if (tempRezerwacja.StartRezerwacji < _rezerwacja.StartRezerwacji)
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
        private async void RemoveReservation(object sender, EventArgs e)
        {

            var ReservationToRemove = (sender as MenuItem).CommandParameter as RezerwacjaModel;

            using (var HttpApiConnector = new HttpApiConnector().GetClient())
            {
                var Content = JsonConvert.SerializeObject(ReservationToRemove);
                var ByteContent = Encoding.UTF8.GetBytes(Content);
                var ByteArrayConent = new ByteArrayContent(ByteContent);
                ByteArrayConent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpApiConnector.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"].ToString());
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(Constants.ConnectionApiUriRezerwacja + ReservationToRemove.ObjectId),
                    Content = ByteArrayConent
                };
                var response = await HttpApiConnector.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    FutureReservations.Remove(ReservationToRemove);
                }
            }
        }

        private void RemoveSelectionOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }
    }
}