using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoBileti.Models
{
    public class Bilet
    {
        public Guid Id { get; set; }
        public String ime { get; set; }
        public DateTime datum { get; set; }
        public int cena { get; set; }
        public String zanr { get; set; }
        public virtual ICollection<BiletInShoppingCart> biletInShoppingCarts { get; set; }
        public virtual ICollection<BiletInOrder> Orders { get; set; }
    }
}
