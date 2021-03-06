﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine_Learning.Neural_Network {

    public class Neuron {

        public Neuron[] prev;
        public WeightSet weights;
        public double val, activated, error;
        public int size;

        private Func<double, double> activation;
        private Func<double, double> derivative;

        public Neuron (int size, int type = Network.TANH) {
            this.size = size;
            this.prev = new Neuron[size];
            this.weights = new WeightSet(size);
            this.setActivation(type);
        }

        public Neuron (int size, WeightSet weights, int type = Network.TANH) {
            this.size = size;
            this.prev = new Neuron[size];
            this.weights = weights;
            this.setActivation(type);
        }

        public void forwardPropagate () {
            val = weights.evaluate(prev);
            activate();
        }

        public void backPropagateError (bool isOutput) {
            if (!isOutput)
                error *= getDerivative();
            weights.updateError(prev, error);
        }

        public void backPropagateRegularize (double learningRate) {
            weights.regularize(learningRate);
        }

        public void backPropagateWeights (double learningRate) {
            weights.update(prev, error, learningRate);
            error = 0;
        }

        public void activate () {
            activated = activation(val);
        }

        public double getDerivative () {
            return derivative(val);
        }

        public void link (Neuron[] prev) {
            for (int i = 0; i < size; i++)
                this.prev[i] = prev[i];
        }

        private double tanhActivation (double val) {
            return Math.Tanh(val);
        }

        private double tanhDerivative (double val) {
            return (1 - Math.Tanh(val) * Math.Tanh(val));
        }

        private double reluActivation (double val) {
            return val < 0 ? 0.01 * val : val;
        }

        private double reluDerivative (double val) {
            return val < 0 ? 0.01 : 1;
        }

        private double sigmoidActivation (double val) {
            return (1 / (1 + Math.Exp(-val)));
        }

        private double sigmoidDerivative (double val) {
            return ((1 / (1 + Math.Exp(-val))) * (1 - 1 / (1 + Math.Exp(-val))));
        }

        public double linearActivation (double val) {
            return val;
        }

        public double linearDerivative (double val) {
            return 1;
        }

        public double softmaxActivation (double val) {
            return Math.Exp(val);
        }

        public double softmaxDerivative (double val) {
            return 1;
        }

        private void setActivation (int type) {
            if (type == Network.TANH) {
                activation = tanhActivation;
                derivative = tanhDerivative;
            } else if (type == Network.RELU) {
                activation = reluActivation;
                derivative = reluDerivative;
            } else if (type == Network.SIGMOID) {
                activation = sigmoidActivation;
                derivative = sigmoidDerivative;
            } else if (type == Network.LINEAR) {
                activation = linearActivation;
                derivative = linearDerivative;
            } else if (type == Network.SOFTMAX) {
                activation = softmaxActivation;
                derivative = softmaxDerivative;
            }
        }
    }
}
