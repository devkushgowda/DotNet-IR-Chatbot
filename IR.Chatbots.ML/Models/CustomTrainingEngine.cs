using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using IR.Chatbots.ML.Interfaces;
using System.Collections.Generic;

namespace IR.Chatbots.ML.Models
{
    public class CustomTrainingEngine : AbstractTrainModel<NeuralTrainInput, PredictionOutput>
    {

        public override string ModelOutputPath => $"{modelName}-model.zip";

        private string modelName;
        private List<NeuralTrainInput> neuralTrainInputs;
        public CustomTrainingEngine(string modelName, List<NeuralTrainInput> neuralTrainInputs)
        {
            this.modelName = modelName.ToLower();
            this.neuralTrainInputs = neuralTrainInputs;
        }

        /// <summary>
        /// Load data from DB.
        /// </summary>
        /// <returns></returns>
        public override List<NeuralTrainInput> LoadData()
        {
            return neuralTrainInputs;
        }

        /// <summary>
        /// Transform and build train pipeline.
        /// </summary>
        /// <returns></returns>
        public override EstimatorChain<KeyToValueMappingTransformer> TransformAndBuildPipeline()
        {
            var fText = $"{nameof(NeuralTrainInput.Text)}Featurized";
            var transformedData = _mlContext.Transforms.Text.FeaturizeText(inputColumnName: nameof(NeuralTrainInput.Text), outputColumnName: fText)
                .Append(_mlContext.Transforms.Concatenate("Features", fText));


            if (EnableCacheCheckpoint())
                transformedData.AppendCacheCheckpoint(_mlContext);   //Remove for large datasets.

            var processedData = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: nameof(NeuralTrainInput._id), outputColumnName: "Label")
                .Append(transformedData);

            var result = processedData.Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
         .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));  //Build pipeline

            return result;
        }
    }

}
