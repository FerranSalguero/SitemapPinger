using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebsitePinger.Models
{
    public class PingerWebClient : HttpClient
    {
        public string UserAgent { get; set; }

        public PingerWebClient(string userAgent)
        {
            this.UserAgent = userAgent;
            this.Timeout = TimeSpan.FromSeconds(30);
        }

    }
}
