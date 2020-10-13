using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePanelsToNewHolder : MonoBehaviour
{   
    private int triggerAmount = 1;

    void OnDisable()
    {
        triggerAmount = 1;
    }
  
    void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerAmount > 0 && other.CompareTag("Player"))
        {
            GameManager.Instance.cameraFollower.move = true;
            ParallaxManager.instance.ChangeParallax();
            PanelSpawner.Instance.ChangePanelSpawnPoint();
            MoveBlackBars.SetBlackBarHeight(PanelSpawner._Instance._currentStartingPanel.GetComponentInChildren<SpriteRenderer>());

            if(MasterSpawner.Instance.activeRegion.tag == "Cloud" || MasterSpawner.Instance.activeRegion.tag == "Storm")
            {
                StartCoroutine(CharacterManager.activeCharacter.GetComponent<PlayerController>().EquipHoverBoard());
                //remove mouse spawner
                MasterSpawner.Instance.RemoveFromChallengeList(MasterSpawner.Instance.listOfSpawners[MasterSpawner.Instance.critterSpawner.name]);
                //remove spider spawner
                MasterSpawner.Instance.RemoveFromChallengeList(MasterSpawner.Instance.listOfSpawners[SpiderSpawner.instance.name]);
            }
            else if (other.GetComponent<CharacterController2D>().isFlying == true && (MasterSpawner.Instance.activeRegion.tag != "Cloud" || MasterSpawner.Instance.activeRegion.tag == "Storm"))
            {
                other.GetComponent<CharacterController2D>().isFlying = false;
                CharacterManager.activeCharacter.GetComponent<PlayerController>().UnequipHoverBoard();
            }
            
            triggerAmount--;
        }
    }
}
