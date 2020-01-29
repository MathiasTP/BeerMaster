using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BeerBong.Models;
using Newtonsoft.Json;

namespace BeerBong.Services
{
    
    public class RestService : IRestService
    {
        HttpClient _client;

        
        public List<OnlineLeaderboard> LeaderboardTider { get; set; }

        

        public RestService()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            _client = new HttpClient(clientHandler);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
        }

        public async Task<List<OnlineLeaderboard>> RefreshDataAsync()
        {
            LeaderboardTider = new List<OnlineLeaderboard>();
            
            try
            {
                var response = await _client.GetAsync("https://webapiprojekt420191125022155.azurewebsites.net/api/LeaderBoard/GetTopTimes");
               
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    LeaderboardTider = JsonConvert.DeserializeObject<List<OnlineLeaderboard>>(content);
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return LeaderboardTider;
            
        }

        public async Task<bool> GetLoginDataAsync(LoginUser login)
        {
            bool status = false;
            try
            {
                var json = JsonConvert.SerializeObject(login);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage response = null;
                response = await _client.PostAsync(
                    "https://webapiprojekt420191125022155.azurewebsites.net/api/Identity/Login", content);
                if (response.IsSuccessStatusCode)
                {
                    var jsonlogin = await response.Content.ReadAsAsync<LoginResponse>();
                    App.Token = jsonlogin.token;
                     status = true;
                     _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
                     var player = new Player();
                     await AddPlayer(player);
                     await GetPlayerId(player);

                }
                else
                {
                    status = false;
                }
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return status;
        }

        public async Task AddPlayer(Player player)
        {
            var json = JsonConvert.SerializeObject(player);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            
            try
            {
                response = await _client.PostAsync(
                    "https://webapiprojekt420191125022155.azurewebsites.net/api/Players/Add", content);
                var jsonlogin = await response.Content.ReadAsAsync<Player>();
                App.player = jsonlogin;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        
        public async Task<bool> QueueGetPlayer(QueueModstander modstander)
        {
            var json = JsonConvert.SerializeObject(modstander);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            try
            {
                await _client.PostAsync("https://webapiprojekt420191125022155.azurewebsites.net/api/Queue/AddPlayer?PlayerId=" + App.player.PlayerId, content);
                response = await _client.GetAsync("https://webapiprojekt420191125022155.azurewebsites.net/api/Queue/GetFirstPlayer");
                if (response.IsSuccessStatusCode)
                {
                    var jsonlogin = await response.Content.ReadAsAsync<QueueModstander>();
                    App.modstander = jsonlogin;
                    if (App.modstander.Playerid != App.player.PlayerId)
                    {
                        await CreateGame(App.game);
                        await AddPlayerToGame(App.game);
                        await RemovePlayerQueue();
                        return true;
                        
                    }
                    //await RemovePlayerQueue();
                    return false;
                }

               // await _client.PostAsync("https://webapiprojekt420191125022155.azurewebsites.net/api/Queue/AddPlayer?PlayerId=" + App.player.PlayerId, content);
                return false;

            }
            catch (Exception e)
            {
                Debug.WriteLine(@"\tERROR {0}", e.Message);
                return false;
            }
        }


        public async Task<bool> SaveOpretBrugerAsync(RegisterUser bruger)
        {
            try
            {
                var json = JsonConvert.SerializeObject(bruger);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                
                response = await _client.PostAsync("https://webapiprojekt420191125022155.azurewebsites.net/api/Identity/Register", content);


                if (response.IsSuccessStatusCode)
                {
                    var jsonlogin = await response.Content.ReadAsAsync<LoginResponse>();
                    App.Token = jsonlogin.token;
                    

                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.Token);
                    Debug.WriteLine(@"\tBruger er oprettet");
                    return true;
                    
                    
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                return false;
            }
        }

        public async Task PushTimes(int id)
        {
            try
            {
                HttpResponseMessage response = null;

                response = await _client.PutAsync($"https://webapiprojekt420191125022155.azurewebsites.net/api/LeaderBoard/InsertTopTimes?playerId={id}", null);


                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tStats er pushet");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async Task<List<GameResult>> GetGameResult(int id)
        {
            var result = new List<GameResult>();

            try
            {
                var response = await _client.GetAsync($"https://webapiprojekt420191125022155.azurewebsites.net/api/Game/GetResult?gameid={id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<GameResult>>(content);
                    App.gameresultat = content;

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return result;
        }


        public async Task<Game> CreateGame(Game game)
        {
            var jsongame = new Game();
            try
            {
                var json = JsonConvert.SerializeObject(game);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await _client.PostAsync("https://webapiprojekt420191125022155.azurewebsites.net/api/Game/Add", content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\t Game er oprettet");
                    jsongame = await response.Content.ReadAsAsync<Game>();
                    App.game = jsongame;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                
            }

            return jsongame;
        }

        public async Task RemovePlayerQueue()
        {
            HttpResponseMessage response = null;
            var json = JsonConvert.SerializeObject(App.modstander.Playerid);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                response = await _client.PutAsync(
                    "https://webapiprojekt420191125022155.azurewebsites.net/api/Queue/RemovePlayer?playerId=" +
                    App.modstander.Playerid, content);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Player slettet fra queue");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
        }

        public async Task AddPlayerToGame(Game game)
        {
           int gameid = App.game.gameId;
           int player1id = App.player.PlayerId;
           int modstanderid = App.modstander.Playerid;

            var uri = new Uri($"https://webapiprojekt420191125022155.azurewebsites.net/api/Game/AddPlayerToGame?gameid={App.game.gameId}&player1id={App.player.PlayerId}&player2id={App.modstander.Playerid}");
            try
            {
                var json = JsonConvert.SerializeObject(game);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                
                response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\t Game er oprettet");
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

        }

        public async Task AddStats(Stats stat)
        {
            var uri = new Uri("https://webapiprojekt420191125022155.azurewebsites.net/api/Stats/Add");
            try
            {
                var json = JsonConvert.SerializeObject(stat);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\t Stats er tilføjet");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async Task<List<WebsocketData>> GetWebsocketData()
        {
            var Data = new List<WebsocketData>();

            try
            {
                var response = await _client.GetAsync("http://192.168.43.171:3000");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Data = JsonConvert.DeserializeObject<List<WebsocketData>>(content);
                    Data = App.BeerBongData;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return Data;

        }

        public async Task<int> GetPlayerId(Player player)
        {
            player = new Player();

            try
            {
                var response = await _client.GetAsync("https://webapiprojekt420191125022155.azurewebsites.net/api/Players/GetPlayerId");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    player = JsonConvert.DeserializeObject<Player>(content);
                    App.player.PlayerId = player.PlayerId;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            return player.PlayerId;
            
        }
    }
}
