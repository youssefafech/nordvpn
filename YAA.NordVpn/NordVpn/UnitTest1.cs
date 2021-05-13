#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NordAPI;
using NordAPI.Model;
using NUnit.Framework;

namespace NordVpn
{
    public class Tests
    {
        private NordAPI.NordApi _nordApi;
        
        [SetUp]
        public void Setup()
        {
            _nordApi = new NordApi("/home/yaa/RiderProjects/YAA.NordVpn/NordAPI/NordAPI1.xml");
        }

        [Test]
        public void Test1()
        {
            Task<IEnumerable<Server>?> itemsTask = _nordApi.Servers();
            IEnumerable<Server>? items = itemsTask.Result;
            
            if (items == null || !items.Any())
                Assert.Fail();
            
            Assert.Pass();
        }
    }
}