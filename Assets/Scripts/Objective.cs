using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    private List<Transform> rings = new List<Transform>();

    public Material activeRing;
    public Material inactiveRing;
    public Material finalRing;

    private int ringPassed = 0;

    private void Start()
    {
        foreach (Transform t in transform)
        {
            rings.Add(t);
            t.GetComponent<MeshRenderer>().material = inactiveRing;
        }

        if (rings.Count == 0)
        {
            return;
        }

        rings[ringPassed].GetComponent<MeshRenderer>().material = activeRing;
        rings[ringPassed].GetComponent<Ring>().ActivateRing();
    }

    public void NextRing()
    {
        rings[ringPassed].GetComponent<Animator>().SetTrigger("collectionTrigger");

        ringPassed++;

        if (ringPassed == rings.Count)
        {
            Victory();
            return;
        }

        if (ringPassed == rings.Count - 1)
            rings[ringPassed].GetComponent<MeshRenderer>().material = finalRing;
        else
            rings[ringPassed].GetComponent<MeshRenderer>().material = activeRing;

        rings[ringPassed].GetComponent<Ring>().ActivateRing();
    }

    private void Victory()
    {
        FindObjectOfType<GameScene>().CompleteLevel();
    }
}
