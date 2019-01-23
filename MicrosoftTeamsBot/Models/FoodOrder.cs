using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicrosoftTeamsBot.Models
{
    public class FoodOrder
    {
        public Guid Id { get; set; }
        public string TypeOfBun { get; set; }
        public string Toppings { get; set; }
        public string Salad { get; set; }
        public string Tomato { get; set; }
        public string Sauce { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
    }
}