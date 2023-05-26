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
        [HttpGet("/decrypted-message-encrypted")]
        public ActionResult<string> Decryptedmessageencrypted()
        {
            return SecondServerControllerFunctions.Decrypt();
        }
    }
}
