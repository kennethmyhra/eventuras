
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using losol.EventManagement.Domain;
using losol.EventManagement.Services;
using losol.EventManagement.Services.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace losol.EventManagement.Web.Api.Controllers
{
	[Route("api/v0/events")]
	public class EventsController : Controller
	{
		private readonly IEventInfoService _eventsService;
		private readonly IEmailSender _emailSender;

		public EventsController(IEventInfoService eventsService, IEmailSender emailSender)
		{
			_eventsService = eventsService;
			_emailSender = emailSender;
		}

		[Route("test")]
		public async Task<IActionResult> TestEmail() 
		{
			await _emailSender.SendEmailAsync("test@test.com", "Subject", "Hey there delilah!");
			return Ok();
		}

		[HttpGet, Route("upcoming")]
		public async Task<IActionResult> Get()
		{
			var events = await _eventsService.GetUpcomingEventsAsync();
			var list = events.Select(s => new
			{
				Id = s.EventInfoId,
				s.Title,
				s.Description,
				s.Location,
				s.City,
				s.DateStart,
				s.DateEnd,
				s.FeaturedImageUrl
			});
			return Ok(list);
		}

	}
}