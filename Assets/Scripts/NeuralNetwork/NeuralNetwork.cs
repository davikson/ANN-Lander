using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NeuralNetwork : MonoBehaviour
{
    protected int numberOfInputs;
    protected int numberOfOutputs;
    protected int numberOfHiddenLayer;
    protected int numberOfNeuronsPerHiddenLayer;
    protected List<NeuronLayer> neuronLayers;
    protected float[] inputs;
    protected float[] outputs;
    public NeuralNetwork()
    {
        CreateNetwork();
    }
    public void CreateNetwork()
    {
        neuronLayers = new List<NeuronLayer>();
        if (numberOfHiddenLayer > 0)
        {
            neuronLayers.Add(new NeuronLayer(numberOfNeuronsPerHiddenLayer, numberOfInputs));

            for (int i = 0; i < numberOfHiddenLayer - 1; i++)
                neuronLayers.Add(new NeuronLayer(numberOfNeuronsPerHiddenLayer, numberOfNeuronsPerHiddenLayer));

            neuronLayers.Add(new NeuronLayer(numberOfOutputs, numberOfNeuronsPerHiddenLayer));
        }
        else
        {
            neuronLayers.Add(new NeuronLayer(numberOfOutputs, numberOfInputs));
        }
    }
    public List<float> GetWeights()
    {
        List<float> weights = new List<float>();
        for (int i = 0; i < numberOfHiddenLayer + 1; i++)
            for (int j = 0; j < neuronLayers[i].numberOfNeurons; j++)
                for (int k = 0; k < neuronLayers[i].neurons[j].numberOfInputs; k++)
                    weights.Add(neuronLayers[i].neurons[j].weights[k]);
        return weights;
    }
    public void PutWeights(List<float> weights)
    {
        int cWeight = 0;
        for (int i = 0; i < numberOfHiddenLayer + 1; i++)
            for (int j = 0; j < neuronLayers[i].numberOfNeurons; j++)
                for (int k = 0; k < neuronLayers[i].neurons[j].numberOfInputs; k++)
                    neuronLayers[i].neurons[j].weights[k] = weights[cWeight++];
    }
    protected int GetNumberOfWeights()
    {
        int weights = 0;
        for (int i = 0; i < numberOfHiddenLayer + 1; i++)
            for (int j = 0; j < neuronLayers[i].numberOfNeurons; j++)
                for (int k = 0; k < neuronLayers[i].neurons[j].numberOfInputs; k++)
                    weights++;
        return weights;
    }
    protected virtual void CalculateOutputs()
    {
        int cWeight = 0;
        List<float> Inputs = new List<float>();
        List<float> Outputs = new List<float>();
        foreach (float f in inputs)
            Inputs.Add(f);
        for(int i = 0; i < numberOfHiddenLayer; i++)
        {
            if (i > 0)
                Inputs = Outputs;
            Outputs.Clear();
            cWeight = 0;

            for(int j = 0; j < neuronLayers[i].numberOfNeurons; j++)
            {
                float netInput = 0;
                int nOfInputs = neuronLayers[i].neurons[j].numberOfInputs;
                for(int k = 0; k < nOfInputs - 1; k++)
                {
                    netInput += neuronLayers[i].neurons[j].weights[k] * Inputs[cWeight++];
                }
                netInput += neuronLayers[i].neurons[j].weights[nOfInputs - 1] * Params.bias;
                Outputs.Add(Sigmoid(netInput, Params.response));
                cWeight = 0;
            }
        }
        for (int i = 0; i < numberOfOutputs; i++)
            outputs[i] = Outputs[i] - 0.5f;
    }
    protected float Sigmoid(float netInput, float response)
    {
        return 1/(1+Mathf.Exp(-netInput/ response));
    }
}
