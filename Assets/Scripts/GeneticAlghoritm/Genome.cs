using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Genome
{
    public List<float> weights;
    public float fitness;
    public Genome()
    {
        fitness = 0;
        weights = new List<float>();
    }
    public Genome(int nOfWeights)
    {
        weights = new List<float>();
        fitness = 0;
        for (int i = 0; i < nOfWeights; i++)
            weights.Add(Random.Range(-1.0f, 1.0f));
    }
    public Genome(List<float> w, float f)
    {
        weights = w;
        fitness = f;
    }
    static public bool operator < (Genome lhs, Genome rhs)
    {
        return (lhs.fitness < rhs.fitness);
    }
    static public bool operator > (Genome lhs, Genome rhs)
    {
        return (lhs.fitness > rhs.fitness);
    }
}
