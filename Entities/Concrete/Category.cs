﻿using Core.Entites;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Category:BaseEntity,IEntity
    {
        public bool IsFeatured { get; set; }

        public List<CategoryProduct>? CategoryProducts { get; set; }
        public List<CategoryLanguage> CategoryLanguages { get; set; }
        
    
    

    }
}
