using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KinoBileti.Models
{
    public class BiletInOrder
    {
        public Guid BiletId { get; set; }
        public Bilet BIlet { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
