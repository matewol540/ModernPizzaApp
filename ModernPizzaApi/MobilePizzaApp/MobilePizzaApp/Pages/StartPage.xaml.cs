using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using MobilePizzaApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        Task<int> IsLoadingTask = null;
        int LastLoadedItemsCount;
        public StartPage()
        {
            InitializeComponent();
            LoadArticlesFromApi(0);
        }
        public async Task<int> LoadArticlesFromApi(int StartIndex)
        {
            var InfTemp = new List<ArticleModel>();
            try
            {
                using (var HttpClient = new HttpApiConnector().GetClient())
                {
                    var response = await HttpClient.GetAsync(Constants.ConnectionApiUriArtykul + StartIndex);
                    if (response.IsSuccessStatusCode)
                    {
                        string Content = await response.Content.ReadAsStringAsync();
                        InfTemp = JsonConvert.DeserializeObject<List<ArticleModel>>(Content);
                    }
                }
                for (int i = 0; i < (InfTemp.Count / 2); i++)
                    NewsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(330, GridUnitType.Absolute) });
                LoadFirstAsMainNews(InfTemp);
                InfTemp.ForEach(x => NewsGrid.Children.Add(new InformationView(x), NewsGrid.Children.Count % 2, NewsGrid.Children.Count / 2));
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Connection refused!", "Ok");
                return 0;
            }
            ActivityIndicator.IsRunning = false;
            IsLoadingTask = null;
            LastLoadedItemsCount = InfTemp.Count;
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

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (IsLoadingTask == null &&
                NewsGrid.Children[NewsGrid.Children.Count - 3].Y + NewsGrid.Children[NewsGrid.Children.Count - 3].Height / 4 < e.ScrollY &&
                LastLoadedItemsCount != 0)
            {
                IsLoadingTask = LoadArticlesFromApi(NewsGrid.Children.Count);
                ActivityIndicator.IsRunning = !IsLoadingTask.IsCompleted;
                Thread.Sleep(2000);
            }
        }
    }
}