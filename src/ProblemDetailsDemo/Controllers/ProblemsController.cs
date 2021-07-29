using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ProblemDetailsDemo.Controllers
{
    [ApiController] // Enables automatic validation
    [Route("[controller]")]
    public class ProblemsController : ControllerBase
    {
        [HttpPost("generic-problem")]
        public IActionResult GenericProblem()
            => Problem(
                type: "https://docs.truelayer.com/problems/insufficient-funds",
                title: "Insufficient Funds",
                statusCode: (int)HttpStatusCode.BadRequest
            );
        
        [HttpPost("implicit-validation")]
        public IActionResult ImplicitValidation(ImplicitRequest request) => Ok();

        [HttpPost("validation-problem")]
        public IActionResult ValidationProblemResult()
        {
            ModelState.AddModelError("Nope", "You shall not pass!");
            return ValidationProblem(ModelState);
        }

        [HttpPost("validation-problem-extended")]
        public IActionResult ValidationProblemExtendedResult()
        {
            ModelState.AddModelError("Nope", "You shall not pass!");
            return ValidationProblem(
                statusCode: 422,
                instance: "https://logs.truelayer.com/traces/" + HttpContext.TraceIdentifier,
                modelStateDictionary: ModelState
            );
        }

        [HttpPost("exception")]
        public IActionResult Exception() => throw new Exception("Uh oh");

        [HttpPost("custom-exception")]
        public IActionResult CustomException() => throw new BankIsDeadException();
    }

    public class ImplicitRequest
    {
        [Required]
        public string FullName { get; set; }
    }
}
