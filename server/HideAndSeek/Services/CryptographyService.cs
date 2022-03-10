using HideAndSeek.DTOs;
using HideAndSeek.Services.Interfaces;
using System.Text;
using System.Text.RegularExpressions;

namespace HideAndSeek.Services
{
    public class CryptographyService : ICryptographyService
    {
        public CryptographyService()
        {

        }


        public async Task<byte[]> GetFileBytes(IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }


        public async Task<string> Encrypt(CryptographyDTO fileData)
        {
            byte[] keyBytes = Encoding.Default.GetBytes(fileData.Key);
            byte[] fileBytes = await GetFileBytes(fileData.File);

            for (int i = 0; i < fileBytes.Length; i++)
            {
                fileBytes[i] ^= keyBytes[i % keyBytes.Length];
            }

            return Convert.ToBase64String(fileBytes);
        }


        public async Task<string> Decrypt(CryptographyDTO fileData)
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            string pathString = $"{currentDirectory}\\EncryptedDecryptedFiles";
            pathString = Path.Combine(pathString, $"{fileData.File.FileName}.dec");

            using (FileStream fileStream = File.Create(pathString))
            {
                await fileData.File.CopyToAsync(fileStream);
            }

            int counter = 0;
            int encryptedLineLength = 0;
            StringBuilder encryptedFile = new StringBuilder();
            foreach (string line in File.ReadLines(pathString))
            {
                if (counter == 0)
                {
                    encryptedFile.Append(line);
                    encryptedLineLength = line.Length;
                } 
                else if(counter == 1)
                {
                    if (Convert.ToInt64(line) != Convert.ToInt64(encryptedLineLength))
                    {
                        throw new InvalidOperationException("Invalid encryption");
                    }
                    
                }
                counter++;
            }


            byte[] keyBytes = Encoding.Default.GetBytes(fileData.Key);
            byte[] fileBytes = Convert.FromBase64String(encryptedFile.ToString());

            for (int i = 0; i < fileBytes.Length; i++)
            {
                fileBytes[i] ^= keyBytes[i % keyBytes.Length];
            }

            if (File.Exists(pathString))
            {
                File.Delete(pathString);
            }

            return Encoding.Default.GetString(fileBytes);
        }


        public async Task<string> CreateFile(CryptographyDTO fileData, string encryptedFile)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string pathString = $"{currentDirectory}\\EncryptedDecryptedFiles";
            pathString = Path.Combine(pathString, $"{fileData.File.FileName}");

            if (File.Exists(pathString))
            {
                File.Delete(pathString);
            }

            List<string> lines = new() { encryptedFile };

            if (fileData.Operation == "encrypt")
            {
                lines.Add(encryptedFile.Length.ToString());
            }
            if (fileData.Crc && fileData.Operation == "encrypt")
            {
                lines.Add(fileData.Crc.ToString());
            }

            using StreamWriter file = new(pathString);

            foreach (string line in lines)
            {
                await file.WriteLineAsync(line);
            }

            return "Operation successful";

        }

        public DownloadFileDTO DownloadFile(string oldFileName, string newFileName, string operation)
        {
            string pattern = @"\b\.[a-zA-Z]{1,3}(.enc)\b";
            Match match = Regex.Match(oldFileName, pattern);

            string currentDirectory = Directory.GetCurrentDirectory();
            string pathString = $"{currentDirectory}\\EncryptedDecryptedFiles\\{oldFileName}"; 

            string fileName;
            if (match.Success && operation == "decrypt")
            {
                fileName = $"{newFileName}{oldFileName[^8..^4]}"; 
            }
            else if (operation == "encrypt")
            {
                fileName = $"{newFileName}.enc";
            }
            else
            {
                fileName = $"{newFileName}.dec";
            }

            byte[] fileBytes = File.ReadAllBytes(pathString);

            if (File.Exists(pathString))
            {
                File.Delete(pathString);
            }

            DownloadFileDTO downloadFileDTO = new DownloadFileDTO();
            downloadFileDTO.FileBytes = fileBytes;
            downloadFileDTO.FileName = fileName;

            return downloadFileDTO;
        }

        

    }
}

