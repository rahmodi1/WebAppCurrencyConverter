using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using WebApp_CurrencyConverter;

namespace OBNMArceloAdmin.Models
{
    public class CurrencyInfo
    {
        public string SourceCurrency { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public string returncode { get; set; }
        public string err { get; set; }
        public long? timestamp { get; set; }

    }

    public class CurrencyInput
    {
        public CurrencyInput()
        {
            // Set Default value
            SourceCurrency = "USD";
            Amount = 1;
        }

        public string SourceCurrency { get; set; }
        public decimal Amount { get; set; }



        public static DateTime? CheckCurrency(string currency, decimal conversionRate)
        {

            using (DbRateListEntities db = new DbRateListEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        CurrencyRate objCurrencyRate = db.CurrencyRates.Where(x => x.Currency.ToLower() == currency).FirstOrDefault();
                        if (objCurrencyRate == null)
                        {

                            CurrencyRate objNewCurrencyRate = new CurrencyRate();
                            objNewCurrencyRate.Currency = currency;
                            objNewCurrencyRate.Rate = conversionRate;
                            objNewCurrencyRate.CreatedDate = DateTime.Now;


                            db.Entry(objNewCurrencyRate).State = EntityState.Added;
                            db.CurrencyRates.Add(objNewCurrencyRate);
                            db.SaveChanges();

                            dbContextTransaction.Commit();
                            return objNewCurrencyRate.CreatedDate;
                        }

                        return objCurrencyRate.CreatedDate;
                    }

                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return null;
                    }
                }
            }

        }
    }
}