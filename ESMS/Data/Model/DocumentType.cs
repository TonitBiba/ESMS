using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class DocumentType
    {
        public DocumentType()
        {
            EmployeeDocuments = new HashSet<EmployeeDocuments>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NInsertedId { get; set; }
        public DateTime DtInserted { get; set; }
        public string NModifyId { get; set; }
        public DateTime? DtModify { get; set; }

        public virtual ICollection<EmployeeDocuments> EmployeeDocuments { get; set; }
    }
}
