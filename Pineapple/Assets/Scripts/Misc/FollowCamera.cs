using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    void Update()
    {
        this.transform.position = !CharacterManager.activeCharacter.GetComponent<PlayerHealth>().dead ? new Vector3(CharacterManager.activeCharacter.transform.position.x, transform.position.y, transform.position.z) :
       new Vector3(CharacterManager.activeCharacter.GetComponent<PlayerHealth>().FindFurthestBodyPart().gameObject.transform.position.x, transform.position.y, transform.position.z);
    }
}
