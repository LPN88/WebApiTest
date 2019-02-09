using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiTest
{
    public class MetricsR3 : IMetricsR3
    {
        static readonly ConcurrentDictionary<int, int> _metrics = new ConcurrentDictionary<int, int>();

        public ConcurrentDictionary<int, int> GetMetricsR3()
        {
            return _metrics;
        }
    }
}