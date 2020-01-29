using System.Collections.Generic;
using System.Threading.Tasks;
using BeerBong.Models;
using Xamarin.Forms;

namespace BeerBong.Services
{
	public class ServiceManager
	{
		IRestService restService;
       

        public ServiceManager (IRestService service)
		{
			restService = service;
		}

		

        public Task<List<OnlineLeaderboard>> GetOnlineLeaderboardAsync()
        {
            return restService.RefreshDataAsync();
        }

        public Task<bool> GetLoginDataAsync(LoginUser login)
        {
            return restService.GetLoginDataAsync(login);
        }

        public Task<bool> GetFirstPlayerInQueue(QueueModstander modstander)
        {
            return restService.QueueGetPlayer(modstander);
        }
		

        public Task<bool> SaveOpretBruger(RegisterUser item)
        {
            return restService.SaveOpretBrugerAsync(item);
        }

        public Task PushStats(Stats stats)
        {
            return restService.AddStats(stats);
        }

        public Task PushtoLeaderboard(int id)
        {
            return restService.PushTimes(id);
        }

        public Task<List<GameResult>> GetGameResult(int id)
        {
            return restService.GetGameResult(id);
        }
      
        public ToolbarItem AddLoginToToolbar()
        {
            var brugerToolbarItem = new ToolbarItem
            {
                Text = "brugernavn:" + App.BrugernavnOnLogIn,

                Order = ToolbarItemOrder.Primary,
                Priority = 0
            };
            return brugerToolbarItem;
        }
    }
}
