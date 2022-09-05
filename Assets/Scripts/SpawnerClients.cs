using System.Collections;
using UnityEngine;

public class SpawnerClients : MonoBehaviour
{
    [SerializeField] GameObject Client;
    int i = 0;
    public int time = 5;
    public int periodTime = 4;
    public int amount = 3;
    private void Start()
    {
        Instantiate(Client);
    }
    void Update()
    {
        while (i != amount)
        {
            i++;
            time += periodTime;
            StartCoroutine(CoroutineWaiter());
        }
    }
    IEnumerator CoroutineWaiter()
    {
        yield return new WaitForSeconds(time);
        Instantiate(Client);
    }

}
