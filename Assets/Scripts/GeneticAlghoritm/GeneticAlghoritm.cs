using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;

public class GeneticAlghoritm : MonoBehaviour
{
    public GameObject subject;
    List<Genome> populationGenomes;
    List<GameObject> subjects;
    public int populationSize;
    int chromosomLength;
    float mutationRate;
    float crossoverRate;
    public float totalFitness;
    public float bestFitness;
    public float averageFitness;
    public float worstFitness;
    public int generationCount;
    public float simulationTime;
    public float time;
    public static GeneticAlghoritm instance;
    bool readed;
    void Awake()
    {
        Params.Load();
        instance = this;
        populationGenomes = new List<Genome>();
        subjects = new List<GameObject>();
        populationSize = Params.populationSize;
        chromosomLength = Params.chromosomLenght;
        mutationRate = Params.mutationRate;
        crossoverRate = Params.crossoverRate;
        simulationTime = Params.simulationTime;
        LoadGenome();
        if (!readed)
            CreateFirstPopulation();
        CreatePopulation();
        time = 0;
    }
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (time > simulationTime)
            Epoch();
    }
    void OnApplicationQuit()
    {
        SaveGenomes();
    }
    Genome Crossover(Genome parent1, Genome parent2)
    {
        Genome child = new Genome();
        int count = 1;
        bool cross = false;
        for (int i = 0; i < chromosomLength; i++)
        {
            if (Random.Range(0.0f, 1.0f) < crossoverRate / count)
            {
                cross = (cross) ? false : true;
                count++;
            }
            if (cross)
                child.weights.Add(parent1.weights[i]);
            else
                child.weights.Add(parent2.weights[i]);
        }
        return child;
    }
    void Mutate(ref Genome individual)
    {
        for (int i = 0; i < chromosomLength; i++)
            if (Random.Range(0.0f, 1.0f) < mutationRate)
                individual.weights[i] = Mathf.Clamp(individual.weights[i] + Random.Range(-1.0f, 1.0f) * Params.maxPerturbation, -1.0f, 1.0f);
    }
    Genome GetChromosomeRoulette()
    {
        float slice = Random.Range(0.0f, totalFitness);
        Genome theChosenOne = null;
        float fitnessSoFar = 0;
        for(int i = 0; i < populationSize; i++)
        {
            fitnessSoFar += populationGenomes[i].fitness;
            if(fitnessSoFar >= slice)
            {
                theChosenOne = populationGenomes[i];
                break;
            }
        }
        return theChosenOne;
    }
    void CalculateFitnesses()
    {
        totalFitness = 0;
        float highestFitnessSoFar = 0;
        float lowestFitnessSoFar = 999999;

        for (int i = 0; i < populationSize; i++)
        {
            if (populationGenomes[i].fitness > highestFitnessSoFar)
                highestFitnessSoFar = populationGenomes[i].fitness;
            if (populationGenomes[i].fitness < lowestFitnessSoFar)
                lowestFitnessSoFar = populationGenomes[i].fitness;
            totalFitness += populationGenomes[i].fitness;
        }
        bestFitness = highestFitnessSoFar;
        worstFitness = lowestFitnessSoFar;
        averageFitness = totalFitness / populationSize;
    }
    void CalculateObjectsFitnesses()
    {
        for (int i = 0; i < populationSize; i++)
            subjects[i].GetComponentInChildren<LanderController>().CalculateFitness();
    }
    void Reset()
    {
        totalFitness = 0;
        bestFitness = 0;
        worstFitness = 999999;
        averageFitness = 0;
    }
    void Epoch()
    {
        Time.timeScale = 0;
        Reset();
        CalculateObjectsFitnesses();
        CopyGenomesFromSubjects();
        CalculateFitnesses();
        SaveStatistics();
        populationGenomes = populationGenomes.OrderBy(x => x.fitness).ToList();
        List<Genome> newPopulation = new List<Genome>();
        for(int i = 0; i < 5; i++)
            newPopulation.Add(populationGenomes.Last());
        while (newPopulation.Count() < populationSize)
        {
            Genome child = Crossover(GetChromosomeRoulette(), GetChromosomeRoulette());
            Mutate(ref child);

            newPopulation.Add(child);
        }
        populationGenomes.Clear();

        populationGenomes = newPopulation;
        DestroyOldPopulation();
        CreatePopulation();
        Debug.Log("Generation: " + generationCount + ". Best fitness: " + bestFitness + ". Worst fitness: " + worstFitness + ". Average fitness: " + averageFitness);
        generationCount++;
        time = 0;
        Time.timeScale = UI.actualTimeScale;
    }
    void CopyGenomesFromSubjects()
    {
        populationGenomes.Clear();
        for (int i = 0; i < populationSize; i++)
            populationGenomes.Add(subjects[i].GetComponentInChildren<LanderController>().chromosome);
    }
    void CreateFirstPopulation()
    {
        for(int i = 0; i < populationSize; i++)
            populationGenomes.Add(new Genome(chromosomLength));
    }
    void CreatePopulation()
    {
        subjects = new List<GameObject>();
        for(int i = 0; i < populationSize; i++)
        {
            GameObject inst = Instantiate(subject, new Vector3(i * 100, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
            inst.GetComponentInChildren<LanderController>().chromosome = populationGenomes[i];
            inst.GetComponentInChildren<LanderController>().CreateNetwork();
            inst.GetComponentInChildren<LanderController>().PutWeights(populationGenomes[i].weights);
            inst.name += " " + i;
            subjects.Add(inst);
        }
        time = 0;
    }
    void DestroyOldPopulation()
    {
        for (int i = 0; i < populationSize; i++)
            Destroy(subjects[i]);
    }
    void SaveStatistics()
    {
        using (StreamWriter file = new StreamWriter(Application.dataPath + "/statistics.txt", true))
            file.WriteLine(string.Format("{0:000}", generationCount) + "\t" + string.Format("{0:##00.0000}", bestFitness) + "\t" + string.Format("{0:##00.0000}", worstFitness) + "\t" + string.Format("{0:##00.0000}", averageFitness));
    }
    void SaveGenomes()
    {
        XmlSerializer xmls = new XmlSerializer(typeof(List<Genome>));
        FileStream file = File.Open(Application.dataPath + "/Genomes.dat", FileMode.OpenOrCreate);
        xmls.Serialize(file, populationGenomes);
        file.Close();
        Debug.Log("Genomes saved to: " + Application.dataPath + "/Genomes.dat");
    }
    void LoadGenome()
    {
        if (File.Exists(Application.dataPath + "/Genomes.dat"))
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<Genome>));
            FileStream file = File.Open(Application.dataPath + "/Genomes.dat", FileMode.Open);
            populationGenomes = (List<Genome>)xmls.Deserialize(file);
            file.Close();
            readed = true;
            Debug.Log("Genomes loaded from: " + Application.dataPath + "/Genomes.dat");
        }
        else
        {
            readed = false;
            Debug.Log("Couldn't load genomes");
        }
    }
}