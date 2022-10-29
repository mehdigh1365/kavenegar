using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Common.Interfaces
{
    public interface IRedisServices
    {
        List<RedisKey> GetAllRedisKey();

        List<RedisKey> GetAllRedisKeyContain(string code);
    }
}
