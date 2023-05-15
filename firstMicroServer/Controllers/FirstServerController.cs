using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System;
using Org.BouncyCastle.Utilities;
using System.Text;

namespace firstMicroServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirstServerController : ControllerBase
    {
        [HttpGet("/")]
        public string helloWorld()
        {
            return "Hello world, i am working";
        }
        [HttpPost("/encrypted-message")]
        public async Task<ActionResult<string>> EncryptedMessageAsync(string message)
        {
            return await FirstServerControllerFunctions.EncrypMessageAsync(message);
        }
    }
}