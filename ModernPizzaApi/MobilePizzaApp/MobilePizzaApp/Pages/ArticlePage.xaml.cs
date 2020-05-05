using MobilePizzaApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticlePage : ContentPage
    {
        ArticleModel artyukul;
        public ArticlePage()
        {
            InitializeComponent();
        }
        public ArticlePage(ArticleModel article)
        {
            InitializeComponent();
            this.ArticleMainImage.Source = ImageSource.FromStream(() => new MemoryStream(article.obraz));
            artyukul = article;
            this.MainStack.BindingContext = article;
        }
    }
}