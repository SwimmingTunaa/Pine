using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ResetSticker : MonoBehaviour
{
    private List<Sticker> stickerList = new List<Sticker>();
    
    void Start()
    {
        stickerList = GetComponentsInChildren<Sticker>().ToList();
    }
    void OnEnable()
    {
        foreach(Sticker s in stickerList)
        {
            s.gameObject.SetActive(true);
        }
    }
}
