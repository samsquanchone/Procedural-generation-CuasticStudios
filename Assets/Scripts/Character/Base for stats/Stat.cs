using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{

    public int baseValue;

    private List<int> modifers = new List<int>();

    public int GetValue()
    {
        int finalValue = baseValue;
        modifers.ForEach(x => finalValue += x);
        return finalValue;
    }

    public void AddModifer(int modifer)
    {
        if (modifer != 0)
        {
            modifers.Add(modifer);
        }
    }

    public void RemoveModifer(int modifer)
    {
        if (modifer != 0)
        {
            modifers.Remove(modifer);
        }
    }


}
