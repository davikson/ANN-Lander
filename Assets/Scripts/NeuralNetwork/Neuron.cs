using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Neuron
{
    public int numberOfInputs;
    public List<float> weights;
    public Neuron(int n)
    {
        numberOfInputs = n;
        weights = new List<float>();
        for (int i = 0; i < numberOfInputs + 1; i++)
            weights.Add(Random.Range(-1.0f, 1.0f));
    }
}
