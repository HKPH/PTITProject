namespace BookStore.Dtos
{
    public class BookSoldDTO
    {
        public DateTime Date { get; set; }
        public int BooksSold { get; set; }
    }

    public class RevenueDTO
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
    }


    public class MonthlyBookSoldDTO
    {
        public int Month { get; set; }
        public int BooksSold { get; set; }
    }

    public class MonthlyRevenueDTO
    {
        public int Month { get; set; }
        public decimal Revenue { get; set; }
    }



    public class QuarterlyBookSoldDTO
    {
        public int Quarter { get; set; }
        public int BooksSold { get; set; }
    }

    public class QuarterlyRevenueDTO
    {
        public int Quarter { get; set; }
        public decimal Revenue { get; set; }
    }
    public class RatingByStarDTO
    {
        public int? Star { get; set; }
        public int Count { get; set; }
    }

    public class RatingDTO
    {
        public DateTime Date { get; set; }
        public List<RatingByStarDTO> RatingsCountByStar { get; set; }
    }

    public class MonthlyRatingDTO
    {
        public int Month { get; set; }
        public List<RatingByStarDTO> RatingsCountByStar { get; set; }
    }

    public class QuarterlyRatingDTO
    {
        public int Quarter { get; set; }
        public List<RatingByStarDTO> RatingsCountByStar { get; set; }
    }
}
