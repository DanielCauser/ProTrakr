using System;
using System.Collections.Generic;
using Realms;

namespace ProTrakr.Models
{
    public class Client : RealmObject
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public IList<Project> Projects { get; } = new List<Project>();
    }
}
