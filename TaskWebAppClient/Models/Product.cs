using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskWebAPIServer.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int DefaultQuantity { get; set; }

        public List<FridgeProduct> fridgeProducts = new();
    }
}
