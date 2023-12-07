using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results.Concrete
{
    public class Result : IResult
    {
        public bool IsSuccess { get; }

        public string Message { get; }
        public Result(bool IsSuccess)
        {
            this.IsSuccess = IsSuccess;
                }
        public Result(bool IsSuccess, string message) : this(IsSuccess)
        {
            Message = message;
        }
   
    }
}
