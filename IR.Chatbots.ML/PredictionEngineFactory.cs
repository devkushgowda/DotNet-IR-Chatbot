using Newtonsoft.Json;
using IR.Chatbots.ML.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IR.Chatbots.ML
{
    public static class PredictionEngineFactory
    {
        private static readonly Dictionary<string, NeuralPredictionEngine> predictionEngines = new Dictionary<string, NeuralPredictionEngine>();

        private static List<ChitChatDataModel> chitChatData = JsonConvert.DeserializeObject<List<ChitChatDataModel>>(File.ReadAllText("data/data_v1.json")).Take(5000).ToList();

        private static List<Collection> redditData = JsonConvert.DeserializeObject<List<Collection>>(File.ReadAllText("data/collection.json"));

        public static void Init(string key, string path)
        {
            if (!predictionEngines.ContainsKey(key))
            {
                var predictionEngine = new NeuralPredictionEngine(path, true);
                predictionEngines[key] = predictionEngine;
            }
        }

        public static string Predict(string key, string input)
        {
            if (predictionEngines.ContainsKey(key))
            {
                var predictionEngine = predictionEngines[key];
                var output = predictionEngine.Predict(new NeuralTrainInput { Text = input })._id;
                return Process(key, output);
            }
            return null;
        }

        public static string Process(string key, string output)
        {
            switch (key)
            {
                case "Chit-Chat":
                    return chitChatData.First(x => x.a_id == output).a;
                default:
                    return redditData.First(x => x.a_id == output).a;
            }
        }
    }
}
