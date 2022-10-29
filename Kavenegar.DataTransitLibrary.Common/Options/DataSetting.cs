using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Common.Options
{
    public class DataSetting
    {
        public bool AutoMigration { get; set; }
        public bool SeedData { get; set; }
        /// <summary>
        /// timeout in seconds
        /// </summary>
        public int MigrationCommandTimout { get; set; }

    }
}
