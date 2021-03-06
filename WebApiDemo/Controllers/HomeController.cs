﻿using System.Web.Http;
using Conifer;

namespace WebApiDemo.Controllers
{
	public class HomeController : ApiController
	{
		private readonly Router _router;

		public HomeController(Router router)
		{
			_router = router;
		}

		public string Get()
		{
			return "HomeController.Get: " +  _router.LinkTo<PersonController>(p => p.GetView(123));
		}
	}
}
