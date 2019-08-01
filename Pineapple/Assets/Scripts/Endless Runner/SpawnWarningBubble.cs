using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWarningBubble : MonoBehaviour
{
    public GameObject warningBubbleToSpawn;

    [HideInInspector] public GameObject warningBubbleClone;
    private bool _firstSpawn;
    // Start is called before the first frame update
    void Awake()
    {
        if(!_firstSpawn)
        {
            warningBubbleClone = Instantiate(warningBubbleToSpawn, transform.position, transform.rotation);
            warningBubbleClone.transform.parent = Camera.main.transform;
            warningBubbleClone.SetActive(false);
            _firstSpawn = true;
        }
    }
}
