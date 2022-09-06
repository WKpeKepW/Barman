using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DialogsRuner : MonoBehaviour
{
    enum StatePositon { enterBar, goBarTable, atTheBarTable, exitTheBar }
    StatePositon statePositon; // ��������
    int nuberPositon = 0; // ������� �������
    dialogs dialogs; // ��������
    NavMeshAgent client; //������
    Transform canvasTransform, cameraTransform; //���������� ���� ������ ������� �� ������
    TextMeshProUGUI textMesh; // ���������� ��� ��������
    public TextMeshProUGUI gameobjecttmp;// ������ ������� ������ ��� �������� ������ �������
    List<TextMeshProUGUI> tmps;// ���� �������
    Transform originForChoises;
    public List<GameObject> positions; // ������ ��������� ��������� 
    public float fixDistantstoTheTarget = 1.06f;// ����������� ���������� �� ����
    int selected, selecredprevios = 0;// ����� ���������
    public float step = 50f;
    bool ableToSelect = false;
    int lengthforSelection = 0;
    void Start()
    {
        tmps = new List<TextMeshProUGUI>();//���� ������
        originForChoises = GameObject.FindGameObjectWithTag("Origin").GetComponent<Transform>();
        dialogs = XmlDialogsReader.LoadXMLData("DialogsRunner");//����������� ���� DialogsRunner
        positions = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));//���� ��������
        client = GetComponent<NavMeshAgent>();//��������� �������
        canvasTransform = transform.GetChild(0);
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        textMesh = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();//��������� ������ �� ����� ��� �������
        statePositon = StatePositon.enterBar;
    }

    // Update is called once per frame
    void Update()
    {
        TargetMove();
        Selection();
    }

    void LateUpdate()
    {
        canvasLockAt();
    }

    void TargetMove()
    {
        client.SetDestination(positions[nuberPositon].transform.position); // ���� �� ����� 
        if (Vector3.Distance(transform.position, positions[nuberPositon].transform.position) < fixDistantstoTheTarget)
        {
            if (statePositon == StatePositon.enterBar) // ��� ������� 1 �� ������� ��� � ���� � ����� � ������ �������
            {
                statePositon = StatePositon.goBarTable;
                RandomEmptyPosition(1, 4);
            }
            else if (statePositon == StatePositon.goBarTable)
            {
                statePositon = StatePositon.atTheBarTable;
                StartCoroutine(CoroutineDialogsStart());

            }
            else if (statePositon == StatePositon.exitTheBar)
            {
                nuberPositon = 0;
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
            randPosition = Random.Range(id[0], id.Count);  //��� �� �� ��� � ��������
        else
            randPosition = id[0];
        nuberPositon = randPosition; //positions.IndexOf(positions[randPosition]);
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = false; // ��� ������� � ��������� �������� ���������� ��������� �������� �� 2 �������, ��������� ���� ��� �������� ������������� �������, ���� ���������� ������ �� �������
    }
    IEnumerator CoroutineClientDestroy(float timer)
    {
        textMesh.text = dialogs.dialog[0].textNPC.Trim();
        yield return new WaitForSeconds(timer);
        textMesh.ClearMesh();
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true; // ���������� �����
        statePositon = StatePositon.exitTheBar;
    }
    IEnumerator CoroutineDialogsStart()
    {
        print("��������");
        bool theend = false;
        int id = 0;
        while (!theend)
        {
            textMesh.text = dialogs.dialog[id].textNPC.Trim();
            CreateTextChoise(id);
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.F));
            yield return null;//��� ���� ����� ��������� ��������� ������ ���������
            if (dialogs.dialog[id].choise[selected].theEnd)
                theend = true;
            else
                id = dialogs.dialog[id].choise[selected].gotoID;
            ChoiseClear();
            textMesh.ClearMesh();
        }
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true; // ���������� �����
        statePositon = StatePositon.exitTheBar;
    }
    void canvasLockAt()// ��������� � ������ ���
    {
        canvasTransform.transform.LookAt(cameraTransform);
        canvasTransform.Rotate(0, 180, 0);
    }
    void CreateTextChoise(int id)
    {
        lengthforSelection = dialogs.dialog[id].choise.Length;
        for (int i = 0; i < lengthforSelection; i++)
        {
            if (i == 0)
                tmps.Add(Instantiate<TextMeshProUGUI>(gameobjecttmp, new Vector3(originForChoises.transform.position.x, originForChoises.transform.position.y + step, originForChoises.transform.position.z), Quaternion.identity, originForChoises));
            else
                tmps.Add(Instantiate<TextMeshProUGUI>(gameobjecttmp, new Vector3(tmps[i - 1].transform.position.x, tmps[i - 1].transform.position.y + step, tmps[i - 1].transform.position.z), Quaternion.identity, originForChoises));
            tmps[i].text = dialogs.dialog[id].choise[i].Value.Trim(); 
        }
        ableToSelect = true;
    }
    void ChoiseClear()
    {
        selecredprevios = 0;
        selected = 0;
        foreach (var tmpsone in tmps)
        {
            Destroy(tmpsone.gameObject);
        }
        tmps.Clear();
        ableToSelect = false;
    }
    void Selection() // ������� �������� �������
    {
        if (ableToSelect)
        {
            selecredprevios = selected;//debug

            int selectClamp = selected;
            if (Input.mouseScrollDelta.y == -1) // ������� �������
                selectClamp--;
            else if (Input.mouseScrollDelta.y == 1)
                selectClamp++;

            if (selectClamp >= lengthforSelection)
                selected = 0;
            else if (selectClamp < 0)
                selected = lengthforSelection-1;
            else
                selected = selectClamp;

            if (selecredprevios != selected)//����������� �������
            {
                print(selected);
                tmps[selecredprevios].alignment = TextAlignmentOptions.Center;
                tmps[selected].alignment = TextAlignmentOptions.Left;
            }
        }
    }
    void dialogShow()
    {
        for (int i = 0; i <= dialogs.dialog.Length - 1; i++)
        {

            print($"{i}id: {dialogs.dialog[i].id}");
            print($"{i}name: {dialogs.dialog[i].name}");
            print($"{i}textNPC: {dialogs.dialog[i].textNPC}");
            for (int k = 0; k <= dialogs.dialog[i].choise.Length - 1; k++)
            {
                print($"{i}-{k}gotoID: {dialogs.dialog[i].choise[k].gotoID}");
                print($"{i}-{k}theEnd: {dialogs.dialog[i].choise[k].theEnd}");
                print($"{i}-{k}TextChoise: {dialogs.dialog[i].choise[k].Value}");
            }
        }
    }
}
