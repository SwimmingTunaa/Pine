using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New upgrade", menuName = "Item/One Time Use Item")]
public class OneTimeUseObject : PickUpObject
{
    [Header("One Time Use")]
    public GameObject itemObject;

}
