using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using MicrosoftTeamsBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicrosoftTeamsBot.Models
{
    public enum MaintenanceMaterial
    {
        Broom = 1,
        Maintenance = 2,
        Sponges = 3,
        Microfiber = 4,
        HandBrush = 5,
        Vacuum= 6,
        Swiffer = 7,
        Trashbags = 8,
        Trashbag = 9,
        Rags = 10,
        Toiletpaper = 11,
        Towels = 12,
        Bleach = 13,
    }

    public enum TypeOfBun
    {
        Dark = 1,
        Light = 2
    }

    public enum Topping
    {
        Ham = 1,
        Cheese = 2,
        Meatloaf = 3,
        Beef = 4,
        Tuna = 5
    }

    public enum Tomatoes
    {
        Yes = 1,
        No = 2
    }

    public enum Salad
    {
        Yes = 1,
        No = 2
    }

    public enum Sauce
    {
        Mayo = 1,
        Ketchup = 2,
        Mustard = 3,
        Nothing = 4
    }

    //Attribute used so that the bot is able to use this class!
    [Serializable]
    public class Sandwich
    {
        //[Prompt("How would you like your sandwich bun?")]
        [Prompt("What {&} would you like? {||}")]
        public TypeOfBun typeOfBun { get; set; }
        [Prompt("Please list all of the toppings that you would like. {||}")]
        public List<Topping> topping { get; set; }
        //[Prompt("Would you like tomatos with that?")]
        [Prompt("Would you like {&} with that? {||}")]
        public Tomatoes tomatoes { get; set; }
        //[Prompt("Would you like salad with that?")]
        [Prompt("Would you like {&} with that? {||}")]
        public Salad salad { get; set; }
        //[Prompt("What sauce do you want?")]
        [Prompt("Please list all of the sauces that you would like. {||}")]
        public List<Sauce> sauce { get; set; }
        [Prompt("How many of these do you want to order?")]
        public int Amount { get; set; }

        //Formulier
        public static IForm<Sandwich> BuildForm()
        {
            return new FormBuilder<Sandwich>()
                .Message("If something is unclear please type 'help'.")
                .OnCompletion(async (context, sandwich) =>
                {
                    string UsersId = context.Activity.From.Id;
                    MSTeamsBotDB.SaveSandwich(UsersId, sandwich, context);
                    await context.PostAsync("Sandwich ordered!");
                })
                .Build();
        }
    }
}