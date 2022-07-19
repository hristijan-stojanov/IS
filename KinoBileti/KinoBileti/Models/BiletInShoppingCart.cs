using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoBileti.Models
{
    public class BiletInShoppingCart
    {
        public Guid BiletId { get; set; }
        public Bilet bilet { get; set; }
        public Guid ShoppingCartId { get; set; }
        public ShoppingCart shoppingCart { get; set; }

    }
}
