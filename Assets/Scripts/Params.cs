using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml.Serialization;

public static class Params
{
    public static int numberOfInputs = 6;
    public static int numberOfOutputs = 2;
    public static int numberOfHiddenLayer = 1;
    public static int numberOfNeuronsPerHiddenLayer = 10;
    public static int populationSize = 100;
    public static int chromosomLenght = 120;
    public static float mutationRate = 0.05f;
    public static float crossoverRate = 0.7f;
    public static float maxPerturbation = 0.3f;
    public static float bias = -1;
    public static float response = 1;
    public static float simulationTime = 20;
    public static void Save()
    {
        ParamsNonStatic tmp = new ParamsNonStatic();

        XmlSerializer xmls = new XmlSerializer(typeof (ParamsNonStatic));
        FileStream file = File.Open(Application.dataPath + "/Params.ini", FileMode.OpenOrCreate);
        xmls.Serialize(file, tmp);
        file.Close();
    }
    public static void Load()
    {
        if (File.Exists(Application.dataPath + "/Params.ini"))
        {
            ParamsNonStatic tmp;
            XmlSerializer xmls = new XmlSerializer(typeof(ParamsNonStatic));
            FileStream file = File.Open(Application.dataPath + "/Params.ini", FileMode.Open);
            tmp = (ParamsNonStatic)xmls.Deserialize(file);
            file.Close();
            Debug.Log("Params loaded from: " + Application.dataPath + "/Params.ini");

            numberOfInputs = tmp.numberOfInputs;
            numberOfOutputs = tmp.numberOfOutputs;
            numberOfHiddenLayer = tmp.numberOfHiddenLayer;
            numberOfNeuronsPerHiddenLayer = tmp.numberOfNeuronsPerHiddenLayer;
            populationSize = tmp.populationSize;
            chromosomLenght = tmp.chromosomLenght;
            mutationRate = tmp.mutationRate;
            crossoverRate = tmp.crossoverRate;
            maxPerturbation = tmp.maxPerturbation;
            bias = tmp.bias;
            response = tmp.response;
            simulationTime = tmp.simulationTime;
        }
        else
        {
            Save();
        }
    }
}
