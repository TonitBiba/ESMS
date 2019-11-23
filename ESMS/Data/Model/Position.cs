using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class Position
    {
        public int Id { get; set; }
        public string NameSq { get; set; }
        public string NameEn { get; set; }
        public string NInsertedId { get; set; }
        public DateTime DtInserted { get; set; }
        public string NModifyId { get; set; }
        public DateTime? DtModify { get; set; }
    }
}
