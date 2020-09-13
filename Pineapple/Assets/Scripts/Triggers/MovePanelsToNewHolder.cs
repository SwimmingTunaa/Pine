using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanelsToNewHolder : MonoBehaviour
{   
    private int triggerAmount = 1;

    void OnEnable()
    {
        triggerAmount = 1;
    }
  
    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0 && other.CompareTag("Player"))
        {
            GameManager.Instance.cameraFollower.move = true;
            triggerAmount--;
            PanelSpawner.Instance.ChangePanelSpawnPoint();
            print(MasterSpawner.Instance.activeRegion.tag);
            if(MasterSpawner.Instance.activeRegion.tag == "Cloud" || MasterSpawner.Instance.activeRegion.tag == "Storm")
            {
                StartCoroutine(CharacterManager.activeCharacter.GetComponent<PlayerController>().EquipHoverBoard());
            }
            else if (other.GetComponent<CharacterController2D>().isFlying == true && (MasterSpawner.Instance.activeRegion.tag != "Cloud" || MasterSpawner.Instance.activeRegion.tag == "Storm"))
            {
                other.GetComponent<CharacterController2D>().isFlying = false;
                CharacterManager.activeCharacter.GetComponent<PlayerController>().UnequipHoverBoard();
            }
        }
    }
}
