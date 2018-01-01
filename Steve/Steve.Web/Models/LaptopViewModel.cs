using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steve.Web.Models
{
    public class LaptopViewModel
    {
        public string Name { get; set; }
        public string Processor { get; set; }
        public string DiagonalScreen { get; set; }
        public string OperatingSystem { get; set; }
        public int HardDiskSpace { get; set; }
        public string Color { get; set; }
        public string GraphicAdapter { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}
