using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Interface;
using MobilePizzaApp.Models;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            UserManagePage.BindingContext = User;
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            Loader.IsRunning = true;
            UserManagePage.BindingContext = User;
            if (User.Avatar != null)
                Avatar.Source = ImageSource.FromStream(() => new MemoryStream(User.Avatar));
            Loader.IsRunning = false;

        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            Loader.IsRunning = true;
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
                User.Password = UserModel.EncryotPw(NewPassword);
                var PendingSend = Newtonsoft.Json.JsonConvert.SerializeObject(User);
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
                        await DisplayAlert("Sukces", $"Haslo konta {User.Mail} zostało zmienione", "Ok");
                    else
                        await DisplayAlert("Error", "Nie udalo sie zmienic hasla", "Ok");
                }
                else await DisplayAlert("Error", "Nie mozna nawiazac polaczenia. Haslo nie zostało zmienione", "Ok");
            }
            Loader.IsRunning = false;
        }
        private void Logout_Clicked(object sender, EventArgs e)
        {
            Loader.IsRunning = true;
            if (Application.Current.Properties.Remove("token"))
            {
                (Application.Current.MainPage as TabbedPage).CurrentPage = (Application.Current.MainPage as TabbedPage).Children[0];
                Loader.IsRunning = false;
                (Application.Current.MainPage as TabbedPage).Children.RemoveAt(4);
                (Application.Current.MainPage as TabbedPage).Children.Insert(4, new UserAccountPage()
                {
                    Title = "Moje konto",
                    IconImageSource = ImageSource.FromResource("ModernPizzaApp.Zasoby.OsobaIkona.png"),
                });
                Application.Current.SavePropertiesAsync();
            }
        }
        public async void ChangeImageAvatar(object sender, EventArgs e)
        {
            try
            {
                var SelectedOption = await DisplayActionSheet("Avatar", "Cancel", null, new string[] { "Wybierz nowe", "Aparat", "Usuń" });
                switch (SelectedOption)
                {
                    case "Wybierz nowe":

                        var stream = await DependencyService.Get<IPhotoPicker>().GetImageStreamAsync();
                        if (stream != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                this.User.Avatar = memoryStream.ToArray();
                                Avatar.Source = ImageSource.FromStream(() => new MemoryStream(User.Avatar));
                            }
                        }
                        break;
                    case "Aparat":
                        try
                        {
                            await CrossMedia.Current.Initialize();
                            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                            {
                                await DisplayAlert("Error", "Aparat nie jest dostepny.", "OK");
                                break;
                            }
                            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                            {
                                Directory = "Sample",
                                Name = "test.jpg"
                            });
                            if (file != null)
                            {
                                Avatar.Source = ImageSource.FromStream(() => file.GetStream());
                                using (var ms = new MemoryStream())
                                {
                                    file.GetStream().CopyTo(ms);
                                    this.User.Avatar = ms.ToArray();
                                }
                            }
                        } catch (Exception err)
                        {
                            Console.WriteLine(err.Message);
                        }
                        break;
                    case "Usuń":

                        break;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
        public async void SendChanges(object sender, EventArgs args)
        {
            using (var HttpConnector = new HttpApiConnector().GetClient())
            {
                var PendingSend = Newtonsoft.Json.JsonConvert.SerializeObject(User);
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
                        await DisplayAlert("Sukces", $"Wszystkie zmiany zostały zapisane", "Ok");
                    else
                        await DisplayAlert("Error", "Nie udalo sie wprowadzic zmian", "Ok");
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
                task1 = DisplayPromptAsync("Step 2", "Wprowadz nowe haslo", "Dalej", "Anuluj");
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
                    return false;
                }
                else
                    return true;
            }
        }
    }
}