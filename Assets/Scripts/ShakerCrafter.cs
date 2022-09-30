using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShakerCrafter : MonoBehaviour
{
    List<string> elements = new List<string>() {"Rum","CocaCola" };
    HashSet<int> currentIDelements = new HashSet<int>();
    GameObject Cap;
    public bool isEmpty = true;
    private void Awake()
    {
        Cap = transform.Find("Cap").gameObject;
    }
    private void Update()
    {
        if(Cap.activeSelf && !isEmpty)
        {
            Craft();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!Cap.activeSelf)
        {
            ElementAdd(other.transform.tag);
            isEmpty = false;
        }
    }

    void ElementAdd(string tag)
    {
        for (int i = 0; i > elements.Count;i++)
        {
            if (elements[i] == tag)
                currentIDelements.Add(i);
        }
    }
    void Craft()
    {
        List<int> elementsID = currentIDelements.ToList();
        elementsID.Sort();
        if (elementsID[0] == 0 && elementsID[1] == 1) // Rum è CocaCola
        {
            print("Cuba Libre");
        }
    }
}
