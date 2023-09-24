namespace ApiJwtLearning.Model
{
    public class AuthModel
    {
        public string meassage { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Eamil { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
