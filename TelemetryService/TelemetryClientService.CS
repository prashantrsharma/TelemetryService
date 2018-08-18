using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ApplicationInsights;

namespace TelemetryService
{
    /// <summary>
    /// Telemetry Client Service
    /// </summary>
    public class TelemetryClientService
    {
        /// <summary>
        /// GetTelemetryClient
        /// </summary>
        /// <returns> TelemetryClient</returns>
        public static TelemetryClient GetTelemetryClient()
        {
            var telemetryClient = new TelemetryClient();
            telemetryClient.InstrumentationKey =
                ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"].ToString();
            return telemetryClient;
        }

        /// <summary>
        /// LogDependency<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="dependecyTypeName"></param>
        /// <param name="dependencyName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async static Task<T> LogDependency<T>(Func<Task<T>> action, TelemetryClient appInsights,
            string dependecyTypeName, string dependencyName, string data)
        {
            var success = false;
            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var tsk = await Task.Run(() => action()).ConfigureAwait(true);
                success = true;
                return tsk;
            }
            finally
            {
                timer.Stop();
                appInsights.TrackDependency(dependecyTypeName, dependencyName, data, startTime, timer.Elapsed, success);
            }

        }

    }
}