﻿using System;

namespace AspMvc.Models
{
    public class EntryModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string workPhone { get; set; }
        public string workDescription { get; set; }
        public string roomNumber { get; set; }
        public string roomPhone { get; set; }
    }
}