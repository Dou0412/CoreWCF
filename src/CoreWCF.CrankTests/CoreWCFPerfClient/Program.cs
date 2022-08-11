﻿using Microsoft.Crank.EventSources;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CoreWCFPerf
{
    public class Parameters
    {
        public const string Binding = "binding";
        public const string ServiceUrl = "serviceurl";
        public const string ReportingUrl = "reportingurl";
        public const string PerfMeasurementDuration = "perfmeasurementduration";
        public const string Requests = "requests";
        public const string CloseAndOpen = "closeandopen";
    }

    public enum TestBinding { BasicHttp, NetTcp }

    class Program
    {
        private TestBinding _paramBinding = TestBinding.BasicHttp;
        private TimeSpan _paramPerfMeasurementDuration = s_defaultPerfMeasurementDuration;
        private string _paramServiceUrl = "";
        private int _paramRequests = 0;
        private bool _paramCloseAndOpen = false;

        private readonly static TimeSpan s_defaultPerfMeasurementDuration = TimeSpan.FromSeconds(10);

        static void Main(string[] args)
        {

            Program test = new Program();

            if (test.ProcessRunOptions(args))
            {
                var startTime = DateTime.Now;
                int requestTime = 0;
                int request = 0;

                Binding binding = null;
                switch (test._paramBinding)
                {
                    case TestBinding.BasicHttp:
                        binding = new BasicHttpBinding();
                        break;
                    case TestBinding.NetTcp:
                        binding = new NetTcpBinding(SecurityMode.None);
                        break;
                    default:
                        break;
                }

                TimeSpan measurementDurationPerTime = test._paramPerfMeasurementDuration ;

                BenchmarksEventSource.Register("firstrequest", Operations.Max, Operations.Max, "First Request (ms)", "Time to first request in ms", "n0");
                BenchmarksEventSource.Register("corewcfperf/requests", Operations.Max, Operations.Sum, "Requests (" + test._paramPerfMeasurementDuration.TotalMilliseconds + " ms)", "Total number of requests", "n0");
                BenchmarksEventSource.Register("corewcfperf/rps/max", Operations.Max, Operations.Sum, "Requests/sec (max)", "Max requests per second", "n0");

                startTime = DateTime.Now;
                ChannelFactory<ISayHello> factory = new ChannelFactory<ISayHello>(binding, new EndpointAddress(test._paramServiceUrl));
                factory.Open();
                var client = factory.CreateChannel();
                ((IClientChannel)client).Open();
                var stopwatchFirstReq = new Stopwatch();
                stopwatchFirstReq.Start();
                var result = client.HelloAsync("hello world").Result;

                BenchmarksEventSource.Measure("firstrequest", stopwatchFirstReq.ElapsedMilliseconds);

                while (DateTime.Now <= startTime.Add(measurementDurationPerTime))
                {
                    var rtnResult = client.HelloAsync("hello world").Result;
                    Console.WriteLine(rtnResult);
                    request++;
                    requestTime = request;
                    if (requestTime >= test._paramRequests & test._paramCloseAndOpen)
                    {
                        try
                        {
                            ((IClientChannel)client).Close();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Client Close failed : {ex}.");
                            ((IClientChannel)client).Abort();
                        }
                        requestTime = 0;
                    }

                    if (((IClientChannel)client).State != CommunicationState.Opened)
                    {
                        client = factory.CreateChannel();
                        ((IClientChannel)client).Open();
                    }
                }
                factory.Close();
                Console.WriteLine("ChannelFactory Closed.");

                BenchmarksEventSource.Measure("corewcfperf/requests", request);
                BenchmarksEventSource.Measure("corewcfperf/rps/max", request / test._paramPerfMeasurementDuration.TotalSeconds);
            }
        }

        private bool ProcessRunOptions(string[] args)
        {
            foreach (string s in args)
            {
                Console.WriteLine(s);
                string[] p = s.Split(new char[] { ':' }, count: 2);
                if (p.Length != 2)
                {
                    continue;
                }

                switch (p[0].ToLower())
                {
                    case Parameters.Binding:
                        if (!Enum.TryParse<TestBinding>(p[1], ignoreCase: true, result: out _paramBinding))
                        {
                            return ReportWrongArgument(s);
                        }
                        break;

                    case Parameters.PerfMeasurementDuration:
                        int perfPerfMeasurementDurationSeconds = 0;
                        if (!Int32.TryParse(p[1], out perfPerfMeasurementDurationSeconds))
                        {
                            return ReportWrongArgument(s);
                        }
                        _paramPerfMeasurementDuration = TimeSpan.FromSeconds(perfPerfMeasurementDurationSeconds);
                        break;

                    case Parameters.Requests:
                        if (!Int32.TryParse(p[1], out _paramRequests))
                        {
                            return ReportWrongArgument(s);
                        }
                        break;

                    case Parameters.ServiceUrl:
                        _paramServiceUrl = p[1];
                        break;

                    case Parameters.CloseAndOpen:
                        if (!bool.TryParse(p[1], out _paramCloseAndOpen))
                        {
                            return ReportWrongArgument(s);
                        }
                        break;

                    default:
                        Console.WriteLine("unknown argument: " + s);
                        continue;
                }
            }
            return true;
        }

        private bool ReportWrongArgument(string arg)
        {
            Console.WriteLine("Wrong parameter: " + arg);
            return false;
        }
    }
}
