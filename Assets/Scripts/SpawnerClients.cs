using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class SpawnerClients : MonoBehaviour
{
    [SerializeField] GameObject Client;
    int clients = 0;
    public int time = 5;
    public int amount = 3;
    public List<GameObject> positions;
    bool timefornewclient = true;
    private void Awake()
    {
        positions = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
    }
    private void Start()
    {
        Instantiate(Client);
    }
    void Update()
    {
        while (clients != amount && timefornewclient)
        {
            int busypositons = 0;
            for (int k = 1; k <= 4; k++)
            {
                if (!positions[k].GetComponent<TargetEmpty>().IsEmpty) // будет работать когда позиция enterbar будет близко к спавну// даже так не работает
                {
                    busypositons++;
                }
            }
            if (busypositons != 4)
            {
                clients++;
                timefornewclient = false;
                StartCoroutine(CoroutineWaiter());
            }
        }
    }
    IEnumerator CoroutineWaiter()
    {
        yield return new WaitForSeconds(time);
        timefornewclient = true;
        Instantiate(Client);
    }

}
