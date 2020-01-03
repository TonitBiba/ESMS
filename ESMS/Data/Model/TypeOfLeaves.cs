using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class TypeOfLeaves
    {
        public TypeOfLeaves()
        {
            Leaves = new HashSet<Leaves>();
        }

        public int Id { get; set; }
        public string NameSq { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<Leaves> Leaves { get; set; }
    }
}
