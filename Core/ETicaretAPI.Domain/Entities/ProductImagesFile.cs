﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entities
{
    public class ProductImagesFile : File
    {
        public ICollection<Product> Products { get; set; }
    }
}
