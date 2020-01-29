using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerBong.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeerBong.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Leaderboard : ContentPage
    {
        LeaderboardViewModel board = new LeaderboardViewModel();
        public Leaderboard()
        {
            InitializeComponent();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            this.ToolbarItems.Add(App.ServiceManager.AddLoginToToolbar());
            await board.SetLeaderboard();
            LeaderboardData.ItemsSource = board.LeaderboardList;
        }
    }
}