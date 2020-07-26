using System.ComponentModel.DataAnnotations;

namespace TestApi
{
    public class BookEntity
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public double Price { get; set; }
    }
}
