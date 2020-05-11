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
    public partial class UserAccountPage : CarouselPage
    {

        public UserAccountPage()
        {
            InitializeComponent();
            this.RegisterPage.ParentPage = this;
        }
    }
}