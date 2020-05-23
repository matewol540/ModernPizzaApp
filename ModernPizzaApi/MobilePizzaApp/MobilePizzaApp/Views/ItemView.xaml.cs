using MobilePizzaApp.Interface;
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
    public partial class ItemView : ContentView
    {
        public ItemView()
        {
            InitializeComponent();
            var model = this.BindingContext as IItemTemplate;

            ItemObject.BindingContext = model;
            if (model.Obraz != null)
                ItemImage.Source = ImageSource.FromStream(() => new MemoryStream(model.Obraz));
            else
                ItemImage.Source = ImageSource.FromUri(new Uri("https://via.placeholder.com/150"));
        }
        public ItemView(IItemTemplate model)
        {
            InitializeComponent();
            if (model == null)
                model = this.BindingContext as IItemTemplate;
            ItemObject.BindingContext = model;
            if (model.Obraz != null)
                ItemImage.Source = ImageSource.FromStream(() => new MemoryStream(model.Obraz));
            else
                ItemImage.Source = ImageSource.FromUri(new Uri("https://via.placeholder.com/150"));
        }
    }
}