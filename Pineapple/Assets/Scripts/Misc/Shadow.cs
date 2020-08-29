using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public GameObject body;
    public float sizeIncreaseMultiplier;
    public LayerMask layerToDetect;


    private Vector3 startScale;
    private Color startColor;
    private SpriteRenderer spriteRenderer;
    private float distanceFromGround;
    private Vector3 shadowMaxSize;

    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        shadowMaxSize = startScale * sizeIncreaseMultiplier;
        startColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(body.activeSelf && !body.GetComponent<CharacterController2D>().m_Grounded)
            ShadowScale();
    }

    void ShadowScale()
    {
        RaycastHit2D hit = Physics2D.Raycast(body.transform.position, -transform.up, 8, layerToDetect);
       // Debug.Log("Shadow raycast hit: " + hit.collider.gameObject.name);
        //if layer is equal to ground then make the shadow stay on the ground
        if(hit.collider != null && hit.collider.gameObject.layer == 12)
        {
            spriteRenderer.enabled = true;
            float newYPos = hit.point.y;
            Vector3 newPos = new Vector3(body.transform.position.x, newYPos, body.transform.position.z);

            transform.position = newPos;

            //make the shadow scale up and opacity is lower based on how high the player is. Pass a threshold then the shadow dissapears
            distanceFromGround = Mathf.Clamp(Mathf.Abs(hit.point.y - body.transform.position.y), 0f, 5f);
            transform.localScale = startScale + Vector3.ClampMagnitude(startScale * distanceFromGround, 10);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.InverseLerp(2,0, distanceFromGround));
        }
        else
            spriteRenderer.enabled = false;
    }
}
