using Kavenegar.DataTransitLibrary.Common.Interfaces;
using Kavenegar.DataTransitLibrary.Common.Options;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Common.Redis
{
    public class RedisService : IRedisServices
    {
        private readonly IOptions<RedisConfiguration> _redisConfiguration;

        public RedisService(IOptions<RedisConfiguration> redisConfiguration)
        {
            _redisConfiguration = redisConfiguration;
        }

        public List<RedisKey> GetAllRedisKey()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(_redisConfiguration.Value.Connection);
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);

            EndPoint endPoint = connection.GetEndPoints().First();

            return connection.GetServer(endPoint).Keys(pattern: "*").ToList();
        }

        public List<RedisKey> GetAllRedisKeyContain(string code)
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(_redisConfiguration.Value.Connection);
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);

            EndPoint endPoint = connection.GetEndPoints().First();

            return connection.GetServer(endPoint).Keys(pattern: code).ToList();
        }
    }
}
