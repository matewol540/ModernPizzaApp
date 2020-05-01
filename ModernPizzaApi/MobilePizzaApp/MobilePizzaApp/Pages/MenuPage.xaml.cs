using System;
using System.Collections.Generic;
using System.Net.Http;
using MobilePizzaApp.ApiConnector;
using MobilePizzaApp.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : CarouselPage
    {
        public MenuPage()
        {
            InitializeComponent();
            SetUpAllPizza();
        }
        async void SetUpAllPizza()
        {
            try
            {
                using (var HttpClient = new HttpApiConnector().GetClient())
                {
                    var response = await HttpClient.GetAsync(Constants.ConnectionApiUri);
                    if (response.IsSuccessStatusCode)
                    {
                        string Content = await response.Content.ReadAsStringAsync();
                        var tempList = JsonConvert.DeserializeObject<List<PizzaModel>>(Content);
                        PizzaItemsList.ItemsSource = tempList;
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

        private void AddItemToOrderList(object sender, EventArgs e)
        {

        }
    }
}