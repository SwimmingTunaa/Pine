using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MoveBlackBars : MonoBehaviour
{
    public SpriteRenderer blackBarTop, blackBarBot;
    public float barTransitionSpeed = 45f;

    static bool _changeBlackBarHeight;
    static float panelHalfSize;

 
    // Update is called once per frame
    void LateUpdate()
    {
        
        if(_changeBlackBarHeight) ChangeBlackBarHeight();
    }

    void ChangeBlackBarHeight()
    {
        Vector3 topNewYPos = new Vector3(blackBarTop.transform.position.x, PanelSpawner.Instance._currentStartingPanel.transform.position.y + panelHalfSize + blackBarTop.bounds.extents.y);
        Vector3 botNewYPos = new Vector3(blackBarTop.transform.position.x, PanelSpawner.Instance._currentStartingPanel.transform.position.y - panelHalfSize - blackBarBot.bounds.extents.y);
        blackBarTop.transform.position = Vector3.MoveTowards(blackBarTop.transform.position, topNewYPos, Time.deltaTime* barTransitionSpeed);
        blackBarBot.transform.position = Vector3.MoveTowards(blackBarBot.transform.position, botNewYPos, Time.deltaTime* barTransitionSpeed);
          
        if(blackBarTop.transform.position == topNewYPos && blackBarBot.transform.position == botNewYPos) _changeBlackBarHeight = false;                                           
    }

    public static void SetBlackBarHeight(SpriteRenderer renderer)
    {
        panelHalfSize = renderer.size.y/2;
        _changeBlackBarHeight = true;
       
    }
    public static void SetBlackBarHeight()
    {
        panelHalfSize = DontDestroy._instance.startingPanel.GetComponentInChildren<SpriteRenderer>().size.y/2;
        _changeBlackBarHeight = true;
    }
}
