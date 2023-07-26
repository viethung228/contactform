using System;
using System.Collections.Generic;
using Serilog;
using MainApi.DataLayer.Entities;
using Manager.WebApp.Models;
using Manager.WebApp.Services;
using System.Reflection;

namespace Manager.WebApp.Helpers
{
    public class HelperContactForm
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(HelperContactForm));
        public static IdentityContactForm UpdateContactFormDetail(ContactFormFullDetailModel identity)
        {
            IdentityContactForm baseInfo = null;

            try
            {
                var apiRs = ContactFormServices.UpdateContactFormAsync(identity).Result;

                baseInfo = apiRs.ConvertData<IdentityContactForm>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        
        public static IdentityAllowance UpdateAllowanceDetail(ContactFormFullDetailModel identity)
        {
            IdentityAllowance baseInfo = null;

            try
            {
                var apiRs = ContactFormServices.UpdateAllowanceAsync(identity).Result;

                baseInfo = apiRs.ConvertData<IdentityAllowance>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static IdentityContactForm GetContactFormByFormId(int formId)
        {
            IdentityContactForm baseInfo = null;

            try
            {
                var apiRs = ContactFormServices.GetContactFormByFormIdAsync(formId).Result;

                baseInfo = apiRs.ConvertData<IdentityContactForm>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static IdentityAllowanceDetail UpdateAllowanceDetailAsync(ContactFormFullDetailModel identity)
        {
            IdentityAllowanceDetail baseInfo = null;

            try
            {
                var apiRs = ContactFormServices.UpdateContactFormAsync(identity).Result;

                baseInfo = apiRs.ConvertData<IdentityAllowanceDetail>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
        public static List<IdentityDependent> UpdateDependentDetail(ContactFormFullDetailModel identity)
        {
            List<IdentityDependent> baseInfo = null;

            try
            {
                var apiRs = ContactFormServices.UpdateDependentAsync(identity).Result;

                baseInfo = apiRs.ConvertData<List<IdentityDependent>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }


        public static List<IdentityContactForm> GetAllCompany(string companyName, int currentpage = 1, int pagesize = 10)
        {
            List<IdentityContactForm> baseInfo = null;

            try
            {
                var apiRs = ContactFormServices.GetAllCompany(companyName, currentpage, pagesize).Result;

                baseInfo = apiRs.ConvertData<List<IdentityContactForm>>();

            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityContactForm> GetByPage(string keyword, int currentpage = 1, int pagesize = 10)
        {
            List<IdentityContactForm> baseInfo = null;

            try
            {
                var apiRs = ContactFormServices.GetByPage(keyword, currentpage, pagesize).Result;

                baseInfo = apiRs.ConvertData<List<IdentityContactForm>>();

            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityContactForm> GetList()
        {
            List<IdentityContactForm> baseInfo = null;

            try
            {
                var apiRs = ContactFormServices.GetList().Result;

                baseInfo = apiRs.ConvertData<List<IdentityContactForm>>();

            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }

        public static List<IdentityContactForm> GetContactFormByCompanyName(string keyword, string companyName, int currentpage = 1, int pagesize = 10)
        {
            List<IdentityContactForm> baseInfo = null;

            try
            {
                var apiRs = ContactFormServices.GetContactFormByCompanyName(keyword, companyName, currentpage, pagesize).Result;

                baseInfo = apiRs.ConvertData<List<IdentityContactForm>>();
            }
            catch (Exception ex)
            {
                _logger.Error("Function {0} error: {1}", MethodBase.GetCurrentMethod().ReflectedType.FullName, ex.ToString());
            }

            return baseInfo;
        }
    }
}