using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiTest
{
    public interface IMetricsR3
    {
        ConcurrentDictionary<int, int> GetMetricsR3();
    }
}