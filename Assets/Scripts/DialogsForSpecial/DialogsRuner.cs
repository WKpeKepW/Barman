using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DialogsRuner : MonoBehaviour
{
    enum StatePositon { enterBar, goBarTable, atTheBarTable, exitTheBar }
    StatePositon statePositon; // меняться
    int nuberPositon = 0; // целевая позиция
    dialogs dialogs; // говорить
    NavMeshAgent client; //ходить
    Transform canvasTransform, cameraTransform; //диалоговое окно всегда смотрит на камеру
    TextMeshProUGUI textMesh; // показывать над клиентом
    public TextMeshProUGUI gameobjecttmp;// пример обьекта текста для создания других текстов
    List<TextMeshProUGUI> tmps;// лист выборов
    Transform originForChoises;
    public List<GameObject> positions; // список возможных положений 
    public float fixDistantstoTheTarget = 1.06f;// минимальное расстояние до цели
    int selected, selecredprevios = 0;// выбор вариантов
    public float step = 50f;
    bool ableToSelect = false;
    int lengthforSelection = 0;
    void Start()
    {
        tmps = new List<TextMeshProUGUI>();//лист выбора
        originForChoises = GameObject.FindGameObjectWithTag("Origin").GetComponent<Transform>();
        dialogs = XmlDialogsReader.LoadXMLData("DialogsRunner");//прочитываем файл DialogsRunner
        positions = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));//лист обьектов
        client = GetComponent<NavMeshAgent>();//навигация клиента
        canvasTransform = transform.GetChild(0);
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        textMesh = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();//получение ссылки на текст для клиента
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
        client.SetDestination(positions[nuberPositon].transform.position); // идти до места 
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
            randPosition = Random.Range(id[0], id.Count);  //Что то не так с рандомом
        else
            randPosition = id[0];
        nuberPositon = randPosition; //positions.IndexOf(positions[randPosition]);
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = false; // при скрытие в испекторе таргетов происходит наложение клиентов на 2 позицию, возможный фикс это удаление родительского обьекта, либо сокращение ссылки до скрипта
    }
    IEnumerator CoroutineClientDestroy(float timer)
    {
        textMesh.text = dialogs.dialog[0].textNPC.Trim();
        yield return new WaitForSeconds(timer);
        textMesh.ClearMesh();
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true; // Освободить место
        statePositon = StatePositon.exitTheBar;
    }
    IEnumerator CoroutineDialogsStart()
    {
        print("Работаем");
        bool theend = false;
        int id = 0;
        while (!theend)
        {
            textMesh.text = dialogs.dialog[id].textNPC.Trim();
            CreateTextChoise(id);
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.F));
            yield return null;//для того чтобы изменение состояния кнопки произошло
            if (dialogs.dialog[id].choise[selected].theEnd)
                theend = true;
            else
                id = dialogs.dialog[id].choise[selected].gotoID;
            ChoiseClear();
            textMesh.ClearMesh();
        }
        positions[nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true; // Освободить место
        statePositon = StatePositon.exitTheBar;
    }
    void canvasLockAt()// перенести в канвас код
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
    void Selection() // рассчёт значение селекта
    {
        if (ableToSelect)
        {
            selecredprevios = selected;//debug

            int selectClamp = selected;
            if (Input.mouseScrollDelta.y == -1) // рассчёт селекта
                selectClamp--;
            else if (Input.mouseScrollDelta.y == 1)
                selectClamp++;

            if (selectClamp >= lengthforSelection)
                selected = 0;
            else if (selectClamp < 0)
                selected = lengthforSelection-1;
            else
                selected = selectClamp;

            if (selecredprevios != selected)//отоброжение селекта
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
