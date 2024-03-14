using Couch_or_chair;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace recognition_window
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "images|*.jpg;*.png;*.jpeg";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                var imageBytes = File.ReadAllBytes(dlg.FileName);
                MLModel1.ModelInput sampleData = new MLModel1.ModelInput()
                {
                    ImageSource = imageBytes,
                };

                // Make a single prediction on the sample data and print results.
                var predictionResult = MLModel1.Predict(sampleData);
                Console.WriteLine($"\n\nPredicted Label value: {predictionResult.PredictedLabel} \nPredicted Label scores: [{String.Join(",", predictionResult.Score)}]\n\n");
                string labelResult = predictionResult.PredictedLabel;
                string selectedFileName = dlg.FileName;
                FileNameLabel.Content = selectedFileName;
                if (predictionResult.Score[0] < 0.9 && labelResult == "chair" || predictionResult.Score[1] < 0.9 && labelResult == "couch")
                {
                    labelResult = "undefined";
                }
                prediction.Content = $"prediction: {labelResult}";
                accuracy.Content = $"accuracy: {String.Join(" - chair\n", predictionResult.Score)} - couch";
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                ImageViewer1.Source = bitmap;

            }
        }
    }
}
