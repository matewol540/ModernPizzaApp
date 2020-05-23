using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Interface;
using MobilePizzaApp.Models;
using MobilePizzaApp.Views;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : CarouselPage
    {
        private ObservableCollection<NapojModel> _Drinks;
        private ObservableCollection<PizzaModel> _Pizza;


        public MenuPage()
        {
            InitializeComponent();

        }
        private async void LoadItemsToCollections(object sender, EventArgs e)
        {
            this.ActivityIndicatorPizza.IsRunning = true;
            this.ActivityIndicatorDrink.IsRunning = true;
            NapojItemsList.Children.Clear();
            PizzaItemsList.Children.Clear();

            await SetUpAllPizza();
            await SetUpAllDrinks();

            _Drinks.Skip(_Drinks.Count / 2).ForEach(x => NapojItemsList.RowDefinitions.Add(new RowDefinition()
            {
                Height = 220
            }));
            _Drinks.ForEach(x => NapojItemsList.Children.Add(new ItemView(x), _Drinks.IndexOf(x) % 2, _Drinks.IndexOf(x) / 2));

            _Pizza.Skip(_Pizza.Count / 2).ForEach(x => PizzaItemsList.RowDefinitions.Add(new RowDefinition()
            {
                Height = 220
            }));
            _Pizza.ForEach(x => PizzaItemsList.Children.Add(new ItemView(x), _Pizza.IndexOf(x) % 2, _Pizza.IndexOf(x) / 2));

            this.ActivityIndicatorPizza.IsRunning = false;
            this.ActivityIndicatorDrink.IsRunning = false;
        }
        async Task SetUpAllPizza()
        {
            try
            {
                using (var HttpClient = new HttpApiConnector().GetClient())
                {
                    var response = await HttpClient.GetAsync(Constants.ConnectionApiUriPizza);
                    if (response.IsSuccessStatusCode)
                    {
                        string Content = await response.Content.ReadAsStringAsync();
                        var tempList = JsonConvert.DeserializeObject<List<PizzaModel>>(Content);
                        _Pizza = new ObservableCollection<PizzaModel>(tempList);
                    }
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Connection refused!", "Ok");
            }
        }
        async Task SetUpAllDrinks()
        {
            try
            {
                using (var HttpClient = new HttpApiConnector().GetClient())
                {
                    var response = await HttpClient.GetAsync(Constants.ConnectionApiUriNapoj);
                    if (response.IsSuccessStatusCode)
                    {
                        string Content = await response.Content.ReadAsStringAsync();
                        var tempList = JsonConvert.DeserializeObject<List<NapojModel>>(Content);
                        _Drinks = new ObservableCollection<NapojModel>(tempList);
                    }
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Connection refused!", "Ok");
            }
        }
    }
}