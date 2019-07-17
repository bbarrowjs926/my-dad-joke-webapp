using System.Threading.Tasks;
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
        public async Task<ActionResult> GetRandomJoke()
        {
            var handler = new DadJokeHandler(new DadJokeService()); 
            var vm = await handler.GetRandomJoke();


            return Json(new {Result = vm.Joke}, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetJokesWithTerm(string jokeTerm)
        {
            var handler = new DadJokeHandler(new DadJokeService()); 
            var vm = await handler.GetJokesWithTerm(jokeTerm);

            return PartialView("_FilteredJokesList", vm);
        }
    }
}