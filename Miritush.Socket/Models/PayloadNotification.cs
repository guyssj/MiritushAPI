namespace Miritush.Socket.Models
{
    public class PayloadNotification
    {
        public string MethodName { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public int ObjectId { get; set; }
        public string ObjectName { get; set; } = string.Empty;
    }
}