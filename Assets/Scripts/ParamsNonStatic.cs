using UnityEngine;
using System.Collections;

[System.Serializable]
public class ParamsNonStatic
{
    public int numberOfInputs = 6;
    public int numberOfOutputs = 2;
    public int numberOfHiddenLayer = 1;
    public int numberOfNeuronsPerHiddenLayer = 10;
    public int populationSize = 100;
    public int chromosomLenght = 120;
    public float mutationRate = 0.05f;
    public float crossoverRate = 0.7f;
    public float maxPerturbation = 0.3f;
    public float bias = -1;
    public float response = 1;
    public float simulationTime = 20;
    public ParamsNonStatic()
    {
        numberOfInputs = Params.numberOfInputs;
        numberOfOutputs = Params.numberOfOutputs;
        numberOfHiddenLayer = Params.numberOfHiddenLayer;
        numberOfNeuronsPerHiddenLayer = Params.numberOfNeuronsPerHiddenLayer;
        populationSize = Params.populationSize;
        chromosomLenght = Params.chromosomLenght;
        mutationRate = Params.mutationRate;
        crossoverRate = Params.crossoverRate;
        maxPerturbation = Params.maxPerturbation;
        bias = Params.bias;
        response = Params.response;
        simulationTime = Params.simulationTime;
    }
}
