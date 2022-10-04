using System.Collections.Generic;
using UnityEngine;

public class ShakerCrafter : MonoBehaviour
{
    List<string> elements;
    List<int> currentIDelements;
    public List<GameObject> coctails;
    GameObject Cap;
    bool Crafting = false;
    BottleManager bm;
    GameObject currentCocatail;
    private void Awake()
    {
        elements = new List<string>() { "Rum", "CocaCola" };
        currentIDelements = new List<int>();
        Cap = transform.Find("Cap").gameObject;
        bm = GetComponent<BottleManager>();
    }
    private void Update()
    {
        if (Cap.activeSelf && Crafting)
        {
            Craft();
            Crafting = false;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!Cap.activeSelf)
        {
            ElementAdd(other.transform.name);
            Crafting = true;
        }
    }
    void ElementAdd(string tag)
    {
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i] == tag)
                if (!currentIDelements.Contains(i))
                    currentIDelements.Add(i);
        }
    }
    void Craft()
    {
        currentIDelements.Sort();
        if (currentCocatail != null)
            bm.DestroyFluid(); // багулина
        if (currentIDelements[0] == 0 && currentIDelements[1] == 1) // Rum и CocaCola
        {
            currentCocatail = Instantiate(coctails[0], transform.TransformPoint(0, 0.1f, 0), Quaternion.identity, transform);
            print("Cuba Libre");
        }
        else
        {
            currentCocatail = Instantiate(coctails[1], transform.TransformPoint(0, 0.1f, 0), Quaternion.identity, transform);
            print("toxic waste");
        }
        bm.FindAndOpen();
    }
}
