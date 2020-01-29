using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BeerBong.Models;
using BeerBong.Services;
using Xamarin.Forms;

namespace BeerBong.ViewModel
{
    public class LeaderboardViewModel : INotifyPropertyChanged
    {
        public List<OnlineLeaderboard> LeaderboardList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        RestService service = new RestService();

        public LeaderboardViewModel()
        { 
            
        }

        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        

        public async Task SetLeaderboard()
        {
            var service = new RestService();

            LeaderboardList = await service.RefreshDataAsync();
            App.board = LeaderboardList;
            OnPropertyChanged();
        }
    }
}
