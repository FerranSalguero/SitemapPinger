using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsitePinger
{
    public class Bootstrapper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static async Task<int> Main(string[] args)
        { 

            TelemetryConfiguration config = TelemetryConfiguration.Active;
            var client = new TelemetryClient();
            client.TrackTrace($"Hello from {Environment.MachineName}");

            try
            {
                await new SitemapPinger().Run();
            }
            catch (Exception exc)
            {
                logger.Error(exc, "Error general");
            }

            return 1; // return code != 0 to re-execute

            //Console.ReadKey();


        }
    }
}
