using System;
using Microsoft.AspNetCore.Mvc;

namespace CookingAssistantBackend.Utilis
{
    public class CustomController : ControllerBase
    {
        // private readonly UnitOfWork _unitOfWork;

        public CustomController(/*UnitOfWork unitOfWork*/)
        {
            // _unitOfWork = unitOfWork;
        }

        protected new IActionResult Ok()
        {
            // _unitOfWork.Commit();
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult Ok<T>(T result)
        {
            // _unitOfWork.Commit();
            return base.Ok(Envelope.Ok(result));
        }

        protected IActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }
    }
}

