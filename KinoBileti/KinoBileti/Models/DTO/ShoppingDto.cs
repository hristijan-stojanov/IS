using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoBileti.Models.DTO
{
    public class ShoppingDto
    {
        public List<BiletInShoppingCart> BiletInShoppingCarts { get; set; }
        public int cena { get; set; }
    }
}
