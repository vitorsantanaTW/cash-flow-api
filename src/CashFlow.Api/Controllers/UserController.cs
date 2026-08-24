using CashFlow.Application.UseCases.User.Register;
using Microsoft.AspNetCore.Mvc;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
   [HttpPost]
   [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
   [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
   public async Task<IActionResult> Register(
    [FromServices] IRegisterUserUseCase registerUserUseCase, 
    [FromBody] RequestRegisterUserJson request
    )
   {
     var response  = await registerUserUseCase.Execute(request);
     return Ok(response);
   }
}