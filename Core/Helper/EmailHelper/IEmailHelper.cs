using Core.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper.EmailHelper
{
    public interface IEmailHelper
    {
        Task<IResult> SendEmailAsync(string userEmail, string confirmationLink, string UserName);
    }
}
