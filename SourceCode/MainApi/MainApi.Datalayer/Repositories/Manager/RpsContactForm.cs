using MainApi.DataLayer.Entities;
using MainApi.SharedLibs;
using MainApi.SharedLibs.Extensions;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MainApi.DataLayer.Repositories.Manager
{
    public class RpsContactForm
    {
        private readonly string _conStr;

        public RpsContactForm(string connectionString)
        {
            _conStr = connectionString;
        }

        public RpsContactForm()
        {
            _conStr = AppConfiguration.GetAppsetting("MainDBConn");
        }
        #region Common
        public static IdentityContactForm ExtractContactFormData(IDataReader reader)
        {
            var record = new IdentityContactForm();

            //Seperate properties
            record.FormId = Utils.ConvertToInt32(reader["FormId"]);
            record.CompanyName = reader["CompanyName"].ToString();
            record.CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime)reader["CreatedDate"] : DateTime.MinValue;
            record.UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? (DateTime)reader["UpdatedDate"] : DateTime.MinValue;
            record.Furigana = reader["Furigana"].ToString();
            record.PhoneNumber = reader["PhoneNumber"].ToString();
            record.FullName = reader["FullName"].ToString();
            record.Gender = Utils.ConvertToInt32(reader["Gender"]);
            record.EmployeeClassification = reader["EmployeeClassification"].ToString();
            record.DepartmentName = reader["DepartmentName"].ToString();
            record.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : DateTime.MinValue;
            record.DateOfJoining = reader["DateOfJoining"] != DBNull.Value ? (DateTime)reader["DateOfJoining"] : DateTime.MinValue;
            record.Furigana2 = reader["Furigana2"].ToString();
            record.Address = reader["Address"].ToString();
            record.Transportation = reader["Transportation"].ToString();
            record.NenkinNumber = reader["NenkinNumber"].ToString();
            record.Insurance = reader["Insurance"].ToString();
            record.PreviousJob = reader["PreviousJob"].ToString();
            record.Remarks = reader["Remarks"].ToString();
            record.ForOffice = reader["ForOffice"].ToString();
            record.OwnerId = Utils.ConvertToInt32(reader["OwnerId"]);
            record.Status = Utils.ConvertToInt32(reader["Status"]);
            if (reader.HasColumn("TotalCount"))
                record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);

            return record;
        }
        public static IdentityContactForm ExtractCompanyData(IDataReader reader)
        {
            var record = new IdentityContactForm();

            //Seperate properties
            record.CompanyName = reader["CompanyName"].ToString();
            record.TotalCount = Utils.ConvertToInt32(reader["TotalCount"]);
            return record;
        }
        public static IdentityDependent ExtractDependentData(IDataReader reader)
        {
            var record = new IdentityDependent();

            //Seperate properties
            record.FormId = Utils.ConvertToInt32(reader["FormId"]);
            record.DependentId = Utils.ConvertToInt32(reader["DependentId"]);
            record.DependentSpouseNenkinNumber = reader["DependentSpouseNenkinNumber"].ToString();
            record.Furigana = reader["Furigana"].ToString();
            record.FullName = reader["FullName"].ToString();
            record.Gender = Utils.ConvertToInt32(reader["Gender"]);
            record.DateOfBirth = reader["DateOfBirth"] != DBNull.Value ? (DateTime)reader["DateOfBirth"] : DateTime.MinValue;
            record.Relationship = reader["Relationship"].ToString();
            record.Occupation = reader["Occupation"].ToString();
            record.AnualIncome = Utils.ConvertToInt32(reader["AnualIncome"]);
            record.Address = reader["Address"].ToString();
            record.EnrollmentDate = reader["EnrollmentDate"] != DBNull.Value ? (DateTime)reader["EnrollmentDate"] : DateTime.MinValue;
            return record;
        }
        public static IdentityAllowance ExtractAllowanceData(IDataReader reader)
        {
            var record = new IdentityAllowance();

            //Seperate properties
            record.AllowanceId = Utils.ConvertToInt32(reader["AllowanceId"]);
            record.FormId = Utils.ConvertToInt32(reader["FormId"]);
            record.SalaryType = reader["SalaryType"].ToString();
            record.Salary = Utils.ConvertToInt32(reader["Salary"]);
            record.CommutingAllowanceType = reader["CommutingAllowanceType"].ToString();
            record.CommutingAllowance = Utils.ConvertToInt32(reader["CommutingAllowance"]);
            record.ApplicableToTax = Utils.ConvertToBoolean(reader["ApplicableToTax"]);
            record.TotalMonthlyAmount = Utils.ConvertToInt32(reader["TotalMonthlyAmount"]);
            return record;
        }
        public static IdentityAllowanceDetail ExtractAllowanceDetailData(IDataReader reader)
        {
            var record = new IdentityAllowanceDetail();

            //Seperate properties
            record.Id = Utils.ConvertToInt32(reader["Id"]);
            record.AllowanceId = Utils.ConvertToInt32(reader["AllowanceId"]);
            record.Position = Utils.ConvertToInt32(reader["Position"]);
            record.Attendance = Utils.ConvertToInt32(reader["Attendance"]);
            record.Alimony = Utils.ConvertToInt32(reader["Alimony"]);
            record.Allowance = Utils.ConvertToInt32(reader["Allowance"]);
            record.CreatedDate = reader["CreatedDate"] != DBNull.Value ? (DateTime)reader["CreatedDate"] : DateTime.MinValue;
            return record;
        }
        #endregion
        public List<IdentityContactForm> GetContactFormByCompanyName(string keyword, string companyName, int currentpage, int pagesize)
        {
            //Common syntax           
            var sqlCmd = @"Company_GetAllContactForm";
            var info = new List<IdentityContactForm>();
            //For parameters
            int offset = (currentpage - 1) * pagesize;
            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", keyword},
                {"@CompanyName", companyName},
                {"@Offset", offset},
                {"@PageSize", pagesize }
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (returnObj.Read())
                        {
                            var data = ExtractContactFormData(returnObj);
                            info.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }
        public List<IdentityContactForm> GetAllCompany(IdentityContactForm identity, int currentpage, int pagesize)
        {
            //Common syntax           
            var sqlCmd = @"Company_GetAll";
            var info = new List<IdentityContactForm>();
            int offset = (currentpage - 1) * pagesize;
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", identity.Keyword},
                {"@Offset", offset},
                {"@PageSize", pagesize }
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (returnObj.Read())
                        {
                            var data = ExtractCompanyData(returnObj);
                            info.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }

        public List<IdentityContactForm> GetByPage(string keyword, int currentpage, int pagesize)
        {
            //Common syntax           
            var sqlCmd = @"Company_ContactForm_GetByPage";
            var info = new List<IdentityContactForm>();
            int offset = (currentpage - 1) * pagesize;
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@Keyword", keyword},
                {"@Offset", offset},
                {"@PageSize", pagesize }
            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (returnObj.Read())
                        {
                            var data = ExtractContactFormData(returnObj);
                            info.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }

        public List<IdentityContactForm> GetList()
        {
            //Common syntax           
            var sqlCmd = @"Company_ContactForm_GetList";
            var info = new List<IdentityContactForm>();

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, null))
                    {
                        while (returnObj.Read())
                        {
                            var data = ExtractContactFormData(returnObj);
                            info.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }

        public IdentityDependent InsertDependent(IdentityDependent identity)
        {
            //Common syntax           
            var sqlCmd = @"Dependent_Insert";
            var info = new IdentityDependent();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FormId", identity.FormId},
                {"@DependentId", identity.DependentId},
                {"@DependentSpouseNenkinNumber", identity.DependentSpouseNenkinNumber},
                {"@Furigana", identity.Furigana},
                {"@FullName", identity.FullName},
                {"@Gender", identity.Gender},
                {"@DateOfBirth", identity.DateOfBirth},
                {"@Relationship", identity.Relationship},
                {"@Occupation", identity.Occupation},
                {"@AnualIncome", identity.AnualIncome},
                {"@Address", identity.Address},
                {"@EnrollmentDate", identity.EnrollmentDate}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractDependentData(returnObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }
        public IdentityAllowance InsertAllowance(IdentityAllowance identity)
        {
            //Common syntax           
            var sqlCmd = @"Allowance_Update";
            var info = new IdentityAllowance();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FormId", identity.FormId},
                {"@SalaryType", identity.SalaryType},
                {"@Salary", identity.Salary},
                {"@CommutingAllowanceType", identity.CommutingAllowanceType},
                {"@CommutingAllowance", identity.CommutingAllowance},
                {"@ApplicableToTax", identity.ApplicableToTax},
                {"@TotalMonthlyAmount", identity.TotalMonthlyAmount}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractAllowanceData(returnObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }
        public IdentityAllowanceDetail InsertAllowanceDetail(IdentityAllowanceDetail identity)
        {
            //Common syntax           
            var sqlCmd = @"Allowance_Type_Update";
            var info = new IdentityAllowanceDetail();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@AllowanceId", identity.AllowanceId},
                {"@Position", identity.Position},
                {"@Attendance", identity.Attendance},
                {"@Alimony", identity.Alimony},
                {"@Allowance", identity.Allowance}
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractAllowanceDetailData(returnObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }
        public IdentityContactForm InsertContactForm(IdentityContactForm identity)
        {
            //Common syntax           
            var sqlCmd = @"Company_ContactForm_Update";
            var info = new IdentityContactForm();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FormId", identity.FormId},
                {"@CompanyName", identity.CompanyName},
                {"@Furigana", identity.Furigana},
                {"@PhoneNumber", identity.PhoneNumber},
                {"@FullName", identity.FullName},
                {"@Gender",identity.Gender },
                {"@EmployeeClassification",identity.EmployeeClassification },
                {"@DepartmentName",identity.DepartmentName },
                {"@DateOfBirth",identity.DateOfBirth },
                {"@DateOfJoining",identity.DateOfJoining },
                {"@Furigana2",identity.Furigana2 },
                {"@Address",identity.Address },
                {"@Transportation",identity.Transportation },
                {"@NenkinNumber",identity.NenkinNumber },
                {"@Insurance",identity.Insurance },
                {"@PreviousJob",identity.PreviousJob },
                {"@Remarks",identity.Remarks },
                {"@ForOffice",identity.ForOffice },
                {"@OwnerId",identity.OwnerId }
            };

            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractContactFormData(returnObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }

        public IdentityAllowance GetAllowanceByFormId(int formId)
        {
            //Common syntax           
            var sqlCmd = @"Allowance_GetByFormId";
            var info = new IdentityAllowance();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FormId", formId}

            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractAllowanceData(returnObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }

        public List<IdentityDependent> GetDependentsByFormId(int formId)
        {
            //Common syntax           
            var sqlCmd = @"Dependent_GetByFormId";
            var info = new List<IdentityDependent>();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FormId", formId}

            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (returnObj.Read())
                        {
                            var data = ExtractDependentData(returnObj);
                            info.Add(data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }

        public IdentityContactForm GetContactFormByFormId(int formId)
        {
            //Common syntax           
            var sqlCmd = @"Company_ContactForm_GetById";
            var info = new IdentityContactForm();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FormId", formId}

            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractContactFormData(returnObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }
        public List<IdentityContactForm> GetContactFormByEmployeeId(int id)
        {
            //Common syntax           
            var sqlCmd = @"Company_ContactForm_GetByEmployeeId";
            var info = new List<IdentityContactForm>();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@OwnerId", id}

            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        while (returnObj.Read())
                        {
                            var dat = ExtractContactFormData(returnObj);
                            info.Add(dat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }
        public IdentityAllowanceDetail GetAllowanceDetailByAllowanceId(int allowanceId)
        {
            //Common syntax           
            var sqlCmd = @"AllowanceType_GetByAllowanceId";
            var info = new IdentityAllowanceDetail();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@AllowanceId", allowanceId}

            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractAllowanceDetailData(returnObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }

        public IdentityContactForm DeleteContactForm(int formId)
        {
            //Common syntax           
            var sqlCmd = @"Company_ContactForm_Delete";
            var info = new IdentityContactForm();
            //For parameters
            var parameters = new Dictionary<string, object>
            {
                {"@FormId", formId}

            };
            try
            {
                using (var conn = new SqlConnection(_conStr))
                {
                    using (var returnObj = MsSqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, sqlCmd, parameters))
                    {
                        if (returnObj.Read())
                        {
                            info = ExtractContactFormData(returnObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var strError = string.Format("Failed to execute {0}. Error: {1}", sqlCmd, ex.Message);
                throw new CustomSQLException(strError);
            }
            return info;
        }
    }
}
