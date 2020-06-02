using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace VanGogh
{

    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int INPUT_LAYERS = 10;
        NeuralNetwork network;
        double result;
        public MainWindow()
        {        
            InitializeComponent();
            int[] layers = { INPUT_LAYERS, 5, 1};
            network = new NeuralNetwork(layers);
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

            for(int i = 0; i < 500; i++)
            {
                double[] inputs = new double[INPUT_LAYERS];
                for(int j = 0; j < INPUT_LAYERS; j ++)
                {
                    inputs[j] = RandomNumber(0, 2);
                }
                network.FeedForward(inputs);
                if(inputs[0] == 1.0)
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
            double[] inputs = new double[INPUT_LAYERS];
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
            }
            network.FeedForward(inputs);
            result = network.getResult();
            resultLabel.Content = result;
        }
    }
}
