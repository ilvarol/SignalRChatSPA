using SignalRChatSPA.Models;
using System.Collections.Generic;

namespace SignalRChatSPA.Models
{
    public class GroupSource
    {
        public GroupSource()
        {
            Groups = new List<Group>();
            Groups.Add(new Group() { GroupName = "general" });
            Groups.Add(new Group() { GroupName = "art" });
            Groups.Add(new Group() { GroupName = "politics" });
            Groups.Add(new Group() { GroupName = "sport" });
            Groups.Add(new Group() { GroupName = "cinema" });
        }

        public List<Group> Groups { get; }
    }
}