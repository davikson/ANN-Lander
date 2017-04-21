using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    int iter = 0;
    static public float actualTimeScale = 1.0f;

    void Update()
    {
        if (Input.GetKeyDown("["))
            iter = Mathf.Clamp(iter-1, 0, (Mathf.CeilToInt(GeneticAlghoritm.instance.populationSize / 25) - 1));
        else if (Input.GetKeyDown("]"))
            iter = Mathf.Clamp(iter+1, 0, (Mathf.CeilToInt(GeneticAlghoritm.instance.populationSize / 25) - 1));
        if (Input.GetKeyDown("p"))
            if (Time.timeScale == 0)
                Time.timeScale = actualTimeScale;
            else
                Time.timeScale = 0;
        else if(Input.GetKeyDown("0"))
        {
            actualTimeScale = 1;
            Time.timeScale = actualTimeScale;
        }
        else if(Input.GetKeyDown("="))
        {
            actualTimeScale = 10;
            Time.timeScale = actualTimeScale;
        }
        if (Input.GetKeyDown("escape"))
            Application.Quit();
        gameObject.transform.position = new Vector3(iter * 25 * 100, 0);
        gameObject.GetComponentInChildren<Text>().text = "Generation: " + GeneticAlghoritm.instance.generationCount + ". Best fitness: " + GeneticAlghoritm.instance.bestFitness + ". Worst fitness: " + GeneticAlghoritm.instance.worstFitness + ". Average fitness: " + GeneticAlghoritm.instance.averageFitness + "\nSubjects: " + (iter * 25) + "-" + (iter * 25 + 24) + "\nTo next epoch: " + string.Format("{0:0.00}s",(GeneticAlghoritm.instance.simulationTime - GeneticAlghoritm.instance.time));
    }
}
