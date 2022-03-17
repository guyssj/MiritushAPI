namespace Miritush.DAL.Model
{
    public partial class Attachment
    {
        public int AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public string MimeType { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

    }
}