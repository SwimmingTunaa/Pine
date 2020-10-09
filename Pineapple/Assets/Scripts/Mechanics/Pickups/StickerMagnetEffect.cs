using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerMagnetEffect : MonoBehaviour
{
  void OnTriggerEnter2D(Collider2D other)
  {
    if(other.GetComponent<Sticker>() != null && !other.GetComponent<Sticker>().move)
    {
        other.gameObject.transform.parent = GameManager.Instance.cameraFollower.gameObject.transform;
        other.GetComponent<Sticker>().move = true;
    }
  }
}
