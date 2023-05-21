using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicDmgAPI.Entity
{
    public class Indicator
    {
        public int Id { get; set; }
        public string RussianName { get; set; }
        public string Title { get; set; }
        public string EnglishName { get; set; }

        public Indicator(int id, string russianName, string title, string englishName)
        {
            Id = id;
            RussianName = russianName;
            Title = title;
            EnglishName = englishName;
        }
    }
}