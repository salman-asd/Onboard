using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASD.Onboard.Web.Controllers;

public class AccountController : BaseController
{
    [HttpGet]
    public string GetWelocome()
    {
        return "Hello World!";
    }
}
