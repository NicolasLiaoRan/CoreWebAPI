using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Services
{
    public class CloudMailService
    {
        private string _mailTo = "developer@qq.com";
        private string _mailFrom = "noreply@alibaba.com";
        public void Send(string subject, string msg)
        {
            Debug.WriteLine($"从{_mailFrom}给{_mailTo}通过{nameof(LocalMailService)}发送了邮件");
        }
    }
}
