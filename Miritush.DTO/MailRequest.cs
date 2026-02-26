using System;
using System.Collections.Generic;

namespace Miritush.DTO
{
    public class MailRequest
    {
        public List<string> ToEmails { get; set; } = new();
        public List<string>? CcEmails { get; set; }
        public List<string>? BccEmails { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public bool IsHtml { get; set; } = true;
        public List<MailAttachment>? Attachments { get; set; }
    }
    public class MailAttachment
    {
        public string FileName { get; set; } = string.Empty;
        public byte[] Content { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "application/octet-stream";
    }
}