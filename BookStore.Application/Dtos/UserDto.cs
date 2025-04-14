namespace BookStore.Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public DateTime? Dob { get; set; }

        public int? Gender { get; set; }

        public int? AccountId { get; set; }

        public bool? Active { get; set; }
    }
}
