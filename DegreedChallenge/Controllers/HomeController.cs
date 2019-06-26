using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DegreedChallenge.Handlers;
using DegreedChallenge.Services;

namespace DegreedChallenge.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetRandomJoke()
        {
            var handler = new DadJokeHandler(new DadJokeService()); 
            var vm = handler.GetRandomJoke();


            return Json(new {Result = vm.Joke}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetJokesWithTerm(string jokeTerm)
        {
            var handler = new DadJokeHandler(new DadJokeService()); 
            var vm = handler.GetJokesWithTerm(jokeTerm);

            return PartialView("_FilteredJokesList", vm);
        }
    }
}