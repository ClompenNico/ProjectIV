using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using MicrosoftTeamsBot.Data;
using MicrosoftTeamsBot.Models;
using MicrosoftTeamsBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace MicrosoftTeamsBot.Dialogs
{
    //Using BotBuilderLuis
    [LuisModel("587a3daf-a5b6-47c9-8db9-e6b6e46e6322", "4b8f0f2cc8984071b05b659205e724b9", domain: "westus.api.cognitive.microsoft.com", Staging = true)]
    //For the bot to use the class
    [Serializable]
    //Using BotBuilderDialogs & BUGREPORT MODEL
    public class LUISDialog : LuisDialog<Sandwich>
    {
        //private readonly IUsersRepo _userRepo;
        private readonly BuildFormDelegate<Sandwich> Sandwich;
        //Injecting a delegate to be able to pass in a new BugReport (so a filled in form)
        public LUISDialog(BuildFormDelegate<Sandwich> sandwich /*, IUsersRepo usersRepo*/)
        {
            this.Sandwich = sandwich;
            //this._userRepo = usersRepo;
        }

        //Callback method is used to wait for the next message to receive
        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            //Everything always leads back to => LUIS, waiting for the next message to receive
            context.Wait(MessageReceived);
        }

        #region TryOut
        //enum yesno
        //{
        //    True = 1,
        //    False = 0
        //}
        #endregion

        //Callback method is used to wait for the next message to receive
        #region MailFail
        private async Task Mail(IDialogContext context, IAwaitable<object> result)
        {
            #region TryOut SendEMail
            //Everything always leads back to => LUIS, waiting for the next message to receive
            //MailMessage mail = new MailMessage("NoReplyBot@clompenbot.onmicrosoft.com", $"nico.clompen@gmail.com");
            //SmtpClient client = new SmtpClient();
            //client.Port = 465; //25 465 587
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Host = "smtp.gmail.com";
            //mail.Subject = "Order of the day.";
            //mail.Body = "this is a test email";
            //client.Send(mail);
            #endregion

            //List<FoodOrder> sandwiches = MSTeamsBotDB.GetSandwichByUserId(context.Activity.From.Id);
            
            //string greeting = $"New order in from {context.Activity.From.Name}:\r\n \r\n";
            //string body = "";
            //string ending = "\r\n \r\nThanks in advance!";

            //var sandwich = MSTeamsBotDB.foodOrder;

            //string tomato = "";
            //string salad = "";
            //if (sandwich.Tomato.ToString() == "true")
            //{
            //    tomato = ", tomato";
            //}

            //if (sandwich.Salad.ToString() == "true")
            //{
            //    salad = ", salad";
            //}

            //body = $"{sandwich.TypeOfBun} sandwich with {sandwich.Toppings}{salad}{tomato} and {sandwich.Sauce} as sauce.";

            //string message = greeting + body + ending;

            //var client = new SmtpClient("smtp.gmail.com", 587)
            //{
            //    Credentials = new NetworkCredential("MSTeamsBot@gmail.com", "MST3@MSB0T"),
            //    EnableSsl = true
            //};
            //client.Send("MSTeamsBot@gmail.com", "nico.clompen@gmail.com", "Order of the day.", message);
            //Console.WriteLine("Sent");
            //Console.ReadLine();

            //context.Wait(MessageReceived);
        }
        #endregion

        [LuisIntent("None")]
        //This is the none intent if luis doesn't recognize none of the other intents!
        public async Task None(IDialogContext context, LuisResult result)
        {
            //await context.PostAsync("I'm sorry I don't know what you mean, currently you can greet me and fill in a bug report.");
            context.Call(new NoneDialog(), Callback);
            //context.Wait(MessageReceived);
        }


        [LuisIntent("GreetingIntent")]
        //This is the GreetingIntent if luis thinks you are greeting him, you will call the GreetingDialog
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                //First will call GreetingDialog, when that is done, Call Callback and wait for the next message to receive
                context.Call(new GreetingDialog(), Callback);
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("HelpIntent")]
        //This is the GreetingIntent if luis thinks you are greeting him, you will call the GreetingDialog
        public async Task Help(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                //First will call GreetingDialog, when that is done, Call Callback and wait for the next message to receive
                await context.PostAsync(
                    "Currently you can order a sandwich,    " +
                    "\r\nAsk what your orders of today were    " +
                    "\r\nAsk if you have received your vouchers    " +
                    "\r\nAsk where to find the maintenance material    " +
                    "\r\nAsk for a list of the company's numbers    " +
                    "\r\nAsk for the company's opening hours    " +
                    "\r\nAnd when the holidays are    " +
                    "\r\nAsk what the wifi password is");
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }

        //VouchersIntent
        [LuisIntent("VouchersIntent")]
        //This is the GreetingIntent if luis thinks you are greeting him, you will call the GreetingDialog
        public async Task Vouchers(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.300)
            {
                //First will call GreetingDialog, when that is done, Call Callback and wait for the next message to receive
                context.Call(new VoucherDialog(/*_userRepo*/), Callback);
            }
            else
            {
                await None(context, result);
            }
        }

        //SwearingIntent
        [LuisIntent("SwearingIntent")]
        //This is the GreetingIntent if luis thinks you are greeting him, you will call the GreetingDialog
        public async Task Swearing(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.750)
            {
                //First will call GreetingDialog, when that is done, Call Callback and wait for the next message to receive
                //context.Call(new SwearingDialog(), Callback);
                await context.PostAsync("That's not so nice of you to say.");
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("OrderSandwichIntent")]
        public async Task Order(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                //First check if already ordered
                List<FoodOrder> foodOrders = MSTeamsBotDB.GetSandwichByUserId(context.Activity.From.Id);
                int date = 0;

                foreach(var foodOrder in foodOrders)
                {
                    if (foodOrder.Date.Date == DateTime.Today.Date)
                    {
                        date += 1;
                    }
                }

                if (date <= 0)
                {
                    //Using BotBuilder FORMFLOW => being able to start up the form flow!!!
                    var enrollmentForm = new FormDialog<Sandwich>(new Sandwich(), this.Sandwich, FormOptions.PromptInStart);
                    context.Call<Sandwich>(enrollmentForm, Callback);
                }
                else
                {
                    await context.PostAsync($"Be aware, you already ordered a sandwich today!");
                    var enrollmentForm = new FormDialog<Sandwich>(new Sandwich(), this.Sandwich, FormOptions.PromptInStart);
                    context.Call<Sandwich>(enrollmentForm, Callback);
                }

            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("OrderedSandwichIntent")]
        public async Task Ordered(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                List<FoodOrder> foodOrders = MSTeamsBotDB.GetSandwichByUserId(context.Activity.From.Id);
                if (foodOrders.Count > 0)
                {
                    string message = "Today you ordered:    \r\n";

                    foreach (var foodOrder in foodOrders)
                    {
                        string tomato = "";
                        string salad = "";
                        if (foodOrder.Tomato.ToString() == "Yes")
                        {
                            tomato = ", tomato";
                        }

                        if (foodOrder.Salad.ToString() == "Yes")
                        {
                            salad = ", salad";
                        }

                        string body = $" {foodOrder.Amount} {(foodOrder.TypeOfBun).ToLower()} sandwich(es) with {(foodOrder.Toppings).ToLower()}{salad}{tomato} and {(foodOrder.Sauce).ToLower()} as sauce.    \r\n";
                        message += body;
                    }

                    await context.PostAsync(message);
                    context.Wait(MessageReceived);
                } else
                {
                    await context.PostAsync("You haven't ordered anything yet today.");
                    context.Wait(MessageReceived);
                }
            }
        }

        [LuisIntent("ThankYouIntent")]
        public async Task ThankYou(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                await context.PostAsync("You're welcome!");
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }        

        
        [LuisIntent("WifiPasswordIntent")]
        public async Task WifiPassword(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                await context.PostAsync("The wifi password is Gsk3csk93");
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("CompanyNumbersIntent")]
        public async Task CompanyNumbers(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                await context.PostAsync("Steven (boss): 0495049584.    \r\nBob: 0485940294    \r\nJilly: 0495869584    \r\nNico: 0495844494");
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("OpeningHoursCompanyIntent")]
        public async Task OpeningHours(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                await context.PostAsync("The company opens up everyday from 8:00 to 18:00 except on sundays and holidays.");
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("HolidaysIntent")]
        public async Task Holidays(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                await context.PostAsync("1 Jan,    " +
                    "\r\n20 Mar, 31 Mar,    " +
                    "\r\n19 Apr, 21 Apr, 22 Apr,    " +
                    "\r\n1 May, 30 May,    " +
                    "\r\n9 Jun, 10 Jun, 21 Jun, 21 Jun,    " +
                    "\r\n21 Jul,    " +
                    "\r\n15 Aug,    " +
                    "\r\n23 Sep,    " +
                    "\r\n27 Oct,    " +
                    "\r\n1 Nov, 11 Nov,    " +
                    "\r\n22 Dec, 24 Dec, 25 Dec, 31 Dec");
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("MaintenanceMaterialIntent")]
        public async Task MaintenanceMaterial(IDialogContext context, LuisResult result)
        {
            //Voor een entity op te zoeken in het type "BugType"
            foreach (var entity in result.Entities.Where(Entity => Entity.Type == "MaintenanceMaterial"))
            {
                //Eerst lowercase alles
                var value = entity.Entity.ToLower();
                //De lijst enums vergelijken met de bugtypes dat we hebben!
                if (Enum.GetNames(typeof(MaintenanceMaterial)).Where(a => a.ToLower().Equals(value)).Count() > 0)
                {
                    await context.PostAsync("Next to the enterance there is a small room labeled 'storage room'. You should be able to find it in there!");
                    context.Wait(MessageReceived);
                    return;
                }
                else
                {
                    await context.PostAsync("I'm sorry I am unable to locate this for you");
                    context.Wait(MessageReceived);
                    return;
                }
            }
            await context.PostAsync("I'm sorry I am unable to locate this for you");
            context.Wait(MessageReceived);
            return;
        }


        #region OrderSandwich fail
        //private async Task OrderSandwich(IDialogContext context, IAwaitable<Sandwich> result)
        //{
        //    var sandwich = result;
        //    //context.UserData.SetValue<string>("Sandwich", sandwich);


        //    await context.PostAsync("Sandwich ordered!");
        //    context.Wait(MessageReceived);
        //}
        #endregion

        #region BugreportExample
        [LuisIntent("NewBugReportIntent")]
        public async Task bugReport(IDialogContext context, LuisResult result)
        {
            if (result.TopScoringIntent.Score > 0.500)
            {
                //Usinig BotBuilder FORMFLOW => being able to start up the form flow!!!
                //var enrollmentForm = new FormDialog<BugReport>(new BugReport(), this.NewBugReport, FormOptions.PromptInStart);
                //context.Call<BugReport>(enrollmentForm, Callback);
                await context.PostAsync("Not available anymore, sorry.");
                context.Wait(MessageReceived);
            }
            else
            {
                await None(context, result);
            }
        }

        [LuisIntent("QueryBugType")]
        public async Task QueryBugTypes(IDialogContext context, LuisResult result)
        {
            //Voor een entity op te zoeken in het type "BugType"
            foreach (var entity in result.Entities.Where(Entity => Entity.Type == "BugType"))
            {
                //Eerst lowercase alles
                var value = entity.Entity.ToLower();
                //De lijst enums vergelijken met de bugtypes dat we hebben!
                if (Enum.GetNames(typeof(BugType)).Where(a => a.ToLower().Equals(value)).Count() > 0)
                {
                    await context.PostAsync("Yes that is a bug type!");
                    context.Wait(MessageReceived);
                    return;
                }
                else
                {
                    await context.PostAsync("I'm sorry that is not a bug type.");
                    context.Wait(MessageReceived);
                    return;
                }
            }
            await context.PostAsync("I'm sorry that is not a bug type.");
            context.Wait(MessageReceived);
            return;
        }
        #endregion
    }
}