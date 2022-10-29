using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Common.Options
{
    public interface IRedisConfiguration
    {
        string Connection { get; set; }

        string InstanceName { get; set; }
    }
}
