using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWarningBubble : MonoBehaviour
{
    public GameObject warningBubbleToSpawn;
    
    [HideInInspector] public GameObject warningBubbleClone;
    // Start is called before the first frame update
    void Awake()
    {
        warningBubbleClone = Instantiate(warningBubbleToSpawn, transform.position, transform.rotation);
        warningBubbleClone.transform.parent = PoolManager.instance.transform;
        warningBubbleClone.SetActive(false);
    }
}
