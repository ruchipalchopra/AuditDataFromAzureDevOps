using System;
using System.Collections.Generic;

namespace bw.Function
{
     internal class ProjectObject
    {
        public int count { get; set; }
        public List<ProjectItems> value { get; set; }
    }

    internal class ProjectItems
    {

        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public int revision { get; set; }
        public string visibility { get; set; }
        public DateTime lastUpdateTime { get; set; }

    }
}