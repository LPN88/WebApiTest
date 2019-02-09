using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiTest.Controllers
{
    public class ValuesController : ApiController
    {
        IMetricsR3 _metricsR3 = null;

        public ValuesController(IMetricsR3 metrics)
        {
            _metricsR3 = metrics;
        }

        [NonAction]
        public HttpResponseMessage GetRandomResponse()
        {
            var rnd = new Random();
            if (rnd.Next(0, 99) % 2 == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [NonAction]
        public async Task<HttpResponseMessage> ExecuteAllReqsAsync()
        {
            var task1 = Task.Run<HttpResponseMessage>(() => Request1());
            var task2 = Task.Run<HttpResponseMessage>(() => Request2());
            var task3 = Task.Run<HttpResponseMessage>(() => Request3());
            var task4 = Task.Run<HttpResponseMessage[]>(() =>
            {
                var array = new HttpResponseMessage[2];
                var resp4 = Request4();
                if (resp4.StatusCode == HttpStatusCode.OK)
                {
                    array[1] = Request5();
                }
                array[0] = resp4;
                return array;
            });
            var str = new StringBuilder();
            await Task.WhenAll(task1, task2, task3, task4);
            str.Append($"Результат запроса №1: {task1.Result.StatusCode.ToString()}\t Результат запроса №2: {task2.Result.StatusCode.ToString()}\t " +
                $"Результат запроса №3: {task3.Result.StatusCode.ToString()}\t " +
                $"Результат запроса №4: {task4.Result[0].StatusCode.ToString()}\t" +
                (task4.Result[1] == null ? "Запрос 5 не выполнялся" : $"Результат запроса №5: { task4.Result[1].StatusCode.ToString()}"));
            Trace.WriteLine(str.ToString());
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(task4.Result[0].StatusCode == HttpStatusCode.OK ? "Вся цепочка методов выполнилась" : "Выполнились только 4 метода");
            return resp;
            //return Request.CreateResponse(HttpStatusCode.OK, task4.Result[0].StatusCode == HttpStatusCode.OK ? "Вся цепочка методов выполнилась" : "Выполнились только 4 метода");            
        }

        [HttpGet]
        public string GetTest(string id)
        {
            return "test";
        }
        // GET api/values
        [HttpGet]
        public HttpResponseMessage Request1()
        {
            var task = Task.Run(() => Thread.Sleep(10000));
            var resp = GetRandomResponse();
            task.Wait();
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage Request2()
        {
            var task = Task.Run(() => Thread.Sleep(7000));
            var resp = GetRandomResponse();
            task.Wait();
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage Request3()
        {
            var task = Task.Run(() =>
            {
                var dict = _metricsR3.GetMetricsR3();//(ConcurrentDictionary<int, int>)_httpContext.Application["MetricsR3"];
                var rnd = new Random();
                int val = rnd.Next(1000, 20000);
                dict.AddOrUpdate(val, 1, (k, v) => v + 1);
                Thread.Sleep(val);
            });
            var resp = GetRandomResponse();
            task.Wait();
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage Request4()
        {
            var task = Task.Run(() => Thread.Sleep(3000));
            var resp = GetRandomResponse();
            task.Wait();
            return resp;
        }

        [HttpGet]
        public HttpResponseMessage Request5()
        {
            var task = Task.Run(() => Thread.Sleep(6000));
            var resp = GetRandomResponse();
            task.Wait();
            return resp;
        }

        // POST api/values
        [HttpPost]
        //[System.Web.Mvc.AsyncTimeout(10)]      
        public async Task<HttpResponseMessage> Search([FromBody]string value)
        {
            HttpResponseMessage resp = null;
            var ee = Request;
            //var cts = new CancellationTokenSource();
            //cts.CancelAfter(11000);            
            try
            {
                var delayTask = Task.Delay(11000);
                var respTask = ExecuteAllReqsAsync();
                var firstTaskFinished = await Task.WhenAny(respTask, delayTask);
                if (firstTaskFinished == delayTask)
                {
                    throw new OperationCanceledException("Timeout");
                }
                else
                {
                    resp = respTask.Result;
                }
            }
            catch (OperationCanceledException ex)
            {
                resp = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
                resp.Content = new StringContent("Произошел таймаут запроса (11000 мс)");
                //this.Request.CreateResponse(HttpStatusCode.RequestTimeout, "Произошел таймаут запроса (11000 мс)");                
            }
            catch (Exception ex)
            {
                resp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                resp.Content = new StringContent("Произошла ошибка во время выполнения запроса");
                //resp = this.Request.CreateResponse(HttpStatusCode.InternalServerError, "Произошла ошибка во время выполнения запроса");
            }
            return resp;
        }

        // PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
