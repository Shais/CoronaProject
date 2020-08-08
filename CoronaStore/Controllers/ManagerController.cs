using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using CoronaStore.Models;
using CoronaStore.Services;

namespace CoronaStore.Controllers
{
    public class ManagerController : Controller
    {
        TweetService ts = new TweetService();

        [HttpPost]
        public async Task<ActionResult> SendTweet(string text)
        {
            TweetPost newTweet = null;
            try
            {
                newTweet = await TweetService.Tweet(text);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(newTweet);
        }
    }
}