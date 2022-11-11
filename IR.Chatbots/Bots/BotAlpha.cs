using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.ML;
using IR.Chatbots.Common.Logging;
using IR.Chatbots.Engine.Session;
using IR.Chatbots.Engine.Test;
using IR.Chatbots.ML.Interfaces;
using IR.Chatbots.ML.Models;

namespace IR.Chatbots.Bots
{
    /// <summary>
    /// Bot Alpha.
    /// </summary>
    public class BotAlpha : IBot
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private static ILog log = LogHelper.GetLogger<BotAlpha>();


        public BotAlpha()
        {
        }

        /// <summary>
        /// Id used to access the bot configuration in DB.
        /// </summary>
        public static string Id => typeof(BotAlpha).FullName;

        /// <summary>
        /// Must define function imposed by the bot framework.
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            //new NeuralTrainingEngine().BuildAndSaveModel();
            //await BotDbTestClass.Feed(BotAlpha.Id, true); //Recreates DB in first request, Comment if you want to retain old data.
            //await BotDbTestClass.Feed(BotAlpha.Id, false, "Mobile"); //Recreates DB in first request, Comment if you want to retain old data.
            var requestState = await turnContext.GetOrCreateUserState(BotAlpha.Id);
            await requestState.HandleRequest(turnContext);
        }
    }

}


