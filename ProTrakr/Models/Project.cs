using System;
using Realms;

namespace ProTrakr.Models
{
    public class Project : RealmObject
    {
        public string Name { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset CompletionData { get; set; }
    }
}
