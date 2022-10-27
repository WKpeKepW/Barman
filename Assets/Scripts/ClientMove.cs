using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ClientMove : MonoBehaviour
{
    protected enum StatePositon { enterBar, goBarTable, atTheBarTable, goToWaitTable, atTheWaitTable, goExitTheBar, exitTheBar, readyToDie } // состояния по которым действует клиент
    protected StatePositon statePositon; // переменная для состояний
    public int nuberPositon = 0; // номер позиции к которой стремиться клиент 
    protected NavMeshAgent client;// переменная для хождения
    protected RandomPlace rp;//класс для определения рандомного места на котором можно посидеть
    public List<GameObject> positions;// перемменая для возмжных позиций на уровне
    public float fixDistantstoTheTarget = 1.20f;//фиксированное расстояние которое определяет как близко должен дойти клиент до цели
    void Awake()
    {
        positions = new List<GameObject>();
        positions.AddRange(GameObject.FindGameObjectsWithTag("Target").OrderBy(go => go.name));
        rp = new RandomPlace();
        client = GetComponent<NavMeshAgent>();
        statePositon = StatePositon.enterBar;
    }

    protected virtual void Start()
    {
        ChoiseSelect.DebugShowChange($"Количество позиций: {positions.Count.ToString()}");
    }

    // Update is called once per frame
    void Update()
    {
        TargetMove();
    }
    protected virtual void TargetMove()
    {
        SetDestination();// меняет нужную позицию
        if (Distance())// срабатывает каждый раз когда цель доходит до нужно позиции
            switch (statePositon)// должно срабатывать лишь раз после менять стейт
            {
                case StatePositon.enterBar:
                    statePositon = StatePositon.goBarTable;// можно считать что мы обьявляем что клиент будет делать дальше
                    rp.RandomEmptyPosition(1, 4,ref nuberPositon,ref positions);
                    break;
                case StatePositon.goBarTable:
                    statePositon = StatePositon.atTheBarTable;
                    StartCoroutine(CoroutineClient(20f));
                    break;
                case StatePositon.goToWaitTable:
                    statePositon = StatePositon.atTheWaitTable;
                    rp.RandomEmptyPosition(5, 13, ref nuberPositon, ref positions);
                    break;
                case StatePositon.atTheWaitTable:
                    statePositon = StatePositon.goExitTheBar;
                    StartCoroutine(CoroutineClient(20f));
                    break;
                case StatePositon.exitTheBar:
                    statePositon = StatePositon.readyToDie;
                    nuberPositon = 0;
                    ChoiseSelect.DebugShowChange("Клиент умирает");
                    ChoiseSelect.DebugShowChange($"Последняя позиция: {nuberPositon}");
                    break;
                case StatePositon.readyToDie:
                    ChoiseSelect.DebugShowChange($"Момент смерти: {Distance().ToString()}");
                    Destroy(gameObject);
                    break;
            }
    }
    protected bool Distance()
    {
        ChoiseSelect.DebugShowChange($"Позиция клиента: {nuberPositon}");
        if (Vector3.Distance(transform.position, positions[nuberPositon].transform.position) < fixDistantstoTheTarget)
            return true;
        else return false;
    }
    protected void SetDestination()
    {
        client.SetDestination(positions[nuberPositon].transform.position);
    }
    protected virtual IEnumerator CoroutineClient(float timer)
    {
        yield return new WaitForSeconds(timer);
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true;
        statePositon++;// переключает состояние на следующее
    }
    public void Death()
    {
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true;
        Destroy(gameObject);
    }
}
