using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace webScraper
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            GetHtmlAsync();
            Console.ReadLine();
        }

        private static async void GetHtmlAsync()
        {

            var url = "https://www.amazon.com/s?k=doom+patrol+omnibus&crid=2HBTXIE5Z803G&sprefix=Doom+pat%2Caps%2C176&ref=nb_sb_ss_i_4_8";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);


            // Gets List
            var ProductsHtml = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("s-main-slot s-result-list s-search-results sg-row")).ToList();

            var ProductListItems = ProductsHtml[0].Descendants("div")
                .Where(node => node.GetAttributeValue("data-asin", "") //data-asin
                .Contains("1")).ToList(); //1

            //Console.WriteLine(ProductListItems.Count());
            //Console.WriteLine();


            foreach (var ProductListItem in ProductListItems)
            {

                // id
                Console.WriteLine(ProductListItem.GetAttributeValue("data-index", ""));

                // ProductName
                Console.WriteLine(ProductListItem.Descendants("a")
                  .Where(node => node.GetAttributeValue("class", "")
                  .Equals("a-link-normal a-text-normal")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                  );

                //// Subtitle
                Console.WriteLine(ProductListItem.Descendants("span")
                  .Where(node => node.GetAttributeValue("class", "")
                  .Contains("a-")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                  );

                // Price
                Console.WriteLine(
                    Regex.Match(
                    ProductListItem.Descendants("a")
                   .Where(node => node.GetAttributeValue("class", "")
                   .Equals("a-size-base a-link-normal a-text-normal")).FirstOrDefault().InnerText
                   , @"\$\d+.\d+")
                  );

                // Book Type
                Console.WriteLine(
                    ProductListItem.Descendants("a")
                  .Where(node => node.GetAttributeValue("class", "")
                  .Equals("a-size-base a-link-normal a-text-bold")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t')
                  );


                //// Url
                //Console.WriteLine(
                //    ProductListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "").Trim('\r', '\n', '\t')
                //    );

                //Console.WriteLine();

            }
        }
    }

}




//*[@id="productTitle"]

//<span class="a-size-medium a-color-price offer-price a-text-normal">$94.33</span>


//HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
//HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.amazon.com/s?k=doom+patrol+omnibus&crid=2HBTXIE5Z803G&sprefix=Doom+pat%2Caps%2C176&ref=nb_sb_ss_i_4_8");
//            foreach(var item in doc.DocumentNode.SelectNodes("//*[@id='productTitle']")) 
//            {
//                Console.WriteLine(item.InnerText);
//            }

//            foreach (var item in doc.DocumentNode.SelectNodes("//*[@class='pa-size-medium a-color-price offer-price a-text-normal']"))
//            {
//                Console.WriteLine(item.InnerText);
//            }