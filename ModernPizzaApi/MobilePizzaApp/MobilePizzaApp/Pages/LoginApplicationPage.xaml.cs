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
        public LoginApplicationPage()
        {
            InitializeComponent();
        }

        private async void Button_ClickedAsync(object sender, EventArgs e)
        {

            if (await LoginToApplication())
            {
                (Application.Current.MainPage as TabbedPage).CurrentPage = (Application.Current.MainPage as TabbedPage).Children[0];
                (Application.Current.MainPage as TabbedPage).Children.RemoveAt(4);
                (Application.Current.MainPage as TabbedPage).Children.Insert(4, new ManageAccountPage()
                {
                    Title = "Moje konto",
                    IconImageSource = ImageSource.FromResource("ModernPizzaApp.Zasoby.OsobaIkona.png"),
                    User = User
                });

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
                    return true;
                }
            }
        }
    }
}