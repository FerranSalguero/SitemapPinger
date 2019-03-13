using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using WebsitePinger.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebsitePinger
{
    public class SitemapPinger
    {
        public SitemapPinger()
        {
            string[] urls = new string[] {
                "https://whereshouldibuy.apphb.com/sitemap/index",
                "https://whereshouldibuyineurope.apphb.com/sitemap/index"
            };

            Console.WriteLine("starting at " + DateTime.UtcNow.ToShortTimeString());


            //client.Headers["user-agent"] = "Mozilla/5.0 (compatible; Pingerbot/0.1)";
            //client.Proxy = new WebProxy("http://10.49.1.1:8080");

            var list = new List<Task>();
            foreach (var url in urls)
            {
                list.Add(DoPing(url));
            }

            Task.WaitAll(list.ToArray());

            Console.WriteLine("ended at " + DateTime.UtcNow.ToShortTimeString());

        }

        private async Task DoPing(string url)
        {
            using (var client = new PingerWebClient("Mozilla/5.0 (compatible; Pingerbot/0.2)"))
            {
                client.Timeout = TimeSpan.FromSeconds(30);

                SitemapIndex index = null;
                using (var reader = await client.GetAsync(url))
                {
                    var s = new XmlSerializer(typeof(SitemapIndex));

                    index = (SitemapIndex)s.Deserialize(await reader.Content.ReadAsStreamAsync());
                }

                Console.WriteLine("sitemaps to ping -> " + index.Sitemaps.Count);

                foreach (var sitemap in index.Sitemaps)
                {
                    UrlSet urlSet = null;
                    using (var reader = await client.GetAsync(sitemap.loc))
                    {
                        var s = new XmlSerializer(typeof(UrlSet));
                        urlSet = (UrlSet)s.Deserialize(await reader.Content.ReadAsStreamAsync());
                    }

                    Console.WriteLine("Urls to ping -> " + urlSet.Urls.Count);

                    foreach (var urlToPing in urlSet.Urls)
                    {
                        Console.WriteLine("Pinging " + urlToPing.loc);
                        try
                        {
                            await client.GetAsync(new Uri(urlToPing.loc));
                        }
                        catch (Exception exc)
                        {
                            //log to logentries...
                            throw new Exception("Error downloading page: " + urlToPing.loc, exc);
                        }
                        Thread.Sleep(30 * 1000);
                    }
                }
            }

        }
    }
}

