using MobilePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (model.obraz != null)
                ArticleImage.Source = ImageSource.FromStream(() => new MemoryStream(model.obraz));
            else
                ArticleImage.Source = ImageSource.FromUri(new Uri("https://via.placeholder.com/150"));

        }
    }
}