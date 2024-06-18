using MainApi.DataLayer.Entities;
using Manager.WebApp.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Manager.WebApp.Models
{
    public class ManageContactFormModel : CommonPagingModel
    {
        public List<IdentityContactForm> SearchResults { get; set; }
    }
    public class ContactFormDetailModel : IdentityContactForm
    {
        public string CurrentTab { get; set; }
    }
    public class ContactFormFullDetailModel
    {
        public ContactFormFullDetailModel()
        {
            ContactForm = new ContactFormModel();
            Allowance = new AllowanceModel();
            AllowanceDetail = new AllowanceDetailModel();
            Dependents = new List<DependentModel>();
        }
        public ContactFormModel ContactForm { get; set; }
        public AllowanceModel Allowance { get; set; }
        public AllowanceDetailModel AllowanceDetail { get; set; }
        public List<DependentModel> Dependents { get; set; }

    }
    //[Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]
    public class ListContactFormFullDetailModel
    {
        public List<ContactFormFullDetailModel> listContactForm { get; set; }
    }
    public class ContactFormModel
    {
        public int FormId { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]
        public string Furigana { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]
        public string FullName { get; set; }
        public int Gender { get; set; }
        public string EmployeeClassification { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]
        public string DepartmentName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]
        public string Furigana2 { get; set; }

        public string Address { get; set; }
        public string Transportation { get; set; }
        public string NenkinNumber { get; set; }
        public string Insurance { get; set; }
        public string PreviousJob { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]
        public string Remarks { get; set; }
        public string ForOffice { get; set; }
        public int OwnerId { get; set; }
        public int Status { get; set; }
    }

    public class AllowanceModel
    {
        public int AllowanceId { get; set; }
        public int FormId { get; set; }
        public string SalaryType { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public int Salary { get; set; }
        public string CommutingAllowanceType { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public int CommutingAllowance { get; set; }
        public int TotalMonthlyAmount { get; set; }
        public bool ApplicableToTax { get; set; }
    }
    public class AllowanceDetailModel
    {
        public int Id { get; set; }
        public int AllowanceId { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public int Position { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public int Attendance { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public int Alimony { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public int Allowance { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class DependentModel
    {
        public int DependentId { get; set; }
        public int FormId { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public string DependentSpouseNenkinNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public string Furigana { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public string FullName { get; set; }
        public int Gender { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public string Relationship { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public string Occupation { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public int AnualIncome { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessageResourceType = typeof(ManagerResource), ErrorMessageResourceName = nameof(ManagerResource.COMMON_ERROR_NULL_VALUE))]

        public DateTime EnrollmentDate { get; set; }
    }
}
