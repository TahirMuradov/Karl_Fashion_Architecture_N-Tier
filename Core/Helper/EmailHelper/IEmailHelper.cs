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
      public  Task<IResult> SendEmailAsync(string userEmail, string confirmationLink, string UserName);
    
        public Task<IResult> SendEmailPdfAsync(string userEmail, string UserName, string pdfLink);
    }
}
