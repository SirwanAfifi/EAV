using System.ComponentModel.DataAnnotations.Schema;

namespace EAV.Models
{
    public class EmployeeAttribute
    {
        public int Id { get; set; }
        public virtual EmployeeEav Employee { get; set; }
        public int EmployeeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
    
    public class EmployeeJsonAttribute
    {
        public int Id { get; set; }
        public virtual EmployeeEav Employee { get; set; }
        public int EmployeeId { get; set; }
        [Column(TypeName = "json")]
        public string Attributes { get; set; }
    }
}