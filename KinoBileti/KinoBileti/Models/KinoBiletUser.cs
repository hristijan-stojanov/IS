using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoBileti.Models
{
    public class KinoBiletUser : IdentityUser
    {
        public String ime { get; set; }
        public String prezime {   get; set; }
        public String  uloga { get; set; }
        public virtual ShoppingCart userCart { get; set; }
        // dodadeno
        public virtual ICollection<Order>  Orders { get; set; }
    }
}
