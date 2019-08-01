using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("Base")]
    public GameObject warningUI;
    public float warningDuration;

    IEnumerator ShowWarning()
    {
        yield return new WaitForSeconds(warningDuration);
        if(!warningUI.activeInHierarchy)
            warningUI.SetActive(true);
    }
}
