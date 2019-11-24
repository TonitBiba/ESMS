using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class States
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ContryId { get; set; }
    }
}
