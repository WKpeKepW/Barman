using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlace
{
    public List<GameObject> positions; // список возможных положений 
    public RandomPlace(List<GameObject> positions)
    {
       this.positions = positions;
    }

    public void RandomEmptyPosition(int firstMayTargetID, int endMayTargetID, out int nuberPosition)
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
        nuberPosition = randPosition; //positions.IndexOf(positions[randPosition]);
        positions[nuberPosition].GetComponent<TargetEmpty>().IsEmpty = false;
        // при скрытие в испекторе таргетов происходит наложение клиентов на 2 позицию, возможный фикс это удаление родительского обьекта, либо сокращение ссылки до скрипта
    }
}
