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
    public partial class ManageAccountPage : ContentPage
    {
        public UserModel User { get; set; }
        public ManageAccountPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var tempUser = User;
            UserManagePage.BindingContext = tempUser;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Step 1", "Wprowadz swoje haslo");
            var IsValid = await CheckPasswordIsValid(new UserModel()
            {
                Mail = User.Mail,
                Password = UserModel.EncryotPw(result)
            });
            if (!IsValid)
            {
                await DisplayAlert("Blad", "Wprowadzone haslo jest nie prawidlowe", "Ok");
                return;
            }
            var NewPassword = await GetNewPassword();
            using (var HttpConnector = new HttpApiConnector().GetClient())
            {
                (UserManagePage.BindingContext as UserModel).Password = UserModel.EncryotPw(NewPassword);
                var PendingSend = Newtonsoft.Json.JsonConvert.SerializeObject((UserManagePage.BindingContext as UserModel));
                var Buffer = Encoding.UTF8.GetBytes(PendingSend);
                var byteContent = new ByteArrayContent(Buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpConnector.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"].ToString());
                var response = await HttpConnector.PutAsync(Constants.ConnectionApiUriUser, byteContent);
                if (response.IsSuccessStatusCode)
                {
                    var Stream = await response.Content.ReadAsByteArrayAsync();
                    Boolean responseResult = bool.Parse(Encoding.UTF8.GetString(Stream));
                    if (responseResult)
                        await DisplayAlert("Sukces", $"{responseResult}", "Ok");
                    else
                        await DisplayAlert("Error", "Nie udalo sie zmienic hasla", "Ok");
                }
                else await DisplayAlert("Error", "Nie mozna nawiazac polaczenia. Haslo nie zostało zmienione", "Ok");
            }
        }

        private async Task<String> GetNewPassword()
        {

            String Password1, Password2;
            Task<String> task1, task2;
            do
            {
                task1 = DisplayPromptAsync("Step 2", "Wprowadz nowe haslo","Dalej","Anuluj");
                Password1 = await task1;
                task2 = DisplayPromptAsync("Step 3", "Ponownie wprowadz nowe haslo", "Dalej", "Anuluj");
                Password2 = await task2;
            }
            while (Password1 != Password2 || task1.IsCanceled || task2.IsCanceled);
            return Password1;
        }

        private async Task<Boolean> CheckPasswordIsValid(UserModel userModel)
        {
            var PendingSend = Newtonsoft.Json.JsonConvert.SerializeObject(userModel);
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
                    return true;
            }
        }

        public void ChangeImageAvatar(object sender, EventArgs e)
        {

        }

        private  void Logout_Clicked(object sender, EventArgs e)
        {
            if (Application.Current.Properties.Remove("token"))
            {
                (Application.Current.MainPage as TabbedPage).CurrentPage = (Application.Current.MainPage as TabbedPage).Children[0];
                (Application.Current.MainPage as TabbedPage).Children.RemoveAt(4);
                (Application.Current.MainPage as TabbedPage).Children.Insert(4, new UserAccountPage()
                {
                    Title = "Moje konto",
                    IconImageSource = ImageSource.FromResource("ModernPizzaApp.Zasoby.OsobaIkona.png"),
                });
                Application.Current.SavePropertiesAsync();
            }
        }
    }
}