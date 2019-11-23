using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class EmployeeDocuments
    {
        public int Id { get; set; }
        public string Employee { get; set; }
        public int? Type { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string NInsertedId { get; set; }
        public DateTime DtInserted { get; set; }
        public string NModifyId { get; set; }
        public DateTime? DtModify { get; set; }

        public virtual AspNetUsers EmployeeNavigation { get; set; }
        public virtual DocumentType TypeNavigation { get; set; }
    }
}
