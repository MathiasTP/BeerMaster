namespace BeerBong.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public bool success { get; set; }
        public object errorMessage { get; set; }
    }
}