using HideAndSeek.DTOs;

namespace HideAndSeek.Services.Interfaces
{
    public interface ICryptographyService
    {
        Task<string> Encrypt(CryptographyDTO fileData);
        Task<string> Decrypt(CryptographyDTO fileData);
        Task<byte[]> GetFileBytes(IFormFile formFile);
        Task<string> CreateFile(CryptographyDTO fileData, string encryptedFile);
        public DownloadFileDTO DownloadFile(string oldFileName, string newFileName, string operation);

    }
}
