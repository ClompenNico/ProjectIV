using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicrosoftTeamsBot.Models
{
    public class Users_Orders
    {
        public int Id { get; set; }
        public string UsersId { get; set; }
        public Guid OrdersId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}