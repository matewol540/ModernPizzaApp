using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginApplicationPage : ContentPage
    {
        UserModel User;
        public CarouselPage ParentPage { get; set; }
        public LoginApplicationPage()
        {
            InitializeComponent();
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {
            LoginAcivityIndicator.IsRunning = true;
            if (User == null && await LoginToApplication())
            {
                (Application.Current.MainPage as Main).CurrentPage = (Application.Current.MainPage as TabbedPage).Children[0];
                (Application.Current.MainPage as Main).Children.Remove(this.ParentPage);
                (Application.Current.MainPage as Main).CreatePageOnUserLogged(User);
                await DisplayAlert("Sukces", "Udało się poprawnie zalogować do aplikajci", "Ok");
            }
        }
        private async Task<Boolean> LoginToApplication()
        {
            var login = loginEntry.Text;
            var pw = passwordEntry.Text;
            User = new UserModel()
            {
                Mail = login,
                Password = UserModel.EncryotPw(pw)
            };
            var PendingSend = Newtonsoft.Json.JsonConvert.SerializeObject(User);
            using (var HtttpClientConnector = new HttpApiConnector().GetClient())
            {
                var Buffer = Encoding.UTF8.GetBytes(PendingSend);
                var byteContent = new ByteArrayContent(Buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await HtttpClientConnector.PostAsync(Constants.ConnectionApiUriUser + "login/", byteContent);

                if (!response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Błąd", "Wprowadzono nie prawidłowe dane logowania", "OK");
                    return false;
                }
                else
                {
                    var token = await response.Content.ReadAsStringAsync();
                    if (Application.Current.Properties.ContainsKey("token"))
                        Application.Current.Properties["token"] = token;
                    else
                        Application.Current.Properties.Add("token", token);
                    if (Zapamietaj.IsChecked)
                        await Application.Current.SavePropertiesAsync();
                    await Main.DownloadUser();
                    User = Main.User;
                    return true;
                }
            }
        }
    }
}