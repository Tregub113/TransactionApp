using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TransactionApp.Core.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using OfficeOpenXml;
using TransactionApp.Data.Repositories;
using TransactionApp.Core;
using TransactionApp.Domain.Objects;
using TransactionApp.Domain.Models;
using System.Reflection;
using TransactionApp.Models;

namespace TransactionApp.Controllers
{
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private readonly IUploadService uploadService;
        private readonly IReportService reportService;

        private const int PageSize = 10;        

        public TransactionController(IUploadService uploadService, IReportService reportService)
        {
            this.reportService = reportService;
            this.uploadService = uploadService;
        }
        
        [HttpPost]
        [Route("Upload")]
        public RedirectResult Upload(IFormFile file)
        {
            if (file == null)
                return Redirect("/Home/Index");

            var stream = file.OpenReadStream();
            uploadService.Upload(stream);
            return Redirect("/Transaction/Report");
            
            
        }
        [HttpGet]
        [Route("Report")]
        public ActionResult Report(int page)
        {
            
            var skip = page==0?0:((page-1) * PageSize);            
            var result = reportService.GetAccountReport(skip, PageSize);
            var pageInfo = new PageInfo(result.TotalCount, page, PageSize);
            var viewModel = new IndexViewReportModel
            {
                AccountsReport = result.AccountReportList,
                PageInfo = pageInfo
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("test")]
        public string test()
        {
            return "testMethod";
        }

    }
}
