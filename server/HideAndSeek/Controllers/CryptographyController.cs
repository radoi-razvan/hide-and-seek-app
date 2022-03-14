using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HideAndSeek.DTOs;
using HideAndSeek.Services.Interfaces;
using System.Text.RegularExpressions;


namespace HideAndSeek.Controllers
{
    [Authorize]
    //[ApiController]
    [Route("cryptography")]
    public class CryptographyController : Controller
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly ILogger<CryptographyController> _logger;

        public CryptographyController(ICryptographyService cryptographyService, ILogger<CryptographyController> logger)
        {
            _cryptographyService = cryptographyService;
            _logger = logger;
        }

        // POST: cryptography/encrypt
        [AllowAnonymous]
        [HttpPost("encrypt")]
        public async Task<ActionResult<string>> EncryptFile([FromForm] CryptographyDTO fileData)
        {
            string encryptedFile;
            string message;
            try
            {
                encryptedFile = await _cryptographyService.Encrypt(fileData);
                message = await _cryptographyService.CreateFile(fileData, encryptedFile);
            }
            catch (Exception)
            {
                return BadRequest("Operation failed!");
            }

            _logger.LogInformation("Operation successful");
            return CreatedAtAction("EncryptFile", message);
        }

        // POST: cryptography/decrypt
        [AllowAnonymous]
        [HttpPost("decrypt")]
        public async Task<ActionResult<string>> DecryptFile([FromForm] CryptographyDTO fileData)
        {
            string decryptedFile;
            string message;
            try
            {
                decryptedFile = await _cryptographyService.Decrypt(fileData);
                message = await _cryptographyService.CreateFile(fileData, decryptedFile);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Invalid encryption");
            }
            catch (Exception)
            {
                return BadRequest("Operation failed!");
            }

            _logger.LogInformation("Operation successful");
            return CreatedAtAction("EncryptFile", message);
        }

        // GET: cryptography/download
        [AllowAnonymous]
        [HttpGet("download/{oldFileName}/{newFileName}/{operation}")]
        public FileResult DownloadFile(string oldFileName, string newFileName, string operation)
        {
            var downloadFileDTO = _cryptographyService.DownloadFile(oldFileName, newFileName, operation); 

            Response.Headers.Add("content-disposition", $"attachment; filename={downloadFileDTO.FileName}");

            _logger.LogInformation("Operation successful");

            return File(downloadFileDTO.FileBytes, "application/octet-stream", downloadFileDTO.FileName);

        }

       

    }
}

