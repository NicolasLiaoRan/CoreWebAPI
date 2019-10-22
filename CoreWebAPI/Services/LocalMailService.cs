using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Services
{
    public class LocalMailService:IMailService
    {
        private string _mailTo = Startup._configuration["mailSettings:mailToAddress"];
        private string _mailFrom = Startup._configuration["mailSettings:mailFromAddress"];
        public void Send(string subject,string msg)
        {
            Debug.WriteLine($"从{_mailFrom}给{_mailTo}通过{nameof(LocalMailService)}发送了邮件");
        }
    }
}
