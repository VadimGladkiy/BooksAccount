using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksAccount.Models
{
    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int book_id { set; get; }

        [Required]
        [MaxLength(100)]
        public string authors_names { set; get; }

        [Required]
        [MaxLength(100)]
        public string book_name { set; get; }

       
        [RegularExpression(@"[\d\-]{11,17}")]
        public string ISBN { set; get; }

        [Required]
        public double book_cost { set; get; }

        public string genre { set; get; }
    }        
}