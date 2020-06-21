using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Models
{
    public class AjaxResult
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
