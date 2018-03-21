using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace pc.BudapestHotel
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else
            {
                await HandleSystemMessage(activity);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private async Task<Activity> HandleSystemMessage(Activity activity)
        {
            if (activity.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                var updateActivity = activity.AsConversationUpdateActivity();
                var botId = updateActivity.Recipient.Id;

                //remove bot from the members added
                updateActivity.MembersAdded = updateActivity.MembersAdded.Where(m => m.Id != botId).ToList();

                foreach (var member in updateActivity.MembersAdded)
                    await Conversation.SendAsync(activity, () => new Dialogs.RootDialog());
            }
            else if (activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (activity.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (activity.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
}
}