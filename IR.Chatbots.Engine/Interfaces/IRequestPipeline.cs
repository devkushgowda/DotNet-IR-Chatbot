using Microsoft.Bot.Builder;
using IR.Chatbots.Engine.Session;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IR.Chatbots.Engine.Interfaces
{
    /// <summary>
    /// Pipeline response data template.
    /// </summary>
    public class PipelineResponse
    {
        /// <summary>
        /// Pipeline response.
        /// </summary>
        public ResponseType Result { get; set; } = ResponseType.Continue;

        /// <summary>
        /// Number of handlers executed the request,
        /// </summary>
        public int Count { get; set; }
    }

    /// <summary>
    /// Request pipeline interface with chain of responsibility mechanism.
    /// </summary>
    public interface IRequestPipeline
    {
        List<IRequestHandler> Pipeline { get; set; }

        Task<PipelineResponse> Execute(ITurnContext turnContext, RequestState requestState);
    }
}