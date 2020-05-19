using Acr.UserDialogs;
using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Pages.ReservationPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateReservationPage : ContentPage
    {
        private List<RestauracjaModel> _restaurants;
        private List<StolikModel> stoliks;
        public int[] TimeInterval = new int[] { 15, 30, 45, 60 };
        DateTime Start;
        public RezerwacjaModel rezerwacja { get; set; }

        public CreateReservationPage()
        {
            InitializeComponent();
            CheckUserLogged();
        }
        public async void CheckUserLogged()
        {
            if (!Application.Current.Properties.ContainsKey("token"))
            {
                await DisplayAlert("Zaloguj sie", "Aby dokonac rezerwacji musisz sie zalogowac", "Ok");
                await Navigation.PopModalAsync(true);
            }
            else
            {
                SetUpPickerForRestaurant();
                IntervalPicker.ItemsSource = TimeInterval;
            }
        }
        public async void SetUpPickerForRestaurant()
        {
            using (var HttpConnector = new HttpApiConnector().GetClient())
            {
                var result = await HttpConnector.GetAsync(Constants.ConnectionApiUriRestauracja);
                if (result.IsSuccessStatusCode)
                {
                    var Content = await result.Content.ReadAsStringAsync();
                    _restaurants = JsonConvert.DeserializeObject<List<RestauracjaModel>>(Content);
                    Restauracja.ItemsSource = _restaurants.Select(x => x.KodRestauracji).ToList();
                }
            }
        }
        private async void SetStartOfReservation(object sender, EventArgs e)
        {
            var dateResult = await UserDialogs.Instance.DatePromptAsync(new DatePromptConfig());
            if (dateResult.Ok)
            {
                var timeResult = await UserDialogs.Instance.TimePromptAsync(new TimePromptConfig
                {
                    MinuteInterval = 30,
                    Use24HourClock = true
                });
                var minutes = 0;
                if (timeResult.Ok)
                {

                    if (timeResult.SelectedTime.Minutes / 15 > 1 || timeResult.SelectedTime.Minutes / 15 < 3)
                        minutes = 30;
                    else
                        minutes = 0;
                    if (timeResult.SelectedTime.Minutes % 15 != 0)
                        await DisplayAlert("Uwaga", $"Zmieniono czas rezerwacji na {timeResult.SelectedTime.Hours}:{minutes}", "Ok");
                    Start = new DateTime(dateResult.SelectedDate.Year, dateResult.SelectedDate.Month, dateResult.SelectedDate.Day, timeResult.SelectedTime.Hours, minutes, 0,DateTimeKind.Local);
                    if (Start <= DateTime.Now.ToLocalTime())
                    {
                        Start = new DateTime();
                        await DisplayAlert("Error", "Nie mozna wybrac daty z przeszlosci", "Ok");
                    } else
                    {
                        StartReservation.Text = Start.ToString("yyyy-MM-dd-HH-mm");
                    }
                }
            }
        }
        private void Restauracja_SelectedIndexChanged(object sender, EventArgs e)
        {
            Stolik.SelectedItem = null;
            Stolik.ItemsSource = _restaurants.First(x => x.KodRestauracji == Restauracja.SelectedItem.ToString()).Stolik.Select(x => x.NumerStolika).ToList();
            stoliks = _restaurants.First(x => x.KodRestauracji == Restauracja.SelectedItem.ToString()).Stolik;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var StartRezerwacji = Start;
            var KoniecRezerwacji = Start.AddMinutes(Double.Parse(IntervalPicker.SelectedItem.ToString()));
            var SelectedTable = stoliks.First(x => x.NumerStolika == Int32.Parse(Stolik.SelectedItem.ToString()));
            var UserMail = Main.User.Mail;


            var Rezerwacja = new RezerwacjaModel()
            {
                StartRezerwacji = StartRezerwacji,
                KoniecRezerwacji = KoniecRezerwacji,
                Stolik = SelectedTable,
                User = Main.User.Mail,
                Status = "Planned"
            };
            if (await CheckIfReservationCanBeDone(Rezerwacja))
            {
                if (await SendReservation(Rezerwacja))
                {
                    await DisplayAlert("Sukces", "Rezerwacja została dodana", "Ok");
                    rezerwacja = Rezerwacja;
                    await Navigation.PopModalAsync(true);
                }
            }
            else
            {
                await DisplayAlert("Error", "Wprowadzone termin jest zajety dla wybranego stolika", "Ok");
            }
        }
        private async Task<bool> CheckIfReservationCanBeDone(RezerwacjaModel Rezerwacja)
        {
            using (var HttpConnector = new HttpApiConnector().GetClient())
            {
                var content = JsonConvert.SerializeObject(Rezerwacja);
                var Buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(Buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await HttpConnector.PostAsync(Constants.ConnectionApiUriRezerwacja + "Check/", byteContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        private async Task<Boolean> SendReservation(RezerwacjaModel Rezerwacja)
        {
            using (var HttpConnector = new HttpApiConnector().GetClient())
            {
                var content = JsonConvert.SerializeObject(Rezerwacja);
                var Buffer = Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(Buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpConnector.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"].ToString());

                var response = await HttpConnector.PostAsync(Constants.ConnectionApiUriRezerwacja, byteContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}