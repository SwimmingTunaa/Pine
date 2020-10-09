using UnityEngine;
using System.Collections;
using Cinemachine;
public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public static CameraShake Instance;
 	public float decreaseFactor = 1.0f; 

    // How long the object should shake for.
    public float shakeDuration = 0f;
    public bool shakeActive = false;
	
    private CinemachineVirtualCamera activeVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cmNoise;
    private Transform cameraStartPos;

    void Awake()
    {
        if(!Instance)
            Instance = this;
    }

    void Update()
    {
        if (shakeActive)
            Shake();
    }

    public void ShakeCamera(float duration, float shakeAmount, float frequency)
    {
 
        activeVirtualCamera = GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        cmNoise = activeVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cameraStartPos = activeVirtualCamera.transform;
        cmNoise.m_AmplitudeGain = shakeAmount;
        cmNoise.m_FrequencyGain = frequency;
        shakeDuration = duration;
        shakeActive = true;
    }

    public void Shake()
    {
        if (shakeDuration > 0)
        {
            shakeDuration -= Time.unscaledDeltaTime * decreaseFactor;
        }
        else
        {
            
            cmNoise.m_AmplitudeGain = 0;
            cmNoise.m_FrequencyGain = 0;
            activeVirtualCamera.transform.position = cameraStartPos.position;
            activeVirtualCamera.transform.rotation = cameraStartPos.rotation;
            shakeDuration = 0f;
            shakeActive = false;

        }
    }
}