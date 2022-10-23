using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskWebAPIServer.Models
{
    public class FridgeModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int Year { get; set; }

        public List<Fridge> fridges = new();
    }
}
