using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Tweetinvi;

namespace CoronaStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private const string TWITTER_CONSUMER_ID = "p8baSjc6aM0bIqdQtOAF5LyVw";
        private const string TWITTER_CONSUMER_SECRET = "yshUXC2smlQW7TLmDmqQTMcBg5DqRxwVRYy7Yl5BQJhujXMFGL";
        private const string TWITTER_ACCESS_TOKEN = "1159126882492846080-czt6objfq6HzWtqf5iZjCbmamNuCz8";
        private const string TWITTER_ACCESS_TOKEN_SECRET = "aGFIZOP845ZaSx25uoWC0pU3L37bFC1k8QerpPKUvXf6f";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Auth.SetUserCredentials(TWITTER_CONSUMER_ID,
                                    TWITTER_CONSUMER_SECRET,
                                    TWITTER_ACCESS_TOKEN,
                                    TWITTER_ACCESS_TOKEN_SECRET);
        }
    }
}
