using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using MicrosoftTeamsBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MicrosoftTeamsBot.Dialogs
{
    [Serializable]
    public class NoneDialog : IDialog
    {
        //StartAsync needed to implement the Dialog interface!
        public async Task StartAsync(IDialogContext dialogContext)
        {
            string guide = " try typing 'help'.";

            Random r = new Random();
            int rnd = r.Next(1, 6);
            string reply = "";

            //Choose answer
            switch (rnd)
            {
                case 1:
                    reply = "I'm sorry I don't know what you mean,";
                    break;
                case 2:
                    reply = "I can't figure out what you mean by that,";
                    break;
                case 3:
                    reply = "That's unclear for me,";
                    break;
                case 4:
                    reply = "I'm not sure what you mean by that,";
                    break;
                case 5:
                    reply = "Sorry I don't understand,";
                    break;
            }

            await dialogContext.PostAsync(String.Format(reply + guide));

            //This means "wait for the user send a message" then call method "MessageReceivedAsync"
            var activity = new Activity();
            dialogContext.Done(activity);
        }
    }
}