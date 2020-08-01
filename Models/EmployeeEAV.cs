using System.Collections.Generic;

namespace EVA_Model.Models
{
    public class EmployeeEav
    {
        public int Id { get; set; }
        public virtual ICollection<EmployeeAttribute> Attributes { get; set; }
    }
}