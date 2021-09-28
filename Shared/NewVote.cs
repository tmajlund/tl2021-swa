using System;

namespace BlazorApp.Shared
{
    public class NewVote
    {
        public string VoteId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Answer { get; set; }
    }
}