using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoBileti.Models
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }
        public String ownerId { get; set; }
        public virtual KinoBiletUser owner { get; set; }
        public virtual ICollection<BiletInShoppingCart> biletInShoppingCarts { get; set; }

        
    }
}
