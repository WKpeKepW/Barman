using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClientMove : MonoBehaviour
{
    enum StatePositon { enterBar, goBarTable, atTheBarTable }
    StatePositon statePositon;
    int nuberPositon = 0;
    NavMeshAgent client;
    public List<GameObject> positions;
    public float fixDistantstoTheTarget = 1.06f;
    void Start()
    {
        positions = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
        client = GetComponent<NavMeshAgent>();
        statePositon = StatePositon.enterBar;
    }

    // Update is called once per frame
    void Update()
    {
        TargetMove();
    }
    void TargetMove()
    {
        client.SetDestination(positions[nuberPositon].transform.position);
        if (Vector3.Distance(transform.position, positions[nuberPositon].transform.position) < fixDistantstoTheTarget)
        {
            if (statePositon == StatePositon.enterBar) // при позиции 1 он сначало идёт к нему а потом к другой позиции
            {
                statePositon = StatePositon.goBarTable;
                RandomEmptyPosition(1, 4);
            }
            else if (statePositon == StatePositon.goBarTable)
            {
                statePositon = StatePositon.atTheBarTable;
                StartCoroutine(CoroutineClientDestroy(20f));
            }
        }
    }
    void RandomEmptyPosition(int firstMayTargetID, int endMayTargetID)
    {
        List<int> id = new List<int>();
        for (int i = firstMayTargetID; i <= endMayTargetID; i++)
        {
            if (positions[i].GetComponent<TargetEmpty>().IsEmpty)
            {
                id.Add(i);
            }
        }
        int randPosition;
        if (id.Count != 1)
            randPosition = Random.Range(id[0], id.Count);  //Что то не так с рандомом
        else
            randPosition = id[0];
        nuberPositon = randPosition; //positions.IndexOf(positions[randPosition]);
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = false; // при скрытие в испекторе таргетов происходит наложение клиентов на 2 позицию, возможный фикс это удаление родительского обьекта, либо сокращение ссылки до скрипта
    }
    IEnumerator CoroutineClientDestroy(float timer)
    {
        yield return new WaitForSeconds(timer);
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true;
        Destroy(gameObject);
    }
}
