using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOrRemoveSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public enum RemoveOrAdd
    {
        remove,
        add
    }

    public RemoveOrAdd action;
    public string spawner;
    public float spawnChance;
    private int _triggerAmount = 1;

    void OnEnable()
    {
        _triggerAmount = 1;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && _triggerAmount > 0 && spawner != "")
        {
            switch(action)
            {
                case RemoveOrAdd.add:
                {
                    MasterSpawner.Instance.AddToChallengeChanceList(MasterSpawner.Instance.listOfSpawners[spawner], spawnChance);
                    break;
                }

                case RemoveOrAdd.remove:
                {
                    MasterSpawner.Instance.RemoveFromChallengeList(MasterSpawner.Instance.listOfSpawners[spawner]);
                    break;
                }
            }
        }
    }
}
