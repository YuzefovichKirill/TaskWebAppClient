using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskWebAppClient.Models
{
    [NotMapped]
    public class FridgeProductVM
    {
        public Fridge fridge;
        public List<Product> products;
    }
}
