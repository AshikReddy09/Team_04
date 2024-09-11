namespace Sprint_sol1.Models
{
    public class BlobViewModel
    {
        public List<BlobDto> Blobs { get; set; } = new List<BlobDto>();
    }

    public class BlobDto
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public string ContentType { get; set; }
    }
}
