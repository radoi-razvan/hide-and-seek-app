namespace HideAndSeek.DTOs
{
    public class CryptographyDTO
    {
        public IFormFile File { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string Operation { get; set; } = null!;
        public bool Crc { get; set; }
    }
}
