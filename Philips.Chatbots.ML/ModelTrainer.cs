using Newtonsoft.Json;
using Philips.Chatbots.ML.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Philips.Chatbots.ML
{
    public class ChitChatDataModel
    {
        public string q { get; set; }
        public string a { get; set; }
        public string a_id { get; set; }
    }

    public class Collection
    {
        public string Topic { get; set; }
        public string q_id { get; set; }
        public string q { get; set; }
        public string a { get; set; }
        public string a_id { get; set; }

    }
    public static class ModelTrainer
    {
        public static void Run()
        {
            //List<NeuralTrainInput> result = new List<NeuralTrainInput>();
            //DbTrainDataCollection.Find(exp => true).ToList()
            //    .ForEach(trainData => trainData.Dataset
            //    .ForEach(text => result.Add(new NeuralTrainInput { _id = trainData._id, Text = text })
            //    ));
            var chitChatInput = JsonConvert.DeserializeObject<List<ChitChatDataModel>>(File.ReadAllText("data/data_v1.json"));
            //chitChatInput.ForEach(x => x.a_id = Guid.NewGuid().ToString());
            //File.WriteAllText("data/data_v1.json", JsonConvert.SerializeObject(chitChatInput, Formatting.Indented));
            var trainData = chitChatInput.Select(x => new NeuralTrainInput { Text = x.q, _id = x.a_id }).ToList();
            var chitChatModelTrainer = new CustomTrainingEngine("ChitChat", trainData.ToList());
            chitChatModelTrainer.BuildAndSaveModel();

            //var reddit = JsonConvert.DeserializeObject<List<Collection>>(File.ReadAllText("data/collection.json"));
            //CreateModel("Education", reddit);
            //CreateModel("Politics", reddit);
            //CreateModel("Healthcare", reddit);
            //CreateModel("Technology", reddit);
            //CreateModel("Environment", reddit);
            //CreateModel("Any", reddit, true);
        }

        private static void CreateModel(string topic, List<Collection> reddit, bool any = false)
        {
            var trainData = reddit.Where(x => any || x.Topic == topic).Select(x => new NeuralTrainInput { Text = x.q, _id = x.a_id }).ToList();
            var modelTrainer = new CustomTrainingEngine(topic, trainData.Take(5000).ToList());
            modelTrainer.BuildAndSaveModel("data/" + modelTrainer.ModelOutputPath);
        }
    }
}
