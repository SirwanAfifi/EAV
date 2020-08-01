namespace EVA_Model.Models
{
    public class EmployeeAttribute
    {
        public int Id { get; set; }
        public virtual EmployeeEav Employee { get; set; }
        public int EmployeeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }
}