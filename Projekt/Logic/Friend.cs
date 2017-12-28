using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
   public class Friend
    {
        public int ID { get; set; }
        public virtual ApplicationUser Requester { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        public bool Accepted { get; set; }
    }
}
