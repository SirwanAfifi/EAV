using System.Collections.Generic;

namespace EAV.Models
{
    public class EmployeeEav
    {
        public int Id { get; set; }
        public virtual ICollection<EmployeeAttribute> Attributes { get; set; }
    }
}