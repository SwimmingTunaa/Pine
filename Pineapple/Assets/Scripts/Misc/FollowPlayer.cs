using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool dontFollowPanelY;
    public bool move;
    public float transitionSpeed = 35f;

    private PlayerHealth playerHealth;
    Camera cam;
    Vector3 followPos;

    private Vector3 newPos;
    private Vector3 posToMoveTo;

    void Start()
    {
        playerHealth = CharacterManager.activeCharacter.GetComponent<PlayerHealth>();
        cam = Camera.main;
    }
    void LateUpdate()
    {
        if(move) moveToNewPos();

        if(dontFollowPanelY)
        {
            newPos = new Vector3(!playerHealth.dead? CharacterManager.activeCharacter.transform.position.x : playerHealth.FindFurthestBodyPart().gameObject.transform.position.x
                                    , transform.position.y);
            this.transform.position = newPos;
        }
        else
        {
            newPos = new Vector3(!playerHealth.dead? CharacterManager.activeCharacter.transform.position.x : playerHealth.FindFurthestBodyPart().gameObject.transform.position.x
                                    , PanelSpawner.Instance._currentStartingPanel.transform.position.y);
            this.transform.position = newPos;
        }        
    }

    public void moveToNewPos()
    {
        if (transform.position != PanelSpawner.Instance._currentStartingPanel.transform.position)
        {
            //transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, yToMoveTo, Time.deltaTime * 7f), transform.position.z);
            Vector3 targetPos = new Vector3(transform.position.x, PanelSpawner.Instance._currentStartingPanel.transform.position.y);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime *  transitionSpeed);
        }
        if(transform.position == PanelSpawner.Instance._currentStartingPanel.transform.position) move = false;
    }
}
