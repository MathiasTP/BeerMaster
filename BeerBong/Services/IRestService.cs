using System.Collections.Generic;
using System.Threading.Tasks;
using BeerBong.Models;

namespace BeerBong.Services
{
	public interface IRestService
	{
		Task<List<OnlineLeaderboard>> RefreshDataAsync ();

        Task<bool> GetLoginDataAsync(LoginUser login);

        Task AddPlayer(Player player);

        Task<bool> QueueGetPlayer(QueueModstander modstander);

        Task<bool> SaveOpretBrugerAsync(RegisterUser bruger);

        Task PushTimes(int id);

        Task<List<GameResult>> GetGameResult(int id);

        Task<Game> CreateGame(Game game);

        Task RemovePlayerQueue();

        Task AddPlayerToGame(Game game);

        Task AddStats(Stats stat);

        Task<List<WebsocketData>> GetWebsocketData();

        Task<int> GetPlayerId(Player player);

    }
}
