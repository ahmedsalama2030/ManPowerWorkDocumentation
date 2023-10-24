using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
 using Infrastructure.Helper.Translate;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
namespace Infrastructure.Helper.ExtentionMethod;
    public static class CommonExtenion
    {
        public static string GetDateArabic(this DateTime date)
        {

            return date.ToString("yyyy/MM/dd", CultureInfo.GetCultureInfo("ar-EG")).GetNumberArabic();
        }
        public static string GetDateTimeArabic(this DateTime date)
        {

            return date.ToString("yyyy/MM/dd HH:mm", CultureInfo.GetCultureInfo("ar-EG")).GetNumberArabic();
        }
        public static string GetDateArabic(this DateTime? date)
        {
            if (date == null)
                return "";
            else
                return date?.ToString("yyyy/MM/dd", CultureInfo.GetCultureInfo("ar-EG")).GetNumberArabic();
        }
        public static int getTotalDay(this DateTime form, DateTime to)
        {
            var dateFrom = new DateTime(form.Year, form.Month, form.Day, 0, 0, 0, 0);
            var dateTo = new DateTime(to.Year, to.Month, to.Day, 23, 59, 59, 9);
            var diff = (dateTo - dateFrom).TotalDays;
            var result = (int)Math.Ceiling(diff);
            return result;
        }
        public static string GetNumberArabic(this string valueAsString)

        {
            return valueAsString.Replace("0", "٠")
              .Replace("1", "١")
              .Replace("2", "٢")
              .Replace("3", "٣")
              .Replace("4", "٤")
              .Replace("5", "٥")
              .Replace("6", "٦")
              .Replace("7", "٧")
              .Replace("8", "٨")
              .Replace("9", "٩");

        }
        public static string GetDayArabic(this byte day)

        {

            var result = "";
            switch (day)
            {
                case 1: result = "الأثنين"; break;
                case 2: result = "الثلاثاء"; break;
                case 3: result = "الاربعاء"; break;
                case 4: result = "الخميس"; break;
                case 5: result = "الجمعة"; break;
                case 6: result = "السبت"; break;
                case 0: result = "الأحد"; break;
            }
            return result;

        }
        public static string ArabicNumberWord(this decimal num)
        {
            List<CurrencyInfo> currencies = new List<CurrencyInfo>();
            currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.EGP));
            try
            {
                var decimalNumber = num;
                var currency = currencies[Convert.ToInt32(0)];
                ToWord toWord = new ToWord(decimalNumber, currency);

                return toWord.ConvertToArabic();

            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    
   public static string GetSerilizeObject(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
        
    }