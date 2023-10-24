using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Dtos.message
{
    public class BaseMessage<T> where T : class
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T ReturnEntity { get; set; }


    }
}