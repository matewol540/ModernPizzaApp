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
using Xamarin.Forms.Internals;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPAge : ContentPage
    {
        private Regex MailRegex = new Regex("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");
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
        public Color MailColor
        {
            get
            {
                var Mail = MailEntry.Text;
                if (String.IsNullOrEmpty(Mail) || MailRegex.IsMatch(Mail))
                    return Color.Red;
                if (MailRegex.IsMatch(Mail))
                    return Color.Green;
                return Color.Transparent;
            }
        }
        public Color PasswordColor
        {
            get
            {
                var PW1 = PasswordEntry.Text;
                var PW2 = RepeatPasswordEntry.Text;
                if (PW1 != PW2 || PW1 == String.Empty || PW2 == String.Empty)
                    return Color.Red;
                return Color.Green;
            }
        }
        public Color CheckBoxColor
        {
            get
            {
                if (IsAgreedTermsOfUse.IsChecked)
                    return Color.Green;
                return Color.Red;
            }
        }

        public List<BoxView> Boxes = new List<BoxView>()
        {
            new BoxView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor=Color.Red,
                IsVisible = false,
                HeightRequest=8
            },
            new BoxView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor=Color.Orange,
                IsVisible = false,
                HeightRequest=8
            },
            new BoxView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor=Color.Yellow,
                IsVisible = false,
                HeightRequest=8
            },
            new BoxView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor=Color.Green,
                IsVisible = false,
                HeightRequest=8
            }
        };

        public RegisterPAge()
        {
            InitializeComponent();
            Boxes.ForEach(x => PasswordIndicator.Children.Add(x));
            this.PasswordEntry.TextChanged += (object send, TextChangedEventArgs err) =>
            {
                var Pw = PasswordEntry.Text;
                int CountToTake = 0;
                if (Pw.Length > 8)
                    CountToTake += 2;
                if (Pw.Any(char.IsUpper) && Pw.Any(char.IsLower))
                    CountToTake += 1;
                if (Pw.Any(char.IsDigit))
                    CountToTake += 1;
                Boxes.Take(CountToTake).ForEach(x => x.IsVisible = true);
                Boxes.Skip(CountToTake).ToList().ForEach(x => x.IsVisible = false);
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
                        await DisplayAlert("Wystąpił błąd...", "Uzytkownik nie moze zostac dodany, ponieważ inne konto zostało zarejestrowane pod tym adresem", "OK");
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Wystąpił błąd...", err.StackTrace, "OK");
            }
        }
        private UserModel CreateUserFromParams(String mailEntry, String passwordEntry, Boolean isAgreedTermsOfUse, String nameEntry = null, String subNameEntry = null)
        {
            String ErrorMessage = GetErrorMessage(mailEntry, passwordEntry, isAgreedTermsOfUse, nameEntry, subNameEntry);
            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                DisplayAlert("Error", ErrorMessage, "OK");
                return null;
            }

            var Model = new UserModel
            {
                Imie = String.IsNullOrEmpty(nameEntry) ? String.Empty : nameEntry,
                Nazwisko = String.IsNullOrEmpty(subNameEntry) ? String.Empty : subNameEntry,
                Mail = mailEntry,
                Password = UserModel.EncryotPw(passwordEntry),
                Role = "User"
            };
            return Model;
        }
        private string GetErrorMessage(String mailEntry, String passwordEntry, Boolean isAgreedTermsOfUse, String nameEntry = null, String subNameEntry = null)
        {
            String ErrorMessage = String.Empty;
            if (String.IsNullOrEmpty(mailEntry) || !MailRegex.IsMatch(mailEntry))
            {
                MailFrame.BorderColor = MailColor;
                ErrorMessage += $"Provided e-mail contains errors. {Environment.NewLine}";
            }
            if (String.IsNullOrEmpty(passwordEntry))
            {
                PasswordFrame.BorderColor = PasswordColor;
                ErrorMessage += $"Password must be filled. {Environment.NewLine}";
            }
            if (passwordEntry != RepeatPasswordEntry.Text)
            {
                PasswordFrame.BorderColor = PasswordColor;
                ErrorMessage += $"Provided passwords are not equal. {Environment.NewLine}";
            }
            if (!IsAgreedTermsOfUse.IsChecked)
            {
                CheckboxFrame.BorderColor = CheckBoxColor;
                ErrorMessage += $"Nie zaakceptowano regulaminu";
            }
            return ErrorMessage;
        }

    }
}
