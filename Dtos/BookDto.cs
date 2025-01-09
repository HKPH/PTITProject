using BookStore.Models;

namespace BookStore.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Author { get; set; }

        public int? PublisherId { get; set; }

        public decimal? Price { get; set; }

        public DateTime? PublicationDate { get; set; }

        public bool? Active { get; set; }

        public int? StockQuantity { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }

}
