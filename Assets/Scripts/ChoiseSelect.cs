using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiseSelect : MonoBehaviour
{
    public TextMeshProUGUI gameobjecttmp;// ������ ������� ������ ��� �������� ������ �������
    List<TextMeshProUGUI> tmps;// ���� �������
    [HideInInspector] public int selected, selecredprevios = 0;// ����� ���������
    public float step = 50f;
    bool ableToSelect = false;
    int lengthforSelection = 0;

    // Start is called before the first frame update
    void Start()
    {
        tmps = new List<TextMeshProUGUI>();//���� ������
    }

    // Update is called once per frame
    void Update()
    {
        Selection();
    }

    public void CreateTextChoise(int id, dialogs dialogs)
    {
        lengthforSelection = dialogs.dialog[id].choise.Length;
        for (int i = 0; i < lengthforSelection; i++)
        {
            if (i == 0)
                tmps.Add(Instantiate<TextMeshProUGUI>(gameobjecttmp, new Vector3(transform.position.x, transform.position.y + step, transform.position.z), Quaternion.identity, transform));
            else
                tmps.Add(Instantiate<TextMeshProUGUI>(gameobjecttmp, new Vector3(tmps[i - 1].transform.position.x, tmps[i - 1].transform.position.y + step, tmps[i - 1].transform.position.z), Quaternion.identity, transform));
            tmps[i].text = dialogs.dialog[id].choise[i].Value.Trim();
        }
        ableToSelect = true;
    }
    public void ChoiseClear()
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
                selected = lengthforSelection - 1;
            else
                selected = selectClamp;

            if (selecredprevios != selected)//����������� �������
            {
                tmps[selecredprevios].alignment = TextAlignmentOptions.Center;
                tmps[selected].alignment = TextAlignmentOptions.Left;
            }
        }
    }

}
