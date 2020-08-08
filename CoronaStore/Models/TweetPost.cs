using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoronaStore.Models
{
    public class TweetPost
    {
        public string PostUrl { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
    }
}