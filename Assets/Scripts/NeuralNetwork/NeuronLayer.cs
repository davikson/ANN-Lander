using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeuronLayer
{
    public int numberOfNeurons;
    public List<Neuron> neurons;
    public NeuronLayer(int nNeurons, int nInputs)
    {
        numberOfNeurons = nNeurons;
        neurons = new List<Neuron>();
        for (int i = 0; i < numberOfNeurons; i++)
            neurons.Add(new Neuron(nInputs));
    }
}
