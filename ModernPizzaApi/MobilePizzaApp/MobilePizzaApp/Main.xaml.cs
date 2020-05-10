using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using MobilePizzaApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : TabbedPage
    {
        public Main()
        {
            InitializeComponent();

        }
        private async Task<UserModel> DownloadUser()
        {
            try
            {
                using (var HttpConnector = new HttpApiConnector().GetClient())
                {
                    HttpConnector.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"].ToString());
                    var response = await HttpConnector.GetAsync(Constants.ConnectionApiUriUser + "auth/");
                    if (response.IsSuccessStatusCode)
                    {
                        var Context = await response.Content.ReadAsStringAsync();
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<UserModel>(Context);
                    }
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Alert", "Musisz odswierzyc dane logowania. Zaloguj się ponownie", "Ok");
            }
            return null;

        }
        private async void PagesOrganizer_Appearing(object sender, EventArgs e)
        {
            var User = new UserModel();
            if (Application.Current.Properties.ContainsKey("token"))
                User = await DownloadUser();

            if (User != null)
            {
                var UserPage = new ManageAccountPage()
                {
                    Title = "Moje konto",
                    IconImageSource = ImageSource.FromResource("MobilePizzaApp.Zasoby.OsobaIkona.png"),
                    User = User
                };
                PagesOrganizer.Children.Insert(4, UserPage);
            }
            else
            {
                var RegLogPages = new UserAccountPage()
                {
                    Title = "Moje konto",
                    IconImageSource = ImageSource.FromResource("MobilePizzaApp.Zasoby.OsobaIkona.png"),
                };
                PagesOrganizer.Children.Insert(4, RegLogPages);
            }
        }
    }
}