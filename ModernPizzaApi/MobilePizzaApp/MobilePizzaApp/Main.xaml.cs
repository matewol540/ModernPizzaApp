using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using MobilePizzaApp.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public static UserModel User { get; set; }

        public Main()
        {
            InitializeComponent();
        }
        public static async Task DownloadUser()
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
                        User = Newtonsoft.Json.JsonConvert.DeserializeObject<UserModel>(Context);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception err)
            {
            }
        }
        private async void PagesOrganizer_Appearing(object sender, EventArgs e)
        {
            PagesOrganizer.Children.RemoveAt(3);

            if (Application.Current.Properties.ContainsKey("token"))
                await DownloadUser();

            if (User != null)
            {
                var UserPage = new ManageAccountPage()
                {
                    Title = "Moje konto",
                    IconImageSource = ImageSource.FromResource("MobilePizzaApp.Zasoby.OsobaIkona.png"),
                    User = User
                };
                PagesOrganizer.Children.Insert(3, UserPage);
            }
            else
            {
                var RegLogPages = new UserAccountPage()
                {
                    Title = "Moje konto",
                    IconImageSource = ImageSource.FromResource("MobilePizzaApp.Zasoby.OsobaIkona.png"),
                };
                PagesOrganizer.Children.Insert(3, RegLogPages);
            }
        }
    }
}