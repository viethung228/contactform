namespace MainApi.DataLayer.Entities
{
    public class IdentityContactForm : IdentityCommon
    {
        public int FormId { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Furigana { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        public string EmployeeClassification { get; set; }
        public string DepartmentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Furigana2 { get; set; }
        public string Address { get; set; }
        public string Transportation { get; set; }
        public string NenkinNumber { get; set; }
        public string Insurance { get; set; }
        public string PreviousJob { get; set; }
        public string Remarks { get; set; }
        public string ForOffice { get; set; }
        public int OwnerId { get; set; }
        public int Status { get; set; }
    }    
    public class IdentityAllowance
    {
        public int AllowanceId { get; set; }
        public int FormId { get; set; }
        public string SalaryType { get; set; }
        public int Salary { get; set; }
        public string CommutingAllowanceType { get; set; }
        public int CommutingAllowance { get; set; }
        public int TotalMonthlyAmount { get; set; }
        public bool ApplicableToTax { get; set; }
    }
    public class IdentityAllowanceDetail
    {
        public int Id { get; set; }
        public int AllowanceId { get; set; }
        public int Position { get; set; }
        public int Attendance { get; set; }
        public int Alimony { get; set; }
        public int Allowance { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class IdentityDependent
    {
        public int DependentId { get; set; }
        public int FormId { get; set; }
        public string DependentSpouseNenkinNumber { get; set; }
        public string Furigana { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Relationship { get; set; }
        public string Occupation { get; set; }
        public int AnualIncome { get; set; }
        public string Address { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
