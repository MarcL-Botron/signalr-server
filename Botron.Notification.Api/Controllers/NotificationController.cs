using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Botron.Http.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Botron.Api;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;

namespace Botron.Notification.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        ILogger<NotificationController> Logger { get; }

        IConfiguration Configuration { get; }

        IHubContext<NotificationHub> HubContext { get; }

        public NotificationController(
            IConfiguration configuration,
            ILogger<NotificationController> logger,
            IHubContext<NotificationHub> hubContext)
        {
            Logger = logger;
            Configuration = configuration;
            HubContext = hubContext;
        }

        [HttpPost()]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(ExceptionResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ExceptionResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ExceptionResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Post(NotificationModel request)
        {
            if (request.Groups != null && request.Groups.Any())
            {
                await HubContext.Clients.Groups(request.Groups).SendAsync(request.Method, request.Data);
            }
            else
            {
                await HubContext.Clients.All.SendAsync(request.Method, request.Data);
            }

            return Accepted();
        }
    }
}
