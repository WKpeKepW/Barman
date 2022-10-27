using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlace
{
    public void RandomEmptyPosition(int firstMayTargetID, int endMayTargetID, ref int number, ref List<GameObject> position)
    {
        ChoiseSelect.DebugShowChange("Random ��������");
        List<int> id = new List<int>();
        for (int i = firstMayTargetID; i <= endMayTargetID; i++)
        {
            if (position[i].GetComponent<TargetEmpty>().IsEmpty)
            {
                id.Add(i);
            }
        }
        int randPosition;
        if (id.Count != 1)
            randPosition = Random.Range(id[0], id.Count);  //��� �� �� ��� � ��������
        else
            randPosition = id[0];
        number = randPosition; //positions.IndexOf(positions[randPosition]);
        position[randPosition].GetComponent<TargetEmpty>().IsEmpty = false;
        // ��� ������� � ��������� �������� ���������� ��������� �������� �� 2 �������, ��������� ���� ��� �������� ������������� �������, ���� ���������� ������ �� �������
    }
}
