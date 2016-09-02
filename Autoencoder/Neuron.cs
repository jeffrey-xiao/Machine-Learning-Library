﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine_Learning.Autoencoder {
    public class Neuron {
        public const double SPARSITY_ESTIMATION = 0.999;

        public Neuron[] prev;
        public WeightSet weights;
        public double val, activated, error, sparsity;
        public int size;

        private Func<double, double> activation;
        private Func<double, double> derivative;

        public Neuron (int size) {
            this.size = size;
            this.prev = new Neuron[size];
            this.weights = new WeightSet(size);
            this.setActivation();
        }

        public Neuron (int size, WeightSet weights) {
            this.size = size;
            this.prev = new Neuron[size];
            this.weights = weights;
            this.setActivation();
        }

        public void forwardPropagate () {
            val = weights.evaluate(prev);
            activate();
            sparsity = SPARSITY_ESTIMATION * sparsity + (1 - SPARSITY_ESTIMATION) * activated;
        }

        public void backPropagateError (bool isOutput) {
            if (!isOutput) {
                double b = Autoencoder.SPARSITY_COST;
                double p = Autoencoder.SPARSITY_TARGET;
                double sparsityGrad = b * (-p / sparsity + (1 - p) / (1 - sparsity));
                error = (error + sparsityGrad) * getDerivative();
            }
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
        
        private double sigmoidActivation (double val) {
            return (1 / (1 + Math.Exp(-val)));
        }

        private double sigmoidDerivative (double val) {
            return ((1 / (1 + Math.Exp(-val))) * (1 - 1 / (1 + Math.Exp(-val))));
        }

        private void setActivation () {
            activation = sigmoidActivation;
            derivative = sigmoidDerivative;
        }
    }
}