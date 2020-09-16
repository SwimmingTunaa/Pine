using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ReadStatObject : MonoBehaviour
{
    
    public enum VariableType
    {
        Float,
        Intger
    }

    public VariableType variableType;

    public FloatVariable floatVar;
    public IntVariable IntVar;

    public string extraText;
    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
      
      
    }
}
