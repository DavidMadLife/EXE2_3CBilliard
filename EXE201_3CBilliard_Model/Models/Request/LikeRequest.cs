﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Models.Request
{
    public class LikeRequest
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
    }
}
