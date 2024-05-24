using System.ComponentModel.DataAnnotations;

namespace API_CONSUME_PROJECT.Models
{
    public class STUDENT
    {
        [Key]
        public int ID { get; set; }
        public string? NAME { get; set; } //"?" this specifies means it will accept null value.
        public int AGE { get; set; }
        public string? ADDRESS { get; set; }
    }
}
