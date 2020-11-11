using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

using StackExchange.Redis;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace sample_redis_cluster_for_dotnetcore
{
    public class Function
    {
        private readonly string redisEndpoint;

        public Function()
        {
            redisEndpoint = Environment.GetEnvironmentVariable("REDIS_CONF_ENDPOINT");
        }

        public string FunctionHandler(string input, ILambdaContext context)
        {
            Console.WriteLine("Start");

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisEndpoint);

            IDatabase db = redis.GetDatabase();

            Stopwatch sw = new Stopwatch();

            sw.Start();
            setData(db);
            sw.Stop();

            Console.WriteLine($"Write Time : {sw.ElapsedMilliseconds} ms");

            sw.Restart();
            getData(db);
            sw.Stop();

            Console.WriteLine($"Read Time : {sw.ElapsedMilliseconds} ms");

            return $"{sw.ElapsedMilliseconds}";
        }

        private void setData(IDatabase db)
        {
            var range = Enumerable.Range(1, 10000);

            foreach (var index in range)
            {
                string random = Guid.NewGuid().ToString("N").Substring(0, 10);
                // db.StringSetAsync($"{index}", random);

                db.StringSet($"{index}", random);
                //Console.WriteLine("Write Endpoint : " + db.IdentifyEndpoint());
            }

            // db.WaitAll();
        }

        private void getData(IDatabase db)
        {
            var range = Enumerable.Range(1, 10000);
            foreach (var index in range)
            {
                db.StringGetAsync($"{index}", CommandFlags.PreferReplica);
            }
            db.WaitAll();
        }
    }
}
