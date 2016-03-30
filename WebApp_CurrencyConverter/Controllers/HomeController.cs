using Newtonsoft.Json;
using OBNMArceloAdmin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApp_CurrencyConverter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [OutputCache(Duration = 10, VaryByParam = "CurrencyInput.SourceCurrency")]
        public JsonResult CurrencyConvertAction(CurrencyInput objCurrencyInput)
        {
            try
            {
                var data = JsonConvert.SerializeObject(objCurrencyInput);
                var request = (HttpWebRequest)WebRequest.Create("http://appcurrencyconverter.azurewebsites.net/api/v0/getInrRate");
                request.Method = "POST";
                request.ContentType = "application/json";
                Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream os = request.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();
                var response = (HttpWebResponse)request.GetResponse();
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                var content = sr.ReadToEnd();

                return Json(new { success = true, content = content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {

                return Json(new { success = false, content = Ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}