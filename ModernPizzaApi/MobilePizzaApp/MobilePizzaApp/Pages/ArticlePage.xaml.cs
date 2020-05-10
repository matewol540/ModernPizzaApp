using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using MobilePizzaApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    public partial class ArticlePage : ContentPage
    {
        ArticleModel artyukul;
        ObservableCollection<KomentarzModel> _comments = new ObservableCollection<KomentarzModel>();

        public ArticlePage()
        {
            InitializeComponent();
        }
        public ArticlePage(ArticleModel article)
        {
            InitializeComponent();
            this.ArticleMainImage.Source = ImageSource.FromStream(() => new MemoryStream(article.obraz));
            artyukul = article;
            this.MainStack.BindingContext = article;
            DownloadCommentsForArticleAsync(article.ObjectId);
            CommentsListView.ItemsSource = _comments;
        }

        private async Task DownloadCommentsForArticleAsync(string objectId)
        {
            LoaderIndicator.IsRunning = true;
            LoaderIndicator.IsVisible = true;
            try
            {
                using (var HttpConnector = new HttpApiConnector().GetClient())
                {
                    var Task = await HttpConnector.GetAsync(Constants.ConnectionApiUriArtykul + "Komentarz/" + objectId);
                    if (Task.IsSuccessStatusCode)
                    {
                        JsonConvert.DeserializeObject<ObservableCollection<KomentarzModel>>(await Task.Content.ReadAsStringAsync()).OrderByDescending(x => x.Data).ToList().ForEach(x => _comments.Add(x));
                    }
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Błąd pobierania komentarzy", "Ok");
            }
            finally
            {
                LoaderIndicator.IsRunning = false;
                LoaderIndicator.IsVisible = false;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {

            LoaderIndicator.IsRunning = true;
            LoaderIndicator.IsVisible = true;
            try
            {
                var KomentObject = new KomentarzModel()
                {
                    ArtukulID = artyukul.ObjectId,
                    Tresc = Komentarz.Text,
                    Autor = "Me",
                    Data = DateTime.Now,
                };
                if (!Application.Current.Properties.ContainsKey("token"))
                {
                    await DisplayAlert("Ooops", "Musisz być zalogowany aby dodać komentarz", "OK");
                    return;
                }
                if (String.IsNullOrEmpty(KomentObject.Tresc))
                {
                    await DisplayAlert("Ooops", "Nie mozesz dodac pustego komentarza", "OK");
                    return;
                }

                var PendingContext = JsonConvert.SerializeObject(KomentObject);
                using (var HttpConnector = new HttpApiConnector().GetClient())
                {
                    var Buffer = Encoding.UTF8.GetBytes(PendingContext);
                    var byteContent = new ByteArrayContent(Buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpConnector.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Application.Current.Properties["token"].ToString());
                    var response = await HttpConnector.PostAsync(Constants.ConnectionApiUriArtykul + "komentarz/", byteContent);

                    if (response.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Sukcess", "Dodano komentarz do artykulu", "Ok");
                        _comments.Insert(0, KomentObject);
                        //this.CommentsGrid.RowDefinitions.Add(new RowDefinition() { Height = 60 });
                        ////this.CommentsGrid.Children.Add(new Frame() { Content = new Label() { Text = KomentObject.Tresc } }, 0, this.CommentsGrid.Children.Count);
                        //this.CommentsGrid.Children.Add(new CommentView() { BindingContext = KomentObject }, 0, this.CommentsGrid.Children.Count);
                        this.Komentarz.Text = String.Empty;
                    }
                    else
                        await DisplayAlert("Error", "Blad dodania komentarza", "Ok");


                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Błąd podaczas dodawanie komentarza", "Ok");
            }
            finally
            {
                LoaderIndicator.IsRunning = false;
                LoaderIndicator.IsVisible = false;
            }
        }

        private void CommentsListView_Refreshing(object sender, EventArgs e)
        {

        }
    }
}