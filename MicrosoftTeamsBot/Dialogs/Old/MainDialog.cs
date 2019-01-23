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
    public class MainDialog
    {
        private async Task MessageReceivedAsync(ConnectorClient connector, Activity activity)
        {
            //await Conversation.SendAsync(activity, () => new RootDialog());
            var reply = activity.CreateReply("You said: " + activity.GetTextWithoutMentions() + " TeamsChannel ID: " + TeamsChannel.Id);
            //await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);
            await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);
        }
    }
}