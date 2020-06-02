using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VanGogh
{
    class NeuralNetwork
    {
        private int[] layers;
        private double[][] neurons;//neurons    
        private double[][] biases;//biasses    
        private double[][][] weights;//weights 
        private double[][] neuronChange;
        private double[][] biasesChange;
        private double[][][] weightsChange;
        private double cost = 0;
        private double learningRate = 0.4;
        //private int[] activations;//layers

        //public float fitness = 0;//fitness

        public NeuralNetwork(int[] layers)
        {
            this.layers = new int[layers.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                this.layers[i] = layers[i];
            }
            InitNeurons();
            InitBiases();
            InitWeights();
        }

        //create empty storage array for the neurons in the networ-k.
        private void InitNeurons()
        {
            List<double[]> neuronsList = new List<double[]>();
            List<double[]> neuronsChangeList = new List<double[]>();
            for (int i = 0; i < layers.Length; i++)
            {
                neuronsList.Add(new double[layers[i]]);
                neuronsChangeList.Add(new double[layers[i]]);
            }
            neurons = neuronsList.ToArray();
            neuronChange = neuronsChangeList.ToArray();
        }

        //initializes and populates array for the biases being held within the network.
        private void InitBiases()
        {
            Random r = new Random();
            List<double[]> biasList = new List<double[]>();
            List<double[]> biasChangeList = new List<double[]>();
            for (int i = 0; i < layers.Length; i++)
            {
                double[] bias = new double[layers[i]];
                for (int j = 0; j < layers[i]; j++)
                {
                    bias[j] = r.NextDouble() - 0.5;
                }
                biasList.Add(bias);
                biasChangeList.Add(new double[layers[i]]);
            }
            biases = biasList.ToArray();
            biasesChange = biasChangeList.ToArray();
        }

        //initializes random array for the weights being held in the network.
        private void InitWeights()
        {
            Random r = new Random();
            List<double[][]> weightsList = new List<double[][]>();
            List<double[][]> weightsChangeList = new List<double[][]>();
            for (int i = 1; i < layers.Length; i++)
            {
                List<double[]> layerWeightsList = new List<double[]>();
                List<double[]> layerWeightsChangeList = new List<double[]>();
                int neuronsInPreviousLayer = layers[i - 1];
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    double[] neuronWeights = new double[neuronsInPreviousLayer];
                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                    {
                        neuronWeights[k] = r.NextDouble() - 0.5;
                    }
                    layerWeightsList.Add(neuronWeights);
                    layerWeightsChangeList.Add(new double[neuronsInPreviousLayer]);
                }
                weightsList.Add(layerWeightsList.ToArray());
                weightsChangeList.Add(layerWeightsChangeList.ToArray());
            }
            weights = weightsList.ToArray();
            weightsChange = weightsChangeList.ToArray();
        }

        //feed forward, inputs >==> outputs.
        public double[] FeedForward(double[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                neurons[0][i] = inputs[i];
            }
            for (int i = 1; i < layers.Length; i++)
            {
                int layer = i - 1;
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    double value = 0f;
                    for (int k = 0; k < neurons[i - 1].Length; k++)
                    {
                        value += weights[i - 1][j][k] * neurons[i - 1][k];
                    }
                    neurons[i][j] = activate(value + biases[i][j]);
                }
            }
            return neurons[neurons.Length - 1];
        }

        public double activate(double value)
        {
            return sigmoid(value);
        }

        public double sigmoid(double value)
        {
            return 1 / (1 + (Math.Pow(Math.E, -value)));
        }

        public double sigmoidDerivative(double value)
        {
            return sigmoid(value)*(1-sigmoid(value));
        }

        /*
        public void setInputLayer(double[] inputs)
        {
            if(inputs.Length == layers[0])
            {
                for (int i = 0; i < layers[0]; i++)
                {
                    neurons[0][i] = inputs[i];
                }
            }        
        }*/

        public double getResult()
        {
            return neurons[layers.Length - 1][0];
        }

        //Comparing For NeuralNetworks performance.
        /*public int CompareTo(NeuralNetwork other)
        {
            if (other == null)
                return 1;
            if (fitness > other.fitness)
                return 1;
            else if (fitness < other.fitness)
                return -1;
            else
                return 0;
        }*/

        //this loads the biases and weights from within a file into the neural network.
        public void Load(string path)
        {
            TextReader tr = new StreamReader(path);
            int NumberOfLines = (int)new FileInfo(path).Length;
            string[] ListLines = new string[NumberOfLines];
            int index = 1;
            for (int i = 1; i < NumberOfLines; i++)
            {
                ListLines[i] = tr.ReadLine();
            }
            tr.Close();
            if (new FileInfo(path).Length > 0)
            {
                for (int i = 0; i < biases.Length; i++)
                {
                    for (int j = 0; j < biases[i].Length; j++)
                    {
                        biases[i][j] = float.Parse(ListLines[index]);
                        index++;
                    }
                }
                for (int i = 0; i < weights.Length; i++)
                {
                    for (int j = 0; j < weights[i].Length; j++)
                    {
                        for (int k = 0; k < weights[i][j].Length; k++)
                        {
                            weights[i][j][k] = float.Parse(ListLines[index]);
                            index++;
                        }
                    }
                }
            }
        }

        public void Backprop(double expected)
        {            
            neuronChange[layers.Length - 1][0] = neurons[layers.Length - 1][0] - expected;
            cost = Math.Pow((expected - neurons[layers.Length - 1][0]), 2);
            for(int layer = layers.Length -1; layer > 0; layer--)
            {
                for(int neuron = 0; neuron < layers[layer]; neuron++)
                {
                    for(int previousNeuron = 0; previousNeuron < layers[layer - 1]; previousNeuron++)
                    {
                        double al1 = neurons[layer - 1][previousNeuron];
                        double al = neurons[layer][neuron];
                        double zl = weights[layer - 1][neuron][previousNeuron] * al1 + biases[layer][neuron];
                        weightsChange[layer - 1][neuron][previousNeuron] += al1 * sigmoidDerivative(zl) * 2 * (neuronChange[layer][neuron]);
                        biasesChange[layer][neuron] += sigmoidDerivative(zl) * 2 * (al - expected);
                        neuronChange[layer - 1][previousNeuron] += weights[layer - 1][neuron][previousNeuron] * sigmoidDerivative(zl) * 2 * (neuronChange[layer][neuron]);
                    }
                    
                }
            }
            for(int i = 0; i < layers.Length - 1; i++)
            {
                for (int j = 0; j < layers[i+1]; j++)
                {
                    for (int k = 0; k < layers[i]; k++)
                    {
                        weights[i][j][k] -= weightsChange[i][j][k] * learningRate;
                        weightsChange[i][j][k] = 0;
                    }
                }
            }
            for (int i = 0; i < layers.Length - 1; i++)
            {
                for (int j = 0; j < layers[i]; j++)
                {
                    biases[i][j] -= biasesChange[i][j] * learningRate;
                    biasesChange[i][j] = 0;
                    neuronChange[i][j] = 0;
                }
            }
        }

        public double[] getInputs()
        {
            return neurons[0];
        }

        /*
        //used as a simple mutation function for any genetic implementations.
        public void Mutate(int chance, float val)
        {
            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    biases[i][j] = (UnityEngine.Random.Range(0f, chance) <= 5) ? biases[i][j] += UnityEngine.Random.Range(-val, val) : biases[i][j];
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = (UnityEngine.Random.Range(0f, chance) <= 5) ? weights[i][j][k] += UnityEngine.Random.Range(-val, val) : weights[i][j][k];

                    }
                }
            }
        }*/

    }
}
