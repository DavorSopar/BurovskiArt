using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtistPortfolio.Models.Models
{
	public class Image
	{
        public long Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string? TitleMK { get; set; }
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string? TitleEN { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string? DescMK { get; set; }
        [Required]
        [Column(TypeName = "varchar(max)")]
        public string? DescEN { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string? TechniqueMK { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string? TechniqueEN { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string? Format { get; set; }
        [Required]
        public bool IsForSale { get; set; }
        [Required]
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? ImageUrl { get; set; }

        public bool AddedToCart { get; set; }

        public int? Price { get; set; }
    }
}

