using Microsoft.AspNetCore.Mvc;
using EncryptDecryptLibray;
namespace secondMicroServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecondServerController : ControllerBase
    {
        [HttpGet("/")]
        public string helloWorld()
        {
            return "Hello world, i am working";
        }
        [HttpPost("/decrypting-encrypted-data")]
        public ActionResult<string> DecryptingEncryptedData([FromBody] string encryptedData)
        {
            byte[] data = Convert.FromBase64String(encryptedData);
            return SecondServerControllerFunctions.Decrypt(data);
        }
    }
}
