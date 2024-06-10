﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenTrail.Forms.ViewModel
{
    public class NewsViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Autor { get; set; }
        public DateTime Date { get; set; }
    }
}
