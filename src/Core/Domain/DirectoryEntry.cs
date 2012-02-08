using System;

namespace Core.Domain
{
    public class DirectoryEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string WorkPhone { get; set; }
        public string WorkDescription { get; set; }
        public string RoomNumber { get; set; }
        public string RoomPhone { get; set; }
    }
}