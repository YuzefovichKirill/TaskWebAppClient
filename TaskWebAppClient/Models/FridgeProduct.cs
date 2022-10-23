using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskWebAPIServer.Models
{
    public class FridgeProduct
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Fridge")]
        public Guid FridgeId { get; set; }
        public Fridge Fridge { get; set; }
        
        public int Quantity { get; set; }
    }
}
