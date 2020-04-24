using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickerMagnetEffect : MonoBehaviour
{
  void OnTriggerStay2D(Collider2D other)
  {
      if(other.GetComponent<Sticker>() != null && !other.GetComponent<Sticker>().move)
    {
        other.GetComponent<Sticker>().move = true;
    }
  }
}
