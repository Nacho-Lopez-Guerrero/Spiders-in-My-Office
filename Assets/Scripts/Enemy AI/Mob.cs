﻿#define DEBUGGER

using UnityEngine;
using System.Collections;
using System;							//To acces enum class


[AddComponentMenu("Hack And Slash/Enemy/AI/All Enemy Scripts")]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AI))]
[RequireComponent(typeof(AdvancedMovement))]

public class Mob : BaseCharacter 
{
#if DEBUGGER
	public bool debugger = true;
#endif
	private	Transform _displayName;

	static public GameObject camera;


	// Use this for initialization
	void Start () 
	{
		_displayName = transform.FindChild("Name");
		camera = GameObject.Find("Main Camera");

//		GetPrimaryAttribute((int)AttributeName.Constitution).BaseValue = 100;
//		GetVital((int)VitalName.Health).Update();

		if(_displayName == null)
		{
			Debug.LogWarning("Pllease add name script to this enemy");
			return;
		}

		_displayName.GetComponent<TextMesh>().text = gameObject.name;
	}


	new void Awake()
	{
		base.Awake();

	}


	void Update()
	{
		//if(_displayName == null)
		//{
		//	Debug.LogWarning("Pllease add name script to this enemy");
		//	return;
		//}

//		_displayName.LookAt(camera.transform);
//		_displayName.Rotate(new Vector3(0, 180, 0));
	}


	public void DisplayHealth()
	{
//		Messenger<int, int>.Broadcast("mob health update", curHealth, maxHealth);		//Es un Trigger. curHealth y maxHealth son los argumentos con que se lama al metodo 'OnChangeHealthBarSize'	 
	}



}	
	
