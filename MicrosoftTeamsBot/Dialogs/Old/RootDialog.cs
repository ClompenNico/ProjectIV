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
    public class RootDialog : IDialog<object>
    {
        //[NonSerialized] private readonly ConnectorClient connector;

        //public RootDialog(ConnectorClient _connector)
        //{
        //    connector = _connector;
        //}

        //StartAsync needed to implement the Dialog interface!
        public Task StartAsync(IDialogContext dialogContext)
        {
            //This means "wait for the user send a message" then call method "MessageReceivedAsync"
            dialogContext.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        //The method that will be called if the user sends you a message
        public async Task MessageReceivedAsync(IDialogContext dialogContext, IAwaitable<object> result)
        {
            //The awaitable result can be translated into an activity
            var activity = await result as Activity;

            using (var connector = new ConnectorClient(new Uri(activity.ServiceUrl)))
            {
                //Do something with this activity => calculate something for us to return (if the text is not empty, put length in integer)
                int length = (activity.Text ?? string.Empty).Length;

                //Return our reply to the user
                //await dialogContext.PostAsync($"You sent {activity.Text} which was {length} characters");

                var reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters and has a TeamsChannel ID of {TeamsChannel.Id}");

                try
                {
                    await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //To make this wait for the next message, like a loop
                dialogContext.Wait(MessageReceivedAsync);
            }
        }

        //Method that will send out the message through Teams
        //public async Task SendMessage(Activity activity, int length)
        //{
        //    var reply = activity.CreateReply($"You said: {activity.Text} with length: {length} and TeamsChannel ID: {TeamsChannel.Id}"); // + "And the ID of the convo is" + tenantId
        //    await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);
        //}
    }
}