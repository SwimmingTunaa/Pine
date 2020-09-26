using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Variable<T> : BaseVariable
{
    public T DefaultValue;
	public T RuntimeValue { get; set; }
    public bool autoReset;

	protected void OnEnable()
	{
		if(autoReset)
			RuntimeValue = DefaultValue;
	}
}
