using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ClientMove : MonoBehaviour
{
    protected enum StatePositon { enterBar, goBarTable, atTheBarTable, goToWaitTable, atTheWaitTable, goExitTheBar, exitTheBar, readyToDie } // ��������� �� ������� ��������� ������
    protected StatePositon statePositon; // ���������� ��� ���������
    public int nuberPositon = 0; // ����� ������� � ������� ���������� ������ 
    protected NavMeshAgent client;// ���������� ��� ��������
    protected RandomPlace rp;//����� ��� ����������� ���������� ����� �� ������� ����� ��������
    public List<GameObject> positions;// ���������� ��� �������� ������� �� ������
    public float fixDistantstoTheTarget = 1.20f;//������������� ���������� ������� ���������� ��� ������ ������ ����� ������ �� ����
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
        ChoiseSelect.DebugShowChange($"���������� �������: {positions.Count.ToString()}");
    }

    // Update is called once per frame
    void Update()
    {
        TargetMove();
    }
    protected virtual void TargetMove()
    {
        SetDestination();// ������ ������ �������
        if (Distance())// ����������� ������ ��� ����� ���� ������� �� ����� �������
            switch (statePositon)// ������ ����������� ���� ��� ����� ������ �����
            {
                case StatePositon.enterBar:
                    statePositon = StatePositon.goBarTable;// ����� ������� ��� �� ��������� ��� ������ ����� ������ ������
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
                    ChoiseSelect.DebugShowChange("������ �������");
                    ChoiseSelect.DebugShowChange($"��������� �������: {nuberPositon}");
                    break;
                case StatePositon.readyToDie:
                    ChoiseSelect.DebugShowChange($"������ ������: {Distance().ToString()}");
                    Destroy(gameObject);
                    break;
            }
    }
    protected bool Distance()
    {
        ChoiseSelect.DebugShowChange($"������� �������: {nuberPositon}");
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
        statePositon++;// ����������� ��������� �� ���������
    }
    public void Death()
    {
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true;
        Destroy(gameObject);
    }
}
