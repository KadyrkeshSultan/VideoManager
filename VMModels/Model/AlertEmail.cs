using System;
using VMModels.Enums;

namespace VMModels.Model
{
    public class AlertEmail
    {
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
        public EmailType EmailType { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MailList { get; set; }
    }
}
