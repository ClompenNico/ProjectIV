using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Connector.Teams.Models;
using MicrosoftTeamsBot.Dialogs;
using MicrosoftTeamsBot.Models;
using MicrosoftTeamsBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MicrosoftTeamsBot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            using (var connector = new ConnectorClient(new Uri(activity.ServiceUrl)))
            {
                //Used to check wether someone has joined the convo
                try
                {
                    TeamsChannelData channelData = activity.GetChannelData<TeamsChannelData>();
                    TeamsChannel.Id = channelData.Tenant.Id;
                }
                catch { }
                
                //Full inbound message in JSON (ACTIVITY)
                if (activity.Type == ActivityTypes.Message)
                {

                    try
                    {
                        //Create user if user doesn't exist
                        MSTeamsBotDB.CreateUserData(activity.From.Id, activity.From.Name);

                        //Send message to LUIS
                        await Conversation.SendAsync(activity, MakeLuisDialog);
                        //await Conversation.SendAsync(activity, () => new RootDialog());
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    #region TryOuts
                    //BotBuilderDIALOGS!!!
                    //Als het een bericht is dan laten we LUIS hier overnemen!
                    //await Conversation.SendAsync(activity, () => new RootDialog());

                    //Next task || try to "convert" this?
                    //await Conversation.SendAsync(activity, MakeLuisDialog);
                    //return new HttpResponseMessage(HttpStatusCode.OK);
                    #endregion

                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }
                else
                {
                    await HandleSystemMessage(activity, connector);

                    //await EchoBot.EchoMessage(connector, activity);
                    return new HttpResponseMessage(HttpStatusCode.Accepted);
                }
            }
        }

        //Luis dialog
        internal IDialog<Sandwich> MakeLuisDialog()
        {
            return Chain.From(() => new LUISDialog(Sandwich.BuildForm /*, _userRepo*/));
        }

        //ALS ER EEN ACTIVITEIT GEBEURT BINNEN IN HET GESPREK
        private async Task HandleSystemMessage(Activity message, ConnectorClient connector)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                //Implement user deletion here
                //If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                //Handle conversation state changes, like members being added and removed
                //Use ACtivity.MembersAdded and Activity.MemberRemoved and Activity.Action for info
                //Not available in all channels

                if (message.MembersAdded[0].Name != "Bot")
                {
                    //await Conversation.SendAsync(message, () => new GreetingDialog());
                    MSTeamsBotDB.CreateUserData(message.From.Id, message.From.Name);
                    var reply = message.CreateReply($"Hi I noticed you were added to the teams group, try typing 'help' if something is unclear."); //  // + "And the ID of the convo is" + tenantId
                    await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                //Handle add/remove from contact lists
                //Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                //Handle knowing that the user is typing
                
                //var reply = message.CreateReply("I noticed you are typing"); //  // + "And the ID of the convo is" + tenantId
                //await connector.Conversations.ReplyToActivityWithRetriesAsync(reply);
            }
            else if (message.Type == ActivityTypes.Ping)
            {
                //Test type to make sure the bot is active
            }
        }
    }
}