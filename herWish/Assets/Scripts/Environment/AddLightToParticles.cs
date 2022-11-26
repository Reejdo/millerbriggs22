using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AddLightToParticles : MonoBehaviour
{
    public GameObject myPrefab;

    private ParticleSystem myParticleSystem;
    private List<GameObject> myInstances = new List<GameObject>();
    private ParticleSystem.Particle[] myParticles;

    // Start is called before the first frame update
    void Start()
    {
        myParticleSystem = GetComponent<ParticleSystem>();
        myParticles = new ParticleSystem.Particle[myParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = myParticleSystem.GetParticles(myParticles);

        while (myInstances.Count < count)
            myInstances.Add(Instantiate(myPrefab, myParticleSystem.transform));

        bool worldSpace = (myParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < myInstances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                    myInstances[i].transform.position = myParticles[i].position;
                else
                    myInstances[i].transform.localPosition = myParticles[i].position;
                myInstances[i].SetActive(true);
            }
            else
            {
                myInstances[i].SetActive(false);
            }
        }
    }
}
