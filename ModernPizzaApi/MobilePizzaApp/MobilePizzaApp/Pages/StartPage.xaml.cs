using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using MobilePizzaApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        Task<int> IsLoadingTask = null; // Task ladujacy odpowiada za activity indicator
        int LastLoadedItemsCount; // Index ostatnio pobranego
        List<ArticleModel> LoadedArticles; // Lsita pobranych artykulow
        List<InformationView> ListInfoView; // Lista widokow utworzonych dla artykulow

        public StartPage()
        {
            InitializeComponent();
            LoadedArticles = new List<ArticleModel>();
        }
        private async void LoadArticles(object sender, EventArgs e)
        {
            var tempLoadedArticles = await DownloadArticlesToList(0);
            LoadFirstAsMainNews(LoadedArticles);
            ListInfoView = CreateInformationViewFromList(LoadedArticles, tempLoadedArticles);
            LastLoadedItemsCount = ListInfoView.Count+1;
            ListInfoView.ForEach(x => NewsGrid.Children.Add(x, NewsGrid.Children.Count % 2, NewsGrid.Children.Count / 2));
        }

        private async Task<int> DownloadArticlesToList(int v) // Liczva ostatnio pobranych
        {
            var InfTemp = new List<ArticleModel>();
            try
            {
                using (var HttpClient = new HttpApiConnector().GetClient())
                {
                    var response = await HttpClient.GetAsync(Constants.ConnectionApiUriArtykul + v);
                    if (response.IsSuccessStatusCode)
                    {
                        string Content = await response.Content.ReadAsStringAsync();
                        InfTemp = JsonConvert.DeserializeObject<List<ArticleModel>>(Content);
                    }
                }
             }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Connection refused!", "Ok");
                return 0;
            }
            finally
            {
                for (int i = 0; i < (InfTemp.Count / 2); i++)
                    NewsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(330, GridUnitType.Absolute) });
                LoadedArticles.AddRange(InfTemp);
                ActivityIndicator.IsRunning = false;
            }
            return InfTemp.Count;
        }
        private void LoadFirstAsMainNews(List<ArticleModel> articleModel)
        {
            if (FirstNews.BindingContext == null)
            {
                FirstNews.BindingContext = articleModel.First();
                FirstImageArticle.Source = ImageSource.FromStream(() => new MemoryStream(articleModel.First().obraz));
                articleModel.Remove(articleModel.First());
            }
        }
        private List<InformationView> CreateInformationViewFromList(List<ArticleModel> articleModels, int ToBeLoadedCount)
        {
            var TempInformationList = new List<InformationView>();
            articleModels.Skip(articleModels.Count - ToBeLoadedCount).ToList().ForEach(x => TempInformationList.Add(new InformationView(x)));
            return TempInformationList;
        }







        private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (IsLoadingTask == null &&
                NewsGrid.Children[NewsGrid.Children.Count - 3].Y + NewsGrid.Children[NewsGrid.Children.Count - 3].Height / 4 < e.ScrollY + 300 &&
                LastLoadedItemsCount != 0)
            {
                IsLoadingTask = DownloadArticlesToList(LoadedArticles.Count+1);
                var TempCount = await IsLoadingTask;
                ListInfoView = CreateInformationViewFromList(LoadedArticles, TempCount);
                LastLoadedItemsCount = ListInfoView.Count;
                ListInfoView.ForEach(x => NewsGrid.Children.Add(x, NewsGrid.Children.Count % 2, NewsGrid.Children.Count / 2));
                ActivityIndicator.IsRunning = !IsLoadingTask.IsCompleted;
                IsLoadingTask = null;
            }
        }


    }
}