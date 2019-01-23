using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicrosoftTeamsBot.Models
{
    public enum BugType
    {
        Security = 1,
        Crash = 2,
        Power = 3,
        Performance = 4,
        Usability = 5,
        SeriousBug = 6,
        Other = 7
    }

    public enum Reproducibility
    {
        Always = 1,
        Sometimes = 2,
        Rarely = 3,
        Unable = 4
    }

    //Attribute used so that the bot is able to use this class!
    [Serializable]
    public class BugReport
    {
        public string Title { get; set; }
        [Prompt("Enter a description for your report")]
        public string Description { get; set; }
        [Prompt("What is your first name?")]
        public string FirstName { get; set; }
        [Describe("Surname")]
        public string LastName { get; set; }
        [Prompt("What is the best date and time for a callback?")]
        public DateTime? BestTimeOfDayToCall { get; set; }
        [Pattern("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}")]
        public string PhoneNumber { get; set; }
        [Prompt("Please list the bug areas that best describe your issue . {||}")]
        public List<BugType> Bug { get; set; }
        public Reproducibility Reproduce { get; set; }

        //Formulier
        public static IForm<BugReport> BuildForm()
        {
            return new FormBuilder<BugReport>().Message("Please fill out a bug, if something is unclear please type 'help' or 'quit' to quit.").Build();
        }
    }
}