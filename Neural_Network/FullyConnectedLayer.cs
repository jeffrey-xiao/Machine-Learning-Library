﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine_Learning.Neural_Network {

    public class FullyConnectedLayer : Layer1D {

        public FullyConnectedLayer (int size, int type) {
            this.size = size;
            this.type = type;
            this.neurons = new Neuron[size];
        }

        public FullyConnectedLayer (StreamReader reader, Layer prev) {
            string[] data = reader.ReadLine().Split();
            this.size = int.Parse(data[0]);
            this.type = int.Parse(data[1]);

            this.neurons = new Neuron[size];

            BindTo(ref prev);

            for (int i = 0; i < size; i++) {
                string[] input = reader.ReadLine().Split();
                neurons[i].weights.bias = double.Parse(input[0]);
                for (int j = 0; j < neurons[i].weights.val.GetLength(0); j++)
                    neurons[i].weights.val[j] = double.Parse(input[j + 1]);
            }
        }

        public override void BindTo (ref Layer layer) {
            prevLayer = layer;

            for (int i = 0; i < size; i++) {
                neurons[i] = new Neuron(prevLayer.size, this.type);
                neurons[i].link(prevLayer.neurons);
            }
        }


        public override void forwardPropagate () {
            for (int i = 0; i < size; i++)
                neurons[i].forwardPropagate();
        }

        public override void backPropagate (double learningRate) {
            for (int i = 0; i < size; i++)
                neurons[i].backPropagateError(false);

            for (int i = 0; i < size; i++) {
                neurons[i].backPropagateRegularize(learningRate);
                neurons[i].backPropagateWeights(learningRate);
            }
        }

        public override String ToString () {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("{0}\n{1} {2}", this.GetType().FullName, size, type));
            for (int i = 0; i < size; i++) {
                sb.Append("\n" + neurons[i].weights.bias);
                for (int j = 0; j < neurons[i].weights.val.GetLength(0); j++)
                    sb.Append(" " + neurons[i].weights.val[j]);
            }
            return sb.ToString();
        }
    }
}
