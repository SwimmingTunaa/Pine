using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class Variable<T> : BaseVariable
{
    public T DefaultValue;
	public T RuntimeValue { get; set; }
    
	protected void OnEnable()
	{
		RuntimeValue = DefaultValue;
	}
}
