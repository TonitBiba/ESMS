using System;
using System.Collections.Generic;

namespace ESMS.Data.Model
{
    public partial class Notifications
    {
        public int NNotificationId { get; set; }
        public string Title { get; set; }
        public string VcText { get; set; }
        public string VcIcon { get; set; }
        public string VcUser { get; set; }
        public DateTime DtInserted { get; set; }
        public string VcInsertedUser { get; set; }

        public virtual AspNetUsers VcInsertedUserNavigation { get; set; }
        public virtual AspNetUsers VcUserNavigation { get; set; }
    }
}
