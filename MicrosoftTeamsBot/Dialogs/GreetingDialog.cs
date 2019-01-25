using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MicrosoftTeamsBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MicrosoftTeamsBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            Random r = new Random();
            int rnd = r.Next(1, 6);
            string reply = "";

            //Choose answer
            switch (rnd)
            {
                case 1:
                    reply = "Hey there, I'm Work bot!";
                    break;
                case 2:
                    reply = "Hello, my name is Work bot";
                    break;
                case 3:
                    reply = "Hi, I am Work bot";
                    break;
                case 4:
                    reply = "Howdy, I'm Work bot";
                    break;
                case 5:
                    reply = "Hi, I'm Work bot";
                    break;
            }

            await context.PostAsync(reply);
            //Takes care of responding to the user
            await Respond(context);

            //Then waits for the next message to be done and return to the MainDialog
            context.Wait(MessageReceivedAsync);
        }

        private async Task Respond(IDialogContext context)
        {
            //Get name, split it and keep first name
            string name = context.Activity.From.Name;
            string[] splitName = name.Split(null);
            name = splitName[0].ToString();

            //Capitalize first letter the name
            char[] a = name.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            name = new string(a);

            //POST context.Activity.From.Id, context.Activity.From.Name, context.Activity.From.Role
            await context.PostAsync(String.Format($"You can type 'help' if something is unclear."));
            context.Wait(MessageReceivedAsync);
        }

        //This method is gonna take care of the message received from the user
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            context.Done(message);
        }
    }
}