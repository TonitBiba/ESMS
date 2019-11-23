using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class UserPosition
    {
        public string UserId { get; set; }
        public int PositionId { get; set; }
        public string NInsertedId { get; set; }
        public DateTime DtInserted { get; set; }
        public string NModifyId { get; set; }
        public DateTime? DtModify { get; set; }
    }
}
