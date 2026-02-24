using Microsoft.ML;
using Microsoft.ML.Data;
using TransferLearningTF;


/*
 * You will build a classification model in ML.NET to categorize images by using a pretrained TensorFlow for image processing
 */

string _assetsPath = Path.Combine(Environment.CurrentDirectory, "assets");
string _imagesFolder = Path.Combine(_assetsPath, "images");
string _trainTagsTsv = Path.Combine(_imagesFolder, "tags.tsv");
string _testTagsTsv = Path.Combine(_imagesFolder, "test-tags.tsv");
string _predictSingleImage = Path.Combine(_imagesFolder, "toaster3.jpg");
string _inceptionTensorFlowModel = Path.Combine(_assetsPath, "inception", "tensorflow_inception_graph.pb");



MLContext mlContext = new MLContext();

ImageModel imageModel = new ImageModel();
ITransformer model = imageModel.GenerateModel(mlContext, _imagesFolder,_inceptionTensorFlowModel, _trainTagsTsv, _testTagsTsv);
imageModel.ClassifySingleImage(mlContext, model, _predictSingleImage);