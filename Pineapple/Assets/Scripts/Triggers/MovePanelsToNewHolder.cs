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
            triggerAmount--;
            GameManager.Instance.cameraFollower.move = true;
            ParallaxManager.instance.ChangeParallax();
            PanelSpawner.Instance.ChangePanelSpawnPoint();
            MoveBlackBars.Instance.SetBlackBarHeight(PanelSpawner._Instance._currentStartingPanel.GetComponentInChildren<SpriteRenderer>());

            if(MasterSpawner.Instance.activeRegion.tag == "Cloud" || MasterSpawner.Instance.activeRegion.tag == "Storm")
            {
                StartCoroutine(CharacterManager.activeCharacter.GetComponent<PlayerController>().EquipHoverBoard());
                //remove mouse spawner
                MasterSpawner.Instance.RemoveFromChallengeList(MasterSpawner.Instance.listOfSpawners[MasterSpawner.Instance.critterSpawner.name]);
                //remove spider spawner
                MasterSpawner.Instance.RemoveFromChallengeList(MasterSpawner.Instance.listOfSpawners[SpiderSpawner.instance.name]);
                //change the fall gravity
                CharacterManager.activeCharacter.GetComponent<CharacterController2D>().m_lowJumpMulitplier = 1.5f;
            }
            if (other.GetComponent<CharacterController2D>().isFlying == true)
            {
                other.GetComponent<CharacterController2D>().isFlying = false;
                CharacterManager.activeCharacter.GetComponent<PlayerController>().UnequipHoverBoard();
                //revert the fall gravity
                CharacterManager.activeCharacter.GetComponent<CharacterController2D>().m_lowJumpMulitplier = 3f;
            }
        }
    }
}
