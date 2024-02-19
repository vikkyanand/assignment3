using MongoDB.Bson;

namespace assignment_3.Models
{
    public class UploadedData
    {
        public ObjectId Id { get; set; }
        public string Text { get; set; }
        public byte[] ImageData { get; set; } // Add this property for ImageData
        public string ImageMimeType { get; set; } // Add this property for ImageMimeType
        public ObjectId ImageId { get; set; } // Reference to the image stored in GridFS
    }
}
