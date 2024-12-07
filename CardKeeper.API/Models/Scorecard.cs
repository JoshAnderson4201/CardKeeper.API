using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardKeeper.API.Models
{
    public class Scorecard
    {
        public string CourseName { get; set; }
        public string Date { get; set; }
        public Dictionary<string, HoleData> Holes { get; set; }
        public string FrontNineScore { get; set; }
        public string BackNineScore { get; set; }
        public string TotalScore { get; set; }
    }      

    public class HoleData
    {
        public string Yardage { get; set; }
        public string Par { get; set; }
        public string Handicap { get; set; }
        public string PlayerScore { get; set; }
    }
}