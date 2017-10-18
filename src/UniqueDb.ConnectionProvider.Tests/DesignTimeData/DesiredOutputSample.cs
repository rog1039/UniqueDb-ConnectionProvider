using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UniqueDb.ConnectionProvider.Tests.DesignTimeData
{
    public class OrderHeaderDesignTimeData
    {
        private const string OrderHeaderDataCustomJson = @"";

        public static OrderHeader Get1()
        {
            return Get().Take(1).First();
        }
        
        public static IList<OrderHeader> Get()
        {
            var normalJson = OrderHeaderDataCustomJson.Replace("`~`", "/");
            var result = JsonConvert.DeserializeObject<List<OrderHeader>>(normalJson);
            return result;
        }
    }

    public class OrderHeader
    {
        public int OrderNumber { get; set; }
    }
}
