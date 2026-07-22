namespace Steam2Api.Dtos.Game
{
    public class GameDto
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }

        public bool State { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }
    }
}
