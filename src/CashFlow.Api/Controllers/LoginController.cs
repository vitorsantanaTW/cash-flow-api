using CashFlow.Application.UseCases.User.Login;
using Microsoft.AspNetCore.Mvc;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
    [FromServices] IDoLoginUseCase loginUseCase,
    [FromBody] RequestLoginJson request
    )
    {
        var response = await loginUseCase.Execute(request);
        return Ok(response);
    }
}