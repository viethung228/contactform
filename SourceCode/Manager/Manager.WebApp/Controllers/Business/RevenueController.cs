using Manager.WebApp.Helpers;
using Manager.WebApp.Models;
using Manager.WebApp.Resources;
using Manager.WebApp.Services;
using Manager.WebApp.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using System;
using Manager.SharedLibs;
using MainApi.DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Manager.DataLayer.Stores;
using Autofac;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System.Web.Mvc;
using Newtonsoft.Json;
using Stripe;

namespace Manager.WebApp.Controllers
{
    public class RevenueController : BaseAuthedController
    {
        private readonly IStoreRevenue _mainStore;
        private readonly IStoreCustomer _cusStore;
        private readonly ILogger<RevenueController> _logger;

        public RevenueController(ILogger<RevenueController> logger)
        {
            _mainStore = Startup.IocContainer.Resolve<IStoreRevenue>();
            _cusStore = Startup.IocContainer.Resolve<IStoreCustomer>();
            _logger = logger;
        }

        public IActionResult Index(ManageRevenueModel model, int pagesize = 5, int currentpage = 1)
        {
            try
            {
                var identity = new RevenueDetailModel();
                model.Page = model.Page == 0 ? currentpage : model.Page;
                model.PageSize = model.PageSize == 0 ? pagesize : model.PageSize;
                if (!model.ListUserName.Contains("All"))
                {
                    model.ListUserName.Add("All");
                }
                if (model.UserName != null && model.UserName != "All")
                {

                    var getId = _cusStore.GetByCustomerName(model.UserName);
                    var info = HelperCustomer.GetBaseInfo(getId.StaffId);
                    if (info != null)
                    {
                        identity.UserId = info.Id;
                    }
                }

                ViewBag.TotalRevenue = 0;
                identity.FromDate = model.FromDate;
                identity.ToDate = model.ToDate == null ? null : model.ToDate.Value.AddDays(1);
                identity.SourceType = model.CoinSourceType;

                var data = _mainStore.GetHistoryAll(identity, model.PageSize, model.Page);
                model.TotalCount = data.FirstOrDefault().TotalCount;
                if (data != null && data.Count > 0)
                {
                    model.ListUserName.AddRange(_cusStore.GetAllCustomerNameHasRevenueHistory());
                    model.SearchResults = new List<RevenueViewModel>();
                    foreach (var item in data)
                    {
                        var dataTemp = _cusStore.GetById(item.UserId);
                        var valSourceType = (EnumSourceCoinType)item.SourceType;

                        model.SearchResults.Add(new RevenueViewModel
                        {
                            UserName = dataTemp == null ? "<i style='color:red;'>deleted user</i>" : dataTemp.UserName,
                            Price = item.Price,
                            Coin = item.Coin,
                            SourceType = valSourceType.ToString(),
                            CreatedDate = item.CreatedDate,
                            TotalCount = item.TotalCount,
                            TotalRevenue = item.TotalRevenue
                        });
                    }

                    if ((model.UserName != null && model.UserName != "All") || model.CoinSourceType != 10)
                    {
                        model.TotalCount = model.SearchResults.Count;
                        foreach (var dat in model.SearchResults)
                        {
                            ViewBag.TotalRevenue += dat.Price;
                        }

                    }
                    else
                    {
                        ViewBag.TotalRevenue = model.SearchResults.FirstOrDefault().TotalRevenue;
                    }

                    model.ListUserName = model.ListUserName.Distinct().ToList();

                    #region Chart

                    ViewBag.valMonthPurchase = new int[12];
                    ViewBag.valMonthAds = new int[12];
                    ViewBag.valDayOfWeekPurchase = new int[7];
                    ViewBag.valDayOfWeekAds = new int[7];

                    identity.SourceType = (int)EnumSourceCoinType.AppPurchase;
                    identity.SourceType = 10;
                    data = _mainStore.GetHistoryAll(identity, 0, currentpage);

                    ViewBag.AppPurchase = 0;
                    foreach (var i in data)
                    {
                        ViewBag.valMonthPurchase[i.CreatedDate.Month] += (int)i.Price;
                        ViewBag.valDayOfWeekPurchase[(int)i.CreatedDate.DayOfWeek - 1] += (int)i.Price;
                        ViewBag.AppPurchase += i.Price;
                    }

                    identity.SourceType = (int)EnumSourceCoinType.RemoveAds;

                    data = _mainStore.GetHistoryAll(identity, 0, currentpage);

                    ViewBag.RemoveAds = 0;
                    foreach (var i in data)
                    {
                        ViewBag.valMonthAds[i.CreatedDate.Month] += (int)i.Price;
                        ViewBag.valDayOfWeekAds[(int)i.CreatedDate.DayOfWeek - 1] += (int)i.Price;
                        ViewBag.RemoveAds += i.Price;
                    }

                    ViewBag.valMonthPurchase = JsonConvert.SerializeObject(ViewBag.valMonthPurchase);
                    ViewBag.valMonthAds = JsonConvert.SerializeObject(ViewBag.valMonthAds);
                    ViewBag.valDayOfWeekPurchase = JsonConvert.SerializeObject(ViewBag.valDayOfWeekPurchase);
                    ViewBag.valDayOfWeekAds = JsonConvert.SerializeObject(ViewBag.valDayOfWeekAds);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not display Revenue page because: {0}", ex.ToString());
            }
            return View("Index", model);
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return new NotFoundResult();
            }

            return PartialView("Partials/_PopupDelete", id);
        }
    }
}
