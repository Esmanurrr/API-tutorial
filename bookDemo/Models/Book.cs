namespace bookDemo.Models
{
    public class Book
    {
        [RequiredControl]
        public int Id { get; set; }
        [RequiredControl]
        public String Title { get; set; }
        [RequiredControl]
        public decimal Price { get; set; }
    }
}
