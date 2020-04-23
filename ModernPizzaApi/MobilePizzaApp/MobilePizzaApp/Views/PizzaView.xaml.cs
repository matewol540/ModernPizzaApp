using MobilePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PizzaView : ViewCell
    {
        private PizzaModel pizza;
        public PizzaView(PizzaModel pizza)
        {
            InitializeComponent();
            this.pizza = pizza;
        }
    }
}