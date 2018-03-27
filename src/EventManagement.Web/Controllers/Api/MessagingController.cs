using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using losol.EventManagement.Services;
using losol.EventManagement.Web.Services;
using losol.EventManagement.ViewModels;
using losol.EventManagement.Services.Messaging.Sms;

namespace losol.EventManagement.Web.Controllers.Api
{
    [Authorize]
	[Route("/api/v0/messaging")]
	public class MessagingController : Controller
	{
		private readonly StandardEmailSender _emailSender;
        private readonly ISmsSender _smsSender;

		public MessagingController(
            StandardEmailSender emailSender, 
            ISmsSender smsSender)
		{
			_emailSender = emailSender;
            _smsSender = smsSender;
		}

		[HttpPost("email")]
		public async Task<IActionResult> SendEmail([FromBody]EmailVM vm) 
		{
			if (!ModelState.IsValid) return BadRequest();
            var emailTasks = vm.To.Select(r => _emailSender.SendAsync(
                new EmailMessage
                {
                    Name = r.Name,
                    Email = r.Email,
                    Subject = vm.Subject,
                    Message = vm.Message
                })
            );
            await Task.WhenAll(emailTasks);
			return Ok();
		}

        [HttpPost("sms")]
		public async Task<IActionResult> SendSms([FromBody]SmsVM vm) 
		{
			if (!ModelState.IsValid) return BadRequest();
            var smsTasks = vm.To.Select(t => _smsSender.SendSmsAsync(t, vm.Text));
            await Task.WhenAll(smsTasks);
			return Ok();
		}

        public class SmsVM
        {
            [Required]
            public IEnumerable<string> To { get; set; }
            [Required]
            public string Text { get; set; }
        }

		public class EmailVM
		{
            public IEnumerable<EmailRecipientVM> To { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
		}

        public class EmailRecipientVM
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }
	}
}
