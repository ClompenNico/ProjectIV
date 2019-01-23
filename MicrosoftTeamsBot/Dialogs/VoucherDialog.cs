using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MicrosoftTeamsBot.Data;
using MicrosoftTeamsBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MicrosoftTeamsBot.Dialogs
{
    [Serializable]
    public class VoucherDialog : IDialog
    {
        //private readonly IUsersRepo _userRepo;
        //public VoucherDialog(IUsersRepo usersRepo)
        //{
        //    this._userRepo = usersRepo;
        //}

        public async Task StartAsync(IDialogContext context)
        {
            Random r = new Random();
            int rnd = r.Next(1, 6);
            string reply = "";

            //Choose answer
            switch (rnd)
            {
                case 1:
                    reply = "Give me a sec, I'm checking.";
                    break;
                case 2:
                    reply = "Just a second";
                    break;
                case 3:
                    reply = "Checking";
                    break;
                case 4:
                    reply = "Give me a minute";
                    break;
                case 5:
                    reply = "Just a sec";
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
            #region Name
            //Get name, split it and keep first name
            string name = context.Activity.From.Name;
            string[] splitName = name.Split(null);
            name = splitName[0].ToString();

            //Capitalize first letter the name
            char[] a = name.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            name = new string(a);
            #endregion

            //Get id
            string UsersId = context.Activity.From.Id;
            bool voucherReceived;

            try
            {

                //Use Id for database search
                Guid voucherId = MSTeamsBotDB.GetUserById(UsersId);

                //Use VouchersId to check for Received
                voucherReceived = MSTeamsBotDB.GetReceivedById(voucherId);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Use VouchersId to check for Received
            //Handle 0 or 1
            if (voucherReceived)
            {
                await context.PostAsync(String.Format($"Yes, you have received your vouchers"));
            }
            else
            {
                await context.PostAsync(String.Format($"No, you haven't received your vouchers yet"));
            }

            #region TryOut SaveState
            ////context.UserData.SetValue<bool>("VoucherReceived", true);
            //context.UserData.TryGetValue<bool>("VoucherReceived", out voucherReceived);

            ////Handle 0 or 1
            //if (voucherReceived)
            //{
            //    await context.PostAsync(String.Format($"Yes, you have received your vouchers"));
            //} else
            //{
            //    await context.PostAsync(String.Format($"No, you haven't received your vouchers yet"));
            //}
            #endregion

            context.Wait(MessageReceivedAsync);

            #region TryOuts save
            //var userName = String.Empty;
            //context.UserData.TryGetValue<string>("Name", out userName);
            //if (string.IsNullOrEmpty(userName))
            //{
            //    await context.PostAsync("What is your name?");
            //    context.UserData.SetValue<bool>("GetName", true);
            //}
            //else
            //{
            //    //POST context.Activity.From.Id, context.Activity.From.Name, context.Activity.From.Role
            //    await context.PostAsync(String.Format($"How can I help you today {context.Activity.From.Name}?"));
            //    context.Wait(MessageReceivedAsync);
            //}
            #endregion
        }

        //This method is gonna take care of the message received from the user
        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            context.Done(message);
        }
    }
}