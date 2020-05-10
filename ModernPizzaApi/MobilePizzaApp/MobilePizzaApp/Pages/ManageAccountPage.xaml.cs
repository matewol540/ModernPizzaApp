﻿using MobilePizzaApp.Models;
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
    public partial class ManageAccountPage : ContentPage
    {
        public UserModel User { get; set; }
        public ManageAccountPage()
        {
            InitializeComponent();
        }
    }
}