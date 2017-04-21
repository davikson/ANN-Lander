using UnityEngine;
using System.Collections;

public class LanderController : NeuralNetwork
{
    public Genome chromosome;
    Rigidbody2D rigid;
    public GameObject Platform;
    public ParticleSystem LeftEngine;
    public ParticleSystem RightEngine;
    public ParticleSystem DownEngine;
    float closestDistans;
    float totalRotation;
    float timeSinceBegin;
    bool landed;
    void Awake()
    {
        Platform.transform.localPosition = new Vector2(Random.Range(-5.0f, 3.0f), Random.Range(-4.0f, -.0f));
        chromosome = new Genome();
        rigid = GetComponent<Rigidbody2D>();
        Physics2D.gravity = new Vector2(0, -1.62f);
        numberOfInputs = Params.numberOfInputs;
        numberOfOutputs = Params.numberOfInputs;
        numberOfHiddenLayer = Params.numberOfHiddenLayer;
        numberOfNeuronsPerHiddenLayer = Params.numberOfNeuronsPerHiddenLayer;
        CreateNetwork();
        inputs = new float[numberOfInputs];
        outputs = new float[numberOfOutputs];
        closestDistans = (gameObject.transform.position - Platform.transform.position).magnitude;
    }
    void FixedUpdate()
    {
        CalculateOutputs();
        rigid.AddRelativeForce(Vector2.up * ((outputs[0] > 0) ? outputs[0] : 0) * 2 * 43000, ForceMode2D.Force);
        DownEngine.emissionRate = outputs[0] * 10000;
        rigid.AddTorque(outputs[1] * 3600);
        LeftEngine.emissionRate = -outputs[1] * 1000;
        RightEngine.emissionRate = outputs[1] * 1000;
        if ((gameObject.transform.position - Platform.transform.position).magnitude < closestDistans)
            closestDistans = (gameObject.transform.position - Platform.transform.position).magnitude;
        totalRotation += rigid.angularVelocity * Time.fixedDeltaTime;
        timeSinceBegin += Time.fixedDeltaTime;
    }
    protected override void CalculateOutputs()
    {
        inputs[0] = rigid.angularVelocity;
        inputs[1] = gameObject.transform.rotation.z;
        inputs[2] = rigid.velocity.x;
        inputs[3] = rigid.velocity.y;
        inputs[4] = (gameObject.transform.position - Platform.transform.position).normalized.x;
        inputs[5] = (gameObject.transform.position - Platform.transform.position).normalized.y;
        base.CalculateOutputs();
    }
    public void CalculateFitness()
    {
        if (!landed)
            chromosome.fitness = 50 / (5 * closestDistans + 1);
        else
            chromosome.fitness += 15 / (3 * Mathf.Abs((gameObject.transform.position - Platform.transform.position).magnitude) + 1);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Platform")
        {
            landed = true;
            chromosome.fitness = 30 / (9 * rigid.velocity.magnitude + 1) + 5 / (4 * timeSinceBegin) + 25 / (7 * (gameObject.transform.position - Platform.transform.position).magnitude + 1) + 25 / (5 * Mathf.Abs(totalRotation) + 1);
        }
    }
}
