using System;
using System.Linq;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using pc.BudapestHotel.Forms;
using pc.BudapestHotel.Interfaces;
using pc.BudapestHotel.Parsers;
using pc.BudapestHotel.ExtensionMethods;
namespace pc.BudapestHotel.Dialogs
{
    [Serializable]
    [LuisModel("<ModelId>", "<subscriptionId>")]
    public class RootDialog : LuisDialog<object>
    {
        #region Intents
        const string BLANK = "";
        const string NONE = "none";
        const string BOOKROOM = "BookRoom";
        #endregion

        IEntityParser ChainOfCommand = new DateRangeParser().AddParsers(new IntegerParser());
        
        public override Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageRecievedAsync);
            return Task.CompletedTask;
        }

        private Task MessageRecievedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var descriptions = new[] { "Book a (room)", "Reserve a (table)", "Check (out)", "Ask the (weather)", "(Feedback)", "Request a (cab)", "(Special) request", "Ask a (question)" };
            var options = new[] { "room", "table", "checkout", "weather", "feedback", "cab", "special", "question" };

            var promptOptions = new PromptOptions<string>(
                prompt: "What may I help you with?",
                options: options,
                retry: String.Empty,
                attempts: 0,

                descriptions: descriptions);

            PromptDialog.Choice(context, OptionMadeAsync, promptOptions, minScore: 0);
            return Task.CompletedTask;
        }

        private async Task OptionMadeAsync(IDialogContext context, IAwaitable<object> result)
        {
            switch (context.Activity.AsMessageActivity().Text)
            {
                case "room":
                    await BookRoom(context, null);
                    return;
                case "table":
                case "checkout":
                case "weather":
                case "feedback":
                case "taxi":
                case "special":
                case "question":
                    break;
                default:
                    var message = context.MakeMessage();
                    message.Text = context.Activity.AsMessageActivity().Text;
                    await base.MessageReceived(context, Awaitable.FromItem(message));
                    break;
            }
        }

        [LuisIntent(BLANK)]
        [LuisIntent(NONE)]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var DidNotUnderstand_Text = $"I'm sorry, i didn't understand \"{result.Query}\"";
            await context.PostAsync(DidNotUnderstand_Text);

            context.Wait(base.MessageReceived);
        }

        [LuisIntent(BOOKROOM)]
        public async Task BookRoom(IDialogContext context, LuisResult result)
        {
            IList<object> Values = new List<object>();
            foreach(var e in result.Entities)
            {
                var entity = ChainOfCommand.Parse(e);
                if (entity != null)
                    Values.Add(entity);
            }

            var form = new FormDialog<IRoomReservation>(new RoomReservationForm(Values.ToArray()), RoomReservationForm.BuildForm);
            await context.Forward(form, RoomReservationCallBackAsync,null);
        }

        private async Task RoomReservationCallBackAsync(IDialogContext context, IAwaitable<IRoomReservation> result)
        {
            try
            {
                var RoomReservation = await result;
                var chkIn = RoomReservation.CheckInDate.Value.ToShortDateString();
                var chkOut = RoomReservation.CheckOutDate.Value.ToShortDateString();
                var msg = $"thanks for booking a room from **{chkIn}** to **{chkOut}**";

                await context.PostAsync(msg);
            }
            finally
            {

                await this.MessageRecievedAsync(context, null);
            }
        }
    }
}