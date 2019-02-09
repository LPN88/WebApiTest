using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiTest.Controllers;

namespace WebApiTest.Tests
{
    [TestClass]
    public class ValuesControllerTest
    {

        [TestMethod]
        public void Post()
        {
            // Arrange
            ValuesController controller = new ValuesController(new MetricsR3());
            // Act
            var watch = Stopwatch.StartNew();
            Task<HttpResponseMessage> respTask = null;           
            respTask = controller.Search("value");                      
            respTask.Wait();
            watch.Stop();
            if (watch.ElapsedMilliseconds < 11000)
            {
                Assert.IsTrue(respTask.Result.StatusCode == System.Net.HttpStatusCode.OK || respTask.Result.StatusCode == System.Net.HttpStatusCode.InternalServerError);
            }
            else
            {
                Assert.AreEqual(respTask.Result.StatusCode, System.Net.HttpStatusCode.RequestTimeout);
            }

            // Assert
        }

        //[TestMethod]
        //public void Put()
        //{
        //    // Arrange
        //    ValuesController controller = new ValuesController();

        //    // Act
        //    //controller.Put(5, "value");

        //    // Assert
        //}

        //[TestMethod]
        //public void Delete()
        //{
        //    // Arrange
        //    ValuesController controller = new ValuesController();

        //    // Act
        //    //controller.Delete(5);

        //    // Assert
        //}
    }
}
