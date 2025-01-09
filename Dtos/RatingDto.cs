namespace BookStore.Dtos
{
    public class RatingDto 
    {
        public int Id { get; set; }

        public int? BookId { get; set; }

        public int? UserId { get; set; }

        public int? Value { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? Comment { get; set; }

        public string? Photo { get; set; }

        public bool? Active { get; set; }
    }

}
