using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestTEXT : MonoBehaviour
{
    public TextMeshProUGUI gameobjecttmp;
    int selected, selecredprevios = 0;
    public float step = 50f;
    List<TextMeshProUGUI> tmps;
    int length = 2;
    // Start is called before the first frame update
    void Start()
    {
        tmps = new List<TextMeshProUGUI>();
       
    }
    void Selection() // рассчёт значение селекта
    {
        selecredprevios = selected;//debug

        int selectClamp = selected;
        if (Input.mouseScrollDelta.y == -1)
            selectClamp--;
        else if (Input.mouseScrollDelta.y == 1)
            selectClamp++;

        if (selectClamp > length)
            selected = 0;
        else if (selectClamp < 0)
            selected = length;
        else
            selected = selectClamp;

        if (selected != selecredprevios)//debug
            print(selected);
    }
    void CreateTextChoise()
    {
        for (int i = 0; i <= length; i++)
        {
            if (i == 0)
                tmps.Add(Instantiate<TextMeshProUGUI>(gameobjecttmp, new Vector3(transform.position.x, transform.position.y + step, transform.position.z), Quaternion.identity, transform));
            else
                tmps.Add(Instantiate<TextMeshProUGUI>(gameobjecttmp, new Vector3(tmps[i - 1].transform.position.x, tmps[i - 1].transform.position.y + step, tmps[i - 1].transform.position.z), Quaternion.identity, transform));
        }
    }
    void SelectionChange() // отоброжение селекта
    {
        if (selecredprevios != selected)
        {
            tmps[selecredprevios].alignment = TextAlignmentOptions.Center;
            tmps[selected].alignment = TextAlignmentOptions.Left;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Selection();
        SelectionChange();
    }
}
