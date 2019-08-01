using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds;
    [HideInInspector] public float[] _parallaxScales;
    public float smoothing;
    private Vector3[] startPos;

    private Transform _cam;
    private Vector3 _previousCamPos;

    void Awake()
    {
        _cam = Camera.main.transform;
         startPos = new Vector3[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            startPos[i] = backgrounds[i].localPosition;      
        }
    }
    void Start()
    {
        _previousCamPos = _cam.position;

        _parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            _parallaxScales[i] = backgrounds[i].position.z*-1;
        }
    }

    void OnEnable()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {            
            Vector3 resetPos = startPos[i];
            backgrounds[i].localPosition = resetPos; 
        }
    }

    void Update()
    {
        for(int i = 0; i < backgrounds.Length; i++)
        {
            if(backgrounds[i].gameObject.activeInHierarchy)
            {
                float parallax = (_previousCamPos.x - _cam.position.x) * _parallaxScales[i];
                float targetPosX = backgrounds[i].position.x + parallax;
                Vector3 targetPos = new Vector3 (targetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
                backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, targetPos, smoothing * Time.deltaTime);  
            }
        }
        _previousCamPos = _cam.position;
    }
}
