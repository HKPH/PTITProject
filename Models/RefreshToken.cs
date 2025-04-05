namespace BookStore.Models
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

}
