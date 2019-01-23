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
    public class EchoBot
    {
        public static async Task EchoMessage(ConnectorClient connector, Activity activity)
        {
            //var message = Conversation.SendAsync(activity, () => new RootDialog());

            //var mes = Conversation.

            //await Conversation.SendAsync(activity, () => new RootDialog(connector));

            var reply = activity.CreateReply("You said: " + activity.GetTextWithoutMentions() + " TeamsChannel ID: " + TeamsChannel.Id); //  // + "And the ID of the convo is" + tenantId
            await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);  // .ReplyToActivityWithRetriesAsync(reply);
        }
    }
}