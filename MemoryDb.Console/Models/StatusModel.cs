using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MemoryDb.Console.Models.Enums;

namespace MemoryDb.Console.Models
{
    public class StatusModel
    {
        public StatusEnum Status { get; set; } = StatusEnum.Success;
        public string Message { get; set; } = string.Empty;
        public object Value { get; set; }
    }
}
