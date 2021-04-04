using LocalaizationGlobalazation.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LocalaizationGlobalazation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHtmlLocalizer<HomeController> _localizer;
        private readonly IOptions<RequestLocalizationOptions> locOptions;
        public HomeController(ILogger<HomeController> logger, IHtmlLocalizer<HomeController> localizer, IOptions<RequestLocalizationOptions> locOptions)
        {
            _logger = logger;
            _localizer = localizer;
            this.locOptions = locOptions;
        }

        public IActionResult Index()
        {
            var titel = _localizer["titel"];
            ViewData["titel"] = titel ;
            return View();
        }

        public IActionResult CultureManagement(string culture,string returnUrl)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),new CookieOptions(){ Expires=DateTimeOffset.Now.AddDays(30)});

            return LocalRedirect(returnUrl);
        }
        public IActionResult Privacy()
        {          
            List<textbyculture> tbc = new List<textbyculture>()
            {
                new textbyculture("en","this english text of privcy."),
                new textbyculture("fa","این متن فارسی حریم شخصی است."),
                new textbyculture("ar","العربی متون اف پرایوسی.")
            };
            var culture = HttpContext.Features.Get<Microsoft.AspNetCore.Localization.IRequestCultureFeature>();
            var cultureList = locOptions.Value.SupportedUICultures.Select(x => new SelectListItem()
            {
                Value = x.Name,
                Text = _localizer.GetString(x.Name)
            }).ToList();
            //var returnUrl = string.IsNullOrWhiteSpace(HttpContext.Request.Path) ? "~/" : $"~{HttpContext.Request.Path}{HttpContext.Request.QueryString}";

            string sText = tbc.FirstOrDefault(a => a.key == culture.RequestCulture.Culture.Name).val;
            ViewData["text"] = sText;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    class textbyculture
    {
        public string key { get; set; }

        public textbyculture(string key, string val)
        {
            this.key = key;
            this.val = val;
        }

        public string val { get; set; }
    }
}
