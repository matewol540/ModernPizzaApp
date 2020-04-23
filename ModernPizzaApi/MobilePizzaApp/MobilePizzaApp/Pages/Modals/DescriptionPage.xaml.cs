using MobilePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DescriptionPage : ContentPage
    {
        private PizzaModel pizza;
        public DescriptionPage()
        {
            InitializeComponent();
        }
        public DescriptionPage(PizzaModel pizza)
        {
            if (pizza == null)
                throw new ArgumentNullException();
            InitializeComponent();
            this.pizza = pizza;
        }

        async void CloseModalButton_Clicked(object sender, EventArgs e)
        {
            if (Navigation.ModalStack.Count > 0)
                await Navigation.PopModalAsync();
        }
    }
}