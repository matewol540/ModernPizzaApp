using MobilePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Pages.ReservationPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationHisotryPage : ContentPage
    {
        private List<RezerwacjaModel> HistoryList;
        public ReservationHisotryPage(List<RezerwacjaModel> history) :base()
        {
            InitializeComponent();
            HistoryList = history;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            HistoryItemList.ItemsSource = HistoryList;
        }

        private void HistoryItemList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            (sender as ListView).SelectedItem = null;
        }
    }
}