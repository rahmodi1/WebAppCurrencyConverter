using Newtonsoft.Json;
using OBNMArceloAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace WebApp_CurrencyConverter.Controllers
{
    public class v0Controller : System.Web.Http.ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }


        [HttpPost]
        public HttpResponseMessage getInrRate(CurrencyInput objCurrencyInput)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            string strJson = null;
            try
            {
                WebClient web = new WebClient();

                string url = string.Format("https://www.google.com/finance/converter?a={0}&from={1}&to={2}", objCurrencyInput.Amount, objCurrencyInput.SourceCurrency, "INR");

                string sPrice = string.Empty;
                string responseMain = web.DownloadString(url);

                sPrice = responseMain.Substring(responseMain.IndexOf("class=bld>"), (responseMain.LastIndexOf("</span>") - responseMain.IndexOf("class=bld>"))).Replace("class=bld>", "");
                sPrice = Convert.ToString(Convert.ToDecimal(Regex.Replace(sPrice, "[^0-9.]", "")));

                decimal rate = decimal.Round(Convert.ToDecimal(sPrice), 2, MidpointRounding.AwayFromZero);

                CurrencyInfo objCurrencyInfo = new CurrencyInfo();
                objCurrencyInfo.SourceCurrency = objCurrencyInput.SourceCurrency;
                objCurrencyInfo.ConversionRate = decimal.Round(rate / objCurrencyInput.Amount, 2, MidpointRounding.AwayFromZero);
                objCurrencyInfo.Amount = decimal.Round(objCurrencyInput.Amount, 2, MidpointRounding.AwayFromZero);
                objCurrencyInfo.Total = rate;
                objCurrencyInfo.returncode = "1";
                objCurrencyInfo.err = "success";

                //call for insert in database
                DateTime date = Convert.ToDateTime(CurrencyInput.CheckCurrency(objCurrencyInput.SourceCurrency, objCurrencyInfo.ConversionRate));
                objCurrencyInfo.timestamp = TimeSpan.FromTicks(date.ToUniversalTime().Ticks).Ticks;



                strJson = JsonConvert.SerializeObject(objCurrencyInfo);
                response.Content = new StringContent(strJson, Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception ex)
            {
                CurrencyInfo objCurrencyInfo = new CurrencyInfo();
                objCurrencyInfo.returncode = "0";
                objCurrencyInfo.err = ex.Message;

                strJson = JsonConvert.SerializeObject(objCurrencyInfo);
                response.Content = new StringContent(strJson, Encoding.UTF8, "application/json");
                return response;
            }
        }
    }
}