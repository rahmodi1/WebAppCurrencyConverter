using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp_CurrencyConverter.Controllers
{
    public class ConverterController : Controller
    {
        // GET: Converter
        public ActionResult Index()
        {
            return View();
        }
    }
}