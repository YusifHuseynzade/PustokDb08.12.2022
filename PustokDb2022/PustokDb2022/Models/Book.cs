using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PustokDb2022.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokDb2022.Models
{
    public class Book : BaseEntity
    {
        public int GenreId { get; set; }
        public int AuthorId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public bool StockStatus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisCountPercent { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsNew { get; set; }
        [NotMapped]
        public IFormFile? PosterFile { get; set; }
        [NotMapped]
        public IFormFile? HoverPosterFile { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }
        [NotMapped]
        public List<int>? BookImageIds { get; set; }
        public Genre? Genres { get; set; }
        public Author? Authors { get; set; }
        public List<BookImage>? BookImages { get; set; }

    }
}
