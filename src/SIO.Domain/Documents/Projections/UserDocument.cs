using MessagePack;

namespace SIO.Domain.Documents.Projections
{
    public class UserDocument
    {
        public string DocumentId { get; set; }
        public string FileName { get; set; }     
    }
}
