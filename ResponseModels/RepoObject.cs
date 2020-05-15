using System;
using System.Collections.Generic;

namespace bw.Function
{
    //Check if classes should be public or internal
    public class RepoObject
    {
        public int count { get; set; }
        public List<RepoItems> value { get; set; }
    }
    public class RepoItems
    {
            public string id { get; set; }
            public string name{ get; set; }
            public string url { get; set; }
            public ProjectDetail project { get; set; }

            public string defaultBranch { get; set; }
            public int size { get; set; }
            public string remoteUrl { get; set; }
            public string sshUrl { get; set; }
            public string webUrl { get; set; }
    }

    public class ProjectDetail
    {
            public string id { get; set; }
            public string name { get; set; }
            public string urlid { get; set; }
            public string state { get; set; }
            public int revision { get; set; }
            public string visibility { get; set; }
            public DateTime lastUpdateTime  { get; set; }

    }

}