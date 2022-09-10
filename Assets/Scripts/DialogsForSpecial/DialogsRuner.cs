using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class DialogsRuner : MonoBehaviour
{
    enum StatePositon { enterBar, goBarTable, atTheBarTable, exitTheBar }
    StatePositon statePositon; // мен€тьс€ // целева€ позици€
    dialogs dialogs; // говорить
    NavMeshAgent client; //ходить
    TextMeshProUGUI textMesh; // показывать над клиентом
    public float fixDistantstoTheTarget = 1.06f;// минимальное рассто€ние до цели
    ChoiseSelect ChoiseSelect;
    RandomPlace rp;
    void Start()
    {
        ChoiseSelect = GameObject.FindGameObjectWithTag("Origin").GetComponent<ChoiseSelect>();
        rp = new RandomPlace();
        dialogs = XmlDialogsReader.LoadXMLData("DialogsRunner");//прочитываем файл DialogsRunner
        client = GetComponent<NavMeshAgent>();//навигаци€ клиента
        textMesh = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();//получение ссылки на текст дл€ клиента
        statePositon = StatePositon.enterBar;
    }

    // Update is called once per frame
    void Update()
    {
        TargetMove();
    }

    void TargetMove()
    {
        client.SetDestination(rp.positions[rp.nuberPositon].transform.position); // идти до места 
        if (Vector3.Distance(transform.position, rp.positions[rp.nuberPositon].transform.position) < fixDistantstoTheTarget)
        {
            if (statePositon == StatePositon.enterBar) // при позиции 1 он сначало идЄт к нему а потом к другой позиции
            {
                statePositon = StatePositon.goBarTable;
                rp.RandomEmptyPosition(1, 4);
            }
            else if (statePositon == StatePositon.goBarTable)
            {
                statePositon = StatePositon.atTheBarTable;
                StartCoroutine(CoroutineDialogsStart());

            }
            else if (statePositon == StatePositon.exitTheBar)
            {
                rp.nuberPositon = 0;
            }
        }
    }
    #region ”далить клиента
    IEnumerator CoroutineClientDestroy(float timer)
    {
        textMesh.text = dialogs.dialog[0].textNPC.Trim();
        yield return new WaitForSeconds(timer);
        textMesh.ClearMesh();
        rp.positions[rp.nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true; // ќсвободить место
        statePositon = StatePositon.exitTheBar;
    }
    #endregion
    IEnumerator CoroutineDialogsStart()
    {
        bool theend = false;
        int id = 0;
        while (!theend)
        {
            textMesh.text = dialogs.dialog[id].textNPC.Trim();
            ChoiseSelect.CreateTextChoise(id,dialogs);
            yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.F));
            yield return null;//дл€ того чтобы изменение состо€ни€ кнопки произошло
            if (dialogs.dialog[id].choise[ChoiseSelect.selected].theEnd)
                theend = true;
            else
                id = dialogs.dialog[id].choise[ChoiseSelect.selected].gotoID;
            ChoiseSelect.ChoiseClear();
            textMesh.ClearMesh();
        }
        rp.positions[rp.nuberPositon].GetComponent<TargetEmpty>().IsEmpty = true; // ќсвободить место
        statePositon = StatePositon.exitTheBar;
    }
    #region “еперь в ChoiseSelect
    //void CreateTextChoise(int id)
    //{
    //    lengthforSelection = dialogs.dialog[id].choise.Length;
    //    for (int i = 0; i < lengthforSelection; i++)
    //    {
    //        if (i == 0)
    //            tmps.Add(Instantiate<TextMeshProUGUI>(gameobjecttmp, new Vector3(originForChoises.transform.position.x, originForChoises.transform.position.y + step, originForChoises.transform.position.z), Quaternion.identity, originForChoises));
    //        else
    //            tmps.Add(Instantiate<TextMeshProUGUI>(gameobjecttmp, new Vector3(tmps[i - 1].transform.position.x, tmps[i - 1].transform.position.y + step, tmps[i - 1].transform.position.z), Quaternion.identity, originForChoises));
    //        tmps[i].text = dialogs.dialog[id].choise[i].Value.Trim(); 
    //    }
    //    ableToSelect = true;
    //}
    //void ChoiseClear()
    //{
    //    selecredprevios = 0;
    //    selected = 0;
    //    foreach (var tmpsone in tmps)
    //    {
    //        Destroy(tmpsone.gameObject);
    //    }
    //    tmps.Clear();
    //    ableToSelect = false;
    //}
    //void Selection() // рассчЄт значение селекта
    //{
    //    if (ableToSelect)
    //    {
    //        selecredprevios = selected;//debug

    //        int selectClamp = selected;
    //        if (Input.mouseScrollDelta.y == -1) // рассчЄт селекта
    //            selectClamp--;
    //        else if (Input.mouseScrollDelta.y == 1)
    //            selectClamp++;

    //        if (selectClamp >= lengthforSelection)
    //            selected = 0;
    //        else if (selectClamp < 0)
    //            selected = lengthforSelection - 1;
    //        else
    //            selected = selectClamp;

    //        if (selecredprevios != selected)//отоброжение селекта
    //        {
    //            print(selected);
    //            tmps[selecredprevios].alignment = TextAlignmentOptions.Center;
    //            tmps[selected].alignment = TextAlignmentOptions.Left;
    //        }
    //    }
    //}
    #endregion
    #region  ак работать с диалогами
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
    #endregion
}
