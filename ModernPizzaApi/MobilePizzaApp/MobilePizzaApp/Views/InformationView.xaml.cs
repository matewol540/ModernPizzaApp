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
    public partial class InformationView : ContentView
    {

        public InformationView()
        {
            InitializeComponent();
        }
        public InformationView(ArticleModel model)
        {
            InitializeComponent();
            ArticleObject.BindingContext = model;
        }
    }
}