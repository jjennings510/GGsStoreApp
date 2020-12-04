using System;

namespace GGsDB.Models
{
    public class VideoGame
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal cost { get; set; }
        public string platform { get; set; }
        public string esrb { get; set; }
        public string description { get; set; }
        public int apiId { get; set; }
        public string imageURL { get; set; }
    }
}