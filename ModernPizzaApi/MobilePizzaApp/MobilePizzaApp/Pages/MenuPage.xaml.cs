using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Interface;
using MobilePizzaApp.Models;
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
            await SetUpAllPizza();
            await SetUpAllDrinks();
            _Drinks.ForEach(x => CreateChildToListView(x));
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
                        PizzaItemsList.ItemsSource = _Pizza;
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
                        NapojItemsList.ItemsSource = _Drinks;
                    }
                }
            }
            catch (Exception err)
            {
                await DisplayAlert("Error", "Connection refused!", "Ok");
            }
        }
        async void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (PizzaItemsList.SelectedItem == null)
                return;
            var Pizza = ((sender as ViewCell).BindingContext as PizzaModel);
            await Navigation.PushModalAsync(new DescriptionPage(Pizza));
            PizzaItemsList.SelectedItem = null;
        }

        void CreateChildToListView(IItemTemplate itemTemplate)
        {

        }
    }
}