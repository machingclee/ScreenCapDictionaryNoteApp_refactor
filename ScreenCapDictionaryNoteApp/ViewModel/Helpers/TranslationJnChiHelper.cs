using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class TranslationJnChiHelper
    {
        public static async Task<string> FetchTranslation(string param)
        {
            string baseUrl = "http://dict.hjenglish.com/jp/jc/";
            string path = baseUrl + HttpUtility.UrlEncode(param).ToUpper();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-HK, en; q=0.8, zh-Hant-HK; q=0.6, zh-Hant; q=0.4, ja; q=0.2");
                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                client.DefaultRequestHeaders.Add("Cookie", "HJ_CST=0; HJ_SSID_3=19eba47b-abab-a4a7-7ee8-4992058e2278; _SREF_3=; _REF=; TRACKSITEMAP=3%2C; HJ_UID=965ef15e-ba78-bfbd-6508-89939eba74e6; _SREG_3=direct|; HJ_SID=55229b4f-4b61-97c6-1378-c1adc8b9ad47; HJ_CSST_3=0; _REG=direct|; acw_tc=76b20f6315763374709907536e26c0eb158490c898cbb79a8510e3697a454f");
                client.DefaultRequestHeaders.Add("Host", "dict.hjenglish.com");
                client.DefaultRequestHeaders.Add("If-None-Match", "\"4cc1-ID5QapBmT9r04YizdaAssMugWwM\"");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");


                var response = await client.GetAsync(path);

                string responseBody = await response.Content.ReadAsStringAsync();

                string truncatedResult = Regex.Replace(responseBody, "\n|\r\n|\t", "");
                truncatedResult = Regex.Match(truncatedResult, "<div class=\"word-details-pane\".* (?=</section>.*?<div class=\"word-details-ads\">)").Value;
                truncatedResult = Regex.Replace(truncatedResult, "<footer.*</footer>", "");
                truncatedResult = Regex.Replace(truncatedResult, @"\s+", " ");
                truncatedResult = Regex.Replace(truncatedResult, "<dt>|<dd>|</dt>|</dd>", "");

                return truncatedResult;
            }


        }
    }
}

