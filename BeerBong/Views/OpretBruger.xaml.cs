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

namespace BeerBong.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OpretBruger : ContentPage
    {
       
        private bool isNewItem;
        public OpretBruger(bool isNew = false)
        {
            InitializeComponent();
            isNewItem = isNew;
            
        }

    }
}