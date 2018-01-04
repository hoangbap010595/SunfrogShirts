using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunfrogShirtsV2.Models
{
    public class OProduct
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double BasePrice { get; set; }
        public double BackPrintPrice { get; set; }
        public string ImageFront { get; set; }
        public string ImageBack { get; set; }
        public List<OColor> Colors { get; set; }

    }
}
