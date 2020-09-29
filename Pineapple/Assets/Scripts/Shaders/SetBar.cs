using UnityEngine;
using UnityEngine.UI;

public class SetBar : MonoBehaviour
{
    [SerializeField]
    private Image image ;
    
    [SerializeField]
    private float maxLevel = 5;
    public float currentLevel ;
    
    private RectTransform imageRectTransform ;

    public float Level
    {
        get { return currentLevel; }
        set
        {
            currentLevel = Mathf.Clamp(value, 0, MaxLevelPoints);
            image.material.SetFloat("_Percent",  currentLevel/MaxLevelPoints);
        }
    }
    public float MaxLevelPoints
    {
        get { return maxLevel; }
        set
        {
            maxLevel = value;
            image.material.SetFloat("_Steps", maxLevel);
        }
    }

    protected void Awake()
    {
        imageRectTransform = image.GetComponent<RectTransform>();
        image.material = Instantiate(image.material); // Clone material
        MaxLevelPoints = MaxLevelPoints; // Force the call to the setter in order to update the material
    }
    
    protected void Update()
    {
        // Dummy test, you can remove this
        if( Input.GetKeyDown(KeyCode.Space) )
        {
            LevelUp( 1 );
        }
    }
    
    public void LevelUp( float damagesPoints )
    {
        Level += 1;
    }
}