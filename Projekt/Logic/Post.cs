using Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
   public class Post
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public virtual ApplicationUser From { get; set; }
        public virtual ApplicationUser To { get; set; }



    }
}
