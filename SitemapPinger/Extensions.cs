using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsitePinger
{
    public static class Extensions
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static Task LogExceptions(this Task task)
        {

            task.ContinueWith(t =>
            {
                var aggException = t.Exception.Flatten();
                foreach (var exception in aggException.InnerExceptions)
                    logger.Error(exception, "Error on unatended task");
            },
            TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }
    }
}
