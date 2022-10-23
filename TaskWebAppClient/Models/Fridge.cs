using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskWebAPIServer.Models
{
    public class Fridge
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string OwnerName  { get; set; }

        [ForeignKey("FridgeModel")]
        public Guid FridgeModelId { get; set; }
        public FridgeModel FridgeModel { get; set; }

        public List<FridgeProduct> fridgeProducts = new ();
    }
}
