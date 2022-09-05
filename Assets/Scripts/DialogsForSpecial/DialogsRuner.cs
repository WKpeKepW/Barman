using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class DialogsRuner : MonoBehaviour
{
    enum StatePositon { enterBar, goBarTable, atTheBarTable, exitTheBar }
    StatePositon statePositon; // меняться
    int nuberPositon = 0; // целевая позиция
    dialogs dialogs; // говорить
    NavMeshAgent client; //ходить
    Transform canvasTransform, cameraTransform; //диалоговое окно всегда смотрит на камеру
    TextMeshProUGUI textMesh; // показывать
    public List<GameObject> positions; // список возможных положений 
    public float fixDistantstoTheTarget = 1.06f;// минимальное расстояние до цели

    [ContextMenu("Start")]
    void Start()
    {
        dialogs = XmlDialogsReader.LoadXMLData("DialogsRunner");//прочитываем файл DialogsRunner
        positions = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
        client = GetComponent<NavMeshAgent>();
        canvasTransform = transform.GetChild(0);
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        textMesh = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();//получение ссылки на текст
        statePositon = StatePositon.enterBar;
    }

    // Update is called once per frame
    void Update()
    {
        TargetMove();
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
                StartCoroutine(CoroutineClientDestroy(5f));
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
    void canvasLockAt()// перенести в канвас код
    {
        canvasTransform.transform.LookAt(cameraTransform);
        canvasTransform.Rotate(0, 180, 0);
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
