using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class SetBlur : MonoBehaviour
{
    public float blurAmount;

    private Material material;
    // Start is called before the first frame update
    void Awake()
    {
        material = GetComponent<Image>().material;
    }

    void Update()
    {
        material = GetComponent<Image>().material;
        material.SetFloat("_Size", blurAmount);
    }
}
