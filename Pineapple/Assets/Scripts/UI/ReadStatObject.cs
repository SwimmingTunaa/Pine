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

    public bool constantUpdate;
    public VariableType variableType;
    public string test;

    public FloatVariable floatVar;
    public IntVariable intVar;

    public string extraText;
    private TextMeshProUGUI text;
    private float varFloat{get {return floatVar.RuntimeValue;}}
    private int varInt{get{return intVar.RuntimeValue;}}

    void Awake()
    {
       text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        switch(variableType)
        {
            case VariableType.Float:
                text.text = varFloat.ToString();
            break;
            case VariableType.Intger:
                text.text = varInt.ToString();
            break;
        }
        if(extraText != null)
        {
            text.text += extraText;
        }
    }

   void Update()
    {
        if(!constantUpdate)
            return;

        switch(variableType)
        {
            case VariableType.Float:
                text.text = floatVar.RuntimeValue.ToString();
            break;
            case VariableType.Intger:
                text.text = intVar.RuntimeValue.ToString();
            break;
        }
        if(extraText != null)
        {
            text.text += extraText;
        }
    }
}
