﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ShopOnline.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int CategoryId { get; set; }


        [ForeignKey("CategoryId")]
        public ProductCategory ProductCategory { get; set; }
    }
}