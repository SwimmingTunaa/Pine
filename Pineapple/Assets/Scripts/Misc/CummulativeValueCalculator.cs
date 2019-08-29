using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cummulativeCalculator<T> where T : ObjectPools
{
    private List<float> _cumulativeEntrySums;
    private float _cumulativeSum;

    public int GetRandomEntryIndex(List<T> poolList)
    {
        CreateCumulativeSums(poolList);
        int randomIndex = GetRandomIndex();
        return randomIndex;
    }

    private int GetRandomIndex()
    {
        float randomValue= Random.Range(0f, _cumulativeSum);
        for(int i = 0; i < _cumulativeEntrySums.Count; i++)
        {
            if(_cumulativeEntrySums[i] >= randomValue)
            {
                return i;
            }
        }
        return _cumulativeEntrySums.Count - 1;
    }
    
    void CreateCumulativeSums(List<T> poolList)
    {
        _cumulativeSum = 0;
        _cumulativeEntrySums = new List<float>();
 
        for(int i = 0; i < poolList.Count; i++)
        {
            _cumulativeSum += poolList[i].Chance;
            _cumulativeEntrySums.Add(_cumulativeSum);
        }
    }
}
