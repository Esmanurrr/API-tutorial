using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
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
