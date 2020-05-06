using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPAge : ContentPage
    {
        public CarouselPage ParentPage { get; set; }
        public Color ImieColor
        {
            get
            {
                var Name = NameEntry.Text;
                var regex = new Regex("^[a-zA-z]*$");
                if (String.IsNullOrEmpty(Name))
                    return Color.Transparent;
                if (regex.IsMatch(Name))
                    return Color.Green;
                return Color.Red;
            }
        }

        public RegisterPAge()
        {
            InitializeComponent();
            this.NameEntry.TextChanged += (object send, TextChangedEventArgs err) =>
            {
                NameFrame.BorderColor = ImieColor;
            };

        }

        private async void RegisterUser(object sender, EventArgs e)
        {
            var User = CreateUserFromParams(MailEntry.Text, PasswordEntry.Text, IsAgreedTermsOfUse.IsChecked, NameEntry.Text, SubNameEntry.Text);
            if (User == null)
                return;
            var PendingSend = JsonConvert.SerializeObject(User);

            try
            {
                using (var HttpClient = new HttpApiConnector().GetClient())
                {
                    var Buffer = Encoding.UTF8.GetBytes(PendingSend);
                    var byteContent = new ByteArrayContent(Buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await HttpClient.PostAsync(Constants.ConnectionApiUriUser, byteContent);
                    if (response.IsSuccessStatusCode)
                    {
                        this.ParentPage.CurrentPage = this.ParentPage.Children[0];
                        await DisplayAlert("Sukces", "Możesz sie zalogować za pomocą wprowadzonych danych", "OK");
                    }
                    else
                        await DisplayAlert("Wystąpił błąd...", "Uzytkownik nie moze zostac dodany. Sprobuj ponownie pozniej", "OK");
                }
            }
            catch (Exception err)
            {

            }
        }
        private UserModel CreateUserFromParams(String mailEntry, String passwordEntry, Boolean isAgreedTermsOfUse, String nameEntry = null, String subNameEntry = null)
        {
            if (String.IsNullOrEmpty(mailEntry) || String.IsNullOrEmpty(passwordEntry))
                return null;
            var Model = new UserModel
            {
                Imie = String.IsNullOrEmpty(nameEntry) ? String.Empty : nameEntry,
                Nazwisko = String.IsNullOrEmpty(subNameEntry) ? String.Empty : subNameEntry,
                Mail = mailEntry,
                Password = ConvertStringToHash(passwordEntry)
            };
            return Model;
        }
        private string ConvertStringToHash(string passwordEntry)
        {
            var EncyrptedPW = String.Empty;
            using (var MD5Encryptor = MD5.Create())
            {
                var tempBytes = MD5Encryptor.ComputeHash(Encoding.UTF8.GetBytes(passwordEntry + "someRandomText"));
                StringBuilder sb = new StringBuilder();
                tempBytes.ToList().ForEach(x => sb.Append(x.ToString("x2")));
                EncyrptedPW = sb.ToString();
            }
            return EncyrptedPW;
        }

    }
}
