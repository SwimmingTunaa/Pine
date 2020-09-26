using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ReadStatObject))]
[CanEditMultipleObjects]
public class VariableTypePicker : Editor
{
    public override void OnInspectorGUI()
     {
         ReadStatObject script = (ReadStatObject)target;
 
         script.variableType = (ReadStatObject.VariableType)EditorGUILayout.EnumPopup("Variable Type", script.variableType);

        switch(script.variableType)
        {
            case ReadStatObject.VariableType.Float:
                script.floatVar = EditorGUILayout.ObjectField("Float", script.floatVar, typeof(FloatVariable),true) as FloatVariable;
            break;
            case ReadStatObject.VariableType.Intger:
                script.intVar = EditorGUILayout.ObjectField("Int", script.intVar, typeof(IntVariable),true) as IntVariable;
            break;
        }
        script.constantUpdate = EditorGUILayout.Toggle("Constant Update?", script.constantUpdate);
        script.extraText = EditorGUILayout.TextField("Addiional text", script.extraText);
     }
}
