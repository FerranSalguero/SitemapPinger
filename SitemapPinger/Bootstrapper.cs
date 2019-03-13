using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePinger
{
    public class Bootstrapper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static int Main(string[] args)
        { 

            TelemetryConfiguration config = TelemetryConfiguration.Active;
            var client = new TelemetryClient();
            client.TrackTrace($"Hello from {Environment.MachineName}");

            try
            {
                new SitemapPinger();
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
