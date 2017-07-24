using System;
using VideoManager.Enums;

namespace VideoManager.Model
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
