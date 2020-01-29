using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BeerBong.Models;
using BeerBong.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.NavigationPage;

namespace BeerBong.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        bool isNewItem;
        public LoginPage(bool isNew = false)
        {
            InitializeComponent();
            isNewItem = isNew;
        }


        private void Button_OnClicked(object sender, EventArgs e)
        {
            Tænker.IsRunning = true;
        }
    }
}