using Microsoft.Azure.Mobile.Server;
using System;

namespace ITW_MobileAppService.DataObjects
{
    public class EventItem : EntityData
    {
        public string Name { get; set; }

        public string EventRecipients { get; set; }

        public DateTime EventDate { get; set; }

        public string EventTime { get; set; }

        public string Location { get; set; }

        public string Category { get; set; }

        public string EventPriority { get; set; }

        public string EventDescription { get; set; }

        public int EventID { get; set; }

        public int EmployeeID { get; set; }

        public bool IsDeleted { get; set; }
    }
    public class EmployeeItem : EntityData
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public int EmployeeID { get; set; }

        public string Department { get; set; }

        public string PrivledgeLevel { get; set; }
    }
    public class RecipientListItem : EntityData
    {
        public int EmployeeID { get; set; }

        public int EventID { get; set; }
    }
    public class EmployeeLoginItem : EntityData
    {
        public int EmployeeID { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}