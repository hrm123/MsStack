using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransferLearningTF
{
    public class ImageModel
    {

        public void DisplayResults(IEnumerable<ImagePrediction> imagePredictionData)
        {
            foreach (ImagePrediction prediction in imagePredictionData)
            {
                Console.WriteLine($"Image: {Path.GetFileName(prediction.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score?.Max()} ");
            }
        }

        public void ClassifySingleImage(MLContext mlContext, ITransformer model, string predictSingleImage)
        {
            var imageData = new ImageData()
            {
                ImagePath = predictSingleImage
            };

            // Make prediction function (input = ImageData, output = ImagePrediction)
            /*
             * The PredictionEngine is a convenience API, which allows you to perform a prediction on a single instance of data. PredictionEngine is not thread-safe. 
             * It's acceptable to use in single-threaded or prototype environments. For improved performance and thread safety in production environments, use the 
             * PredictionEnginePool service, which creates an ObjectPool of PredictionEngine objects for use throughout your application.
             * */
            var predictor = mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
            var prediction = predictor.Predict(imageData);
            Console.WriteLine($"Image: {Path.GetFileName(imageData.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score?.Max()} ");
        }

        /// <summary>
        /// Construct the ML.NET model pipeline - An ML.NET model pipeline is a chain of estimators. No execution happens during pipeline construction. The estimator 
        /// objects are created but not executed.  This method creates a pipeline for the model, and trains the pipeline to produce the ML.NET model. 
        /// It also evaluates the model against some previously unseen test data.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="imagesFolder"></param>
        /// <param name="inceptionTensorFlowModel"></param>
        /// <param name="trainTagsTsv"></param>
        /// <param name="testTagsTsv"></param>
        /// <returns></returns>
        public ITransformer GenerateModel(MLContext mlContext, string imagesFolder, string inceptionTensorFlowModel, string trainTagsTsv, string testTagsTsv)
        {
            IEstimator<ITransformer> pipeline = 
                mlContext.Transforms
                // load the images from the folder and the paths specified in the ImageData class. The LoadImages transform creates a new column "input" with the image data in byte[] format.
                .LoadImages(outputColumnName: "input", imageFolder: imagesFolder, inputColumnName: nameof(ImageData.ImagePath))
                // The image transforms transform the images into the model's expected format.
                .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight, inputColumnName: "input"))
                // extract pixel data into numeric vectors and apply mean offset and scale (if any) as expected by the model. The ExtractPixels transform creates a new column "input" with the image data in float[] format.
                .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean))
                // Load TrensorFlow model into memory. ScoreTensorFlowModel applies the loaded TensorFlow model to the input data. The output of this
                // transform is a new column "softmax2_pre_activation" with the output from the model in float[] format.
                .Append(mlContext.Model.LoadTensorFlowModel(inceptionTensorFlowModel)
                .ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { "input" }, addBatchDimensionInput: true))
                .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: "Label"))
                .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "softmax2_pre_activation"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel"))
                .AppendCacheCheckpoint(mlContext);
            IDataView trainingData = mlContext.Data.LoadFromTextFile<ImageData>(path: trainTagsTsv, hasHeader: false);
            ITransformer model = pipeline.Fit(trainingData);

            IDataView testData = mlContext.Data.LoadFromTextFile<ImageData>(path: testTagsTsv, hasHeader: false);
            IDataView predictions = model.Transform(testData);

            // Create an IEnumerable for the predictions for displaying results
            IEnumerable<ImagePrediction> imagePredictionData = mlContext.Data.CreateEnumerable<ImagePrediction>(predictions, true);
            DisplayResults(imagePredictionData);

            MulticlassClassificationMetrics metrics =
            mlContext.MulticlassClassification.Evaluate(predictions,
                labelColumnName: "LabelKey",
                predictedLabelColumnName: "PredictedLabel");
            Console.WriteLine($"LogLoss is: {metrics.LogLoss}");
            Console.WriteLine($"PerClassLogLoss is: {String.Join(" , ", metrics.PerClassLogLoss.Select(c => c.ToString()))}");
            return model;
        }

    }
}
