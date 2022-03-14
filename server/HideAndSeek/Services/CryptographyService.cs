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
            string currentDirectory = Directory.GetCurrentDirectory();
            string currentFilePathString = $"{currentDirectory}\\EncryptedDecryptedFiles";
            currentFilePathString = Path.Combine(currentFilePathString, $"{fileData.File.FileName}");

            using (FileStream fileStream = File.Create(currentFilePathString))
            {
                await fileData.File.CopyToAsync(fileStream);
            }

            List<string> file = new List<string>();
            if (fileData.Crc)
            {
                file.Add($"{fileData.File.Length};{fileData.Crc}");
            }
            else
            {
                file.Add(fileData.File.Length.ToString());
            }

            foreach (string line in File.ReadLines(currentFilePathString))
            {
                file.Add(line);
            }

            if (File.Exists(currentFilePathString))
            {
                File.Delete(currentFilePathString);
            }

            string newFilePathString = $"{currentDirectory}\\EncryptedDecryptedFiles";
            newFilePathString = Path.Combine(newFilePathString, $"New{fileData.File.FileName}");
            using (StreamWriter newFile = new(newFilePathString))
            {
                foreach (string line in file)
                {
                    await newFile.WriteLineAsync(line);
                }
            }

            byte[] keyBytes = Encoding.Default.GetBytes(fileData.Key);
            byte[] fileBytes = File.ReadAllBytes(newFilePathString);

            for (int i = 0; i < fileBytes.Length; i++)
            {
                fileBytes[i] ^= keyBytes[i % keyBytes.Length];
            }

            if (File.Exists(newFilePathString))
            {
                File.Delete(newFilePathString);
            }

            return Convert.ToBase64String(fileBytes);
        }


        public async Task<string> Decrypt(CryptographyDTO fileData)
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            string pathString = $"{currentDirectory}\\EncryptedDecryptedFiles";
            pathString = Path.Combine(pathString, $"{fileData.File.FileName}");

            using (FileStream fileStream = File.Create(pathString))
            {
                await fileData.File.CopyToAsync(fileStream);
            }

            StringBuilder encryptedFile = new StringBuilder();
            foreach (string line in File.ReadLines(pathString))
            {
                encryptedFile.Append(line);
            }


            byte[] keyBytes = Encoding.Default.GetBytes(fileData.Key);
            byte[] fileBytes = Convert.FromBase64String(encryptedFile.ToString());

            for (int i = 0; i < fileBytes.Length; i++)
            {
                fileBytes[i] ^= keyBytes[i % keyBytes.Length];
            }

            var result = Encoding.Default.GetString(fileBytes);
            //var splittedTxt = result.Split("/r/n");
            //int fileLengthIndex = 0;
            //int firstLineIndex = 0;
            //int secondLineIndex = 1;
            //var fileLength = splittedTxt[firstLineIndex].Split(';')[fileLengthIndex];
            //var firstLineLength = splittedTxt[secondLineIndex].Length;
            //if (Convert.ToInt64(fileLength) != Convert.ToInt64(fileData.File.Length)- Convert.ToInt64(firstLineLength))
            //{
            //    throw new InvalidOperationException("Invalid encryption");
            //}

            if (File.Exists(pathString))
            {
                File.Delete(pathString);
            }

            return result;
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

