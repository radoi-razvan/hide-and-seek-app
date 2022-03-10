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
            try
            {
                encryptedFile = await _cryptographyService.Encrypt(fileData);
            }
            catch (Exception)
            {
                return BadRequest("Operation failed!");
            }

            string message = await _cryptographyService.CreateFile(fileData, encryptedFile);
            _logger.LogInformation("Operation successful");
            return CreatedAtAction("EncryptFile", message);
        }

        // POST: cryptography/decrypt
        [AllowAnonymous]
        [HttpPost("decrypt")]
        public async Task<ActionResult<string>> DecryptFile([FromForm] CryptographyDTO fileData)
        {
            string decryptedFile;
            try
            {
                decryptedFile = await _cryptographyService.Decrypt(fileData);
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Invalid encryption");
            }
            catch (Exception)
            {
                return BadRequest("Operation failed!");
            }

            string message = await _cryptographyService.CreateFile(fileData, decryptedFile);
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

