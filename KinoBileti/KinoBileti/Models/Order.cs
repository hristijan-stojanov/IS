using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoBileti.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public String userId { get; set; }
        public KinoBiletUser user { get; set; }
        public virtual ICollection<BiletInOrder> Bilets { get; set; }
    }
}
