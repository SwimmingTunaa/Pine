using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionChanger : MonoBehaviour
{
    public Region regionToChangeTo;
    private int _triggerAmount = 1;

    void ChangeRegion()
    {
        MasterSpawner.Instance.activeRegion = regionToChangeTo;
    }
}
