using System;

namespace DefaultNamespace.Data.DataStructures
{
    [Serializable]
    public class PollEntry
    {
        public string text;
        public int id;
        public int voteCount;

        public string GetDisplay()
        {
            return $"#{id} {text} ({voteCount})";
        }
    }
}