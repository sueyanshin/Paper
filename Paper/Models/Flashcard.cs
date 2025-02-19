using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paper.Models
{
    public class Flashcard
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsFlipped { get; set; }
    }
}
