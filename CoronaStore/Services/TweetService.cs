using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoronaStore.Models;
using Tweetinvi;

namespace CoronaStore.Services
{
    public class TweetService
    {
        public static async Task<TweetPost> Tweet(String textToTweet)
        {
            var tweet = await TweetAsync.PublishTweet(textToTweet);

            if (tweet.IsTweetPublished)
            {
                TweetPost newTweet = new TweetPost()
                {
                    Text = tweet.Text,
                    PostUrl = tweet.Url,
                    CreationDate = tweet.TweetLocalCreationDate
                };

                return newTweet;
            }

            return null;
        }
    }
}