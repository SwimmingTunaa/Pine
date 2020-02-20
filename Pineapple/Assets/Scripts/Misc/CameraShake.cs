using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    public static float shakeAffectRadius = 0.1f;

	
    // Amplitude of the shake. A larger value shakes the camera harder.
    public static float shakeAmplitude = 0.1f;
    public float decreaseFactor = 1.0f; 

    // How long the object should shake for.
    public static float shakeDuration = 0f;

    public static bool shakeActive =true;
	
    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = Camera.main.transform;
        }
    }

    void Start()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeActive)
            Shake();
    }

    public static void ShakeCamera(float duration)
    {
        shakeDuration = duration;
        shakeActive = true;
    }
    public static void ShakeCamera(float duration, float shakeRadius, float shakeAmount)
    {
        duration = shakeDuration;
        shakeAffectRadius = shakeRadius;
        shakeAmplitude = shakeAmount;
        shakeActive = true;
    }

    public void Shake()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmplitude; 
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
            shakeActive = false;
        }
    }
}