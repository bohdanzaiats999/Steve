using Steve.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Steve.Web.Models
{
    public class LaptopListViewModel
    {
        public IList<LaptopModel> Laptops { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}
