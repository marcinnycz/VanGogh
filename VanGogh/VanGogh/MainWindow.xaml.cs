using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VanGogh
{

    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int IMAGE_SIZE = 256;
        const int INPUT_LAYERS = IMAGE_SIZE * IMAGE_SIZE * 3;
        NeuralNetwork network;
        double result;
        Bitmap starryNight;
        Bitmap notStarry;

        public MainWindow()
        {
            InitializeComponent();
            int[] layers = { INPUT_LAYERS, 75, 1 };
            network = new NeuralNetwork(layers);

           

            //starryNight = new Bitmap("G:/VanGogh/red.jpg");
            //notStarry = new Bitmap("G:/VanGogh/blue.jpg");

            //starryNight = new Bitmap("G:/VanGogh/red.jpg");
            //notStarry = new Bitmap("G:/VanGogh/blue.jpg");
            starryNight = new Bitmap("G:/VanGogh/starrynight.jpg");
            notStarry = new Bitmap("G:/VanGogh/notstarry/mona.bmp");
        }

        public byte[] randomPart(Bitmap source)
        {

            Random rnd = new Random();
            System.Drawing.Rectangle cropRect = new System.Drawing.Rectangle(rnd.Next(0, source.Width - IMAGE_SIZE), rnd.Next(0, source.Height - IMAGE_SIZE), IMAGE_SIZE, IMAGE_SIZE);
            Bitmap imgPart = source.Clone(cropRect, source.PixelFormat);
            BitmapSource src = CreateBitmapSourceFromBitmap(imgPart);
            Bitmap testBmp = new Bitmap(IMAGE_SIZE, IMAGE_SIZE);
           
            byte[] arr = new byte[IMAGE_SIZE * IMAGE_SIZE * 3];
            for (int y = 0; y < IMAGE_SIZE; y++)
            { 
                for (int x = 0; x < IMAGE_SIZE; x++)
                {

                    System.Drawing.Color color = imgPart.GetPixel(x, y);

                    arr[y * IMAGE_SIZE * 3 + x * 3] = color.R;
                    arr[y * IMAGE_SIZE * 3 + x * 3 + 1] = color.G;
                    arr[y * IMAGE_SIZE * 3 + x * 3 + 2] = color.B;

                       
                }
            }
            for (int y = 0; y < IMAGE_SIZE; y++)
            {
                for (int x = 0; x < IMAGE_SIZE; x++)
                {
                    byte r = arr[y * IMAGE_SIZE * 3 + x * 3];
                    byte g = arr[y * IMAGE_SIZE * 3 + x * 3 + 1];
                    byte b = arr[y * IMAGE_SIZE * 3 + x * 3 + 2];
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(255, r, g, b);

                    testBmp.SetPixel(x, y, color);
                }
            }

            neuronImage.Source = CreateBitmapSourceFromBitmap(testBmp);
            previewImage.Source = src;
            return arr;
        }

        public static BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }


        // Generate a random number between two numbers  
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            Thread.Sleep(2);
            return random.Next(min, max);
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {

            /*for(int i = 0; i < 20000; i++)
            {
                double[] inputs = new double[INPUT_LAYERS];
                int sum = 0;
                for(int j = 0; j < INPUT_LAYERS; j ++)
                {
                    inputs[j] = RandomNumber(0, 2);
                    sum += (int)inputs[j];
                }
                network.FeedForward(inputs);
                if(sum > 4)
                {
                    network.Backprop(1.0);
                }
                else 
                {
                    network.Backprop(0.0);
                }
                result = network.getResult();
                resultLabel.Content = result;
            }*/
            Random rnd = new Random();
            for (int i = 0; i < 250; i++)
            {
                double[] inputs = new double[INPUT_LAYERS];
                byte[] inputImage = new byte[IMAGE_SIZE * IMAGE_SIZE * 3];
                int correct = rnd.Next(0, 10);
                if (correct > 4)
                {
                    //string path = "G:/VanGogh/o/" + rnd.Next(1, 11) + ".png" ;
                    //inputImage = randomPart(new Bitmap(path));
                    inputImage = randomPart(starryNight);
                }
                else
                {
                    //string path = "G:/VanGogh/x/" + rnd.Next(1, 11) + ".png";
                    //inputImage = randomPart(new Bitmap(path));
                    inputImage = randomPart(notStarry);
                }
                for (int j = 0; j < INPUT_LAYERS; j++)
                {
                    inputs[j] = inputImage[j]/255.0;
                }
                network.FeedForward(inputs);
                if (correct > 3)
                {
                    network.Backprop(1.0);
                }
                else
                {
                    network.Backprop(0.0);
                }
                result = network.getResult();
                resultLabel.Content = result;
            }

        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            /*double[] inputs = new double[INPUT_LAYERS];
            if (CB1.IsChecked == true)
            {
                inputs[0] = 1;
            }
            if (CB2.IsChecked == true)
            {
                inputs[1] = 1;
            }
            if (CB3.IsChecked == true)
            {
                inputs[2] = 1;
            }
            if (CB4.IsChecked == true)
            {
                inputs[3] = 1;
            }
            if (CB5.IsChecked == true)
            {
                inputs[4] = 1;
            }
            if (CB6.IsChecked == true)
            {
                inputs[5] = 1;
            }
            if (CB7.IsChecked == true)
            {
                inputs[6] = 1;
            }
            if (CB8.IsChecked == true)
            {
                inputs[7] = 1;
            }
            if (CB9.IsChecked == true)
            {
                inputs[8] = 1;
            }
            if (CB10.IsChecked == true)
            {
                inputs[9] = 1;
            }*/

            double[] inputs = new double[INPUT_LAYERS];
            byte[] inputImage = new byte[IMAGE_SIZE * IMAGE_SIZE * 3];
            Random rnd = new Random();
            int correct = rnd.Next(0, 11);
            if (correct > 3)
            {
                //string path = "G:/VanGogh/o/" + rnd.Next(1, 11) + ".png";
                //inputImage = randomPart(new Bitmap(path));
                inputImage = randomPart(starryNight);
            }
            else
            {
                //string path = "G:/VanGogh/x/" + rnd.Next(1, 11) + ".png";
                //inputImage = randomPart(new Bitmap(path));
                inputImage = randomPart(notStarry);
            }
            for (int j = 0; j < INPUT_LAYERS; j++)
            {
                inputs[j] = inputImage[j] / 255.0;
            }
            network.FeedForward(inputs);
           

            network.FeedForward(inputs);
            result = network.getResult();
            resultLabel.Content = result;
        }

        private void nextImgButton_Click(object sender, RoutedEventArgs e)
        {
            randomPart(starryNight);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            network.Save("neuralsave.txt");
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            network.Load("neuralsave.txt");
        }
    }
}
