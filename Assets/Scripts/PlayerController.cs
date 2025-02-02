﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class PlayerController : MonoBehaviour 
{

	// Handling
	public float rotationSpeed = 450;
	public float walkSpeed = 5;
	public float runSpeed = 8;
	private float acceleration = 5;

	// System
	private Quaternion targetRotation;
	private Vector3 CurrentVelocityMod;

	// Components
	public Transform handHold;
	private CharacterController controller;
	private Camera cam;
	private Gun Currentgun;
	private Animator animator;

	private bool _running;
	private bool _reloading;

	public GameObject flashlight;	
	public Gun[] guns;

	private AnimatorTransitionInfo armsTransitionInfo;
	private GameGUI _gui;
	private Player _playerScript;



	// Use this for initialization
	void Start () 
	{
		_running = false;
		_gui = GameObject.FindGameObjectWithTag("GUI").GetComponent<GameGUI>();
		_playerScript = GetComponent<Player>();
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		cam = Camera.main;
		EquipGun(0);
	}
	
	// Update is called once per frame
	void Update () 
	{			
		armsTransitionInfo = animator.GetAnimatorTransitionInfo(1);

		if(Input.GetButton("Run"))
			_running = true;
		else
			_running = false;

		controlMouse();
		//controlWASD();

		if(Input.GetButtonDown("Flashlight"))
		{
			if(flashlight.activeSelf)
			{
				flashlight.SetActive(false);
				_playerScript.aSources[2].Play();			
			}
			else
			{
				flashlight.SetActive(true);
				_playerScript.aSources[2].Play();			
			}
		}

		if(Currentgun)
		{
			if(Input.GetButtonDown("Shoot") && !_running)	// Si pulsamos fuego una vez
			{
				Currentgun.Shoot();
			}
			else if(Input.GetButton("Shoot") && !_running)	// Si mantenemos boton pulsado
			{
				Currentgun.ShootContinuos();
			}

			if(Input.GetButtonDown("Reload"))
			{
				if(Currentgun.Reload())
				{
					animator.SetTrigger("Reload");
					Debug.Log("Recargando");

					_playerScript.aSources[3].Play();
					_reloading = true;
				}

			}

			if(_reloading)
			{
				Debug.Log("intentando recaragr!!!");
				if(true)//armsTransitionInfo.nameHash == Animator.StringToHash("Arms Layer.Reload -> Arms Layer.Weapon Hold"))
				{				
					Debug.Log("Recargada!!!");
					

					Currentgun.FinishReload();
					_reloading = false;
				}

			}

		}

		for(int cnt = 0; cnt < guns.Length; cnt++)
		{
			if(Input.GetKeyDown((cnt + 1) + "") || Input.GetKeyDown("[" + (cnt + 1) + "]"))
			{
				_gui.SetWeaponIcon(guns[cnt].name);
				animator.SetTrigger("ChangeWeapon");


				EquipGun(cnt);
				Debug.Log("Changing wepoan");

				break;	
			}
		}
	}

	void EquipGun(int i)
	{
		if(Currentgun)
		{
			Destroy(Currentgun.gameObject);
			Debug.Log("Arma destruida!!");
		}

		Currentgun = Instantiate(guns[i], handHold.position, handHold.rotation) as Gun;
		Currentgun.gui = _gui;
		Currentgun.transform.parent = handHold;
		animator.SetFloat("WeaponID", Currentgun.gunID);
		Debug.Log("Arma cambiada!!");


	}

	void controlMouse()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
		targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
		transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		

		CurrentVelocityMod = Vector3.MoveTowards(CurrentVelocityMod, input, acceleration * Time.deltaTime);
		Vector3 motion = CurrentVelocityMod;
		motion *= Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1?.7f:1;
		motion *= (Input.GetButton("Run"))?runSpeed:walkSpeed;
		
		motion += Vector3.up * -8;	//Para modificar un posbile salto (no implementado)
		
		controller.Move(motion * Time.deltaTime);

		// Modulo del vector motion: velocidad total a la que te mueves
		animator.SetFloat("Speed", Mathf.Sqrt(motion.x * motion.x + motion.z * motion.z));
	}

	void controlWASD()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		
		if(input != Vector3.zero)
		{
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
		}

		CurrentVelocityMod = Vector3.MoveTowards(CurrentVelocityMod, input, acceleration * Time.deltaTime);
		Vector3 motion = CurrentVelocityMod;
		motion *= Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1?.7f:1;
		motion *= (Input.GetButton("Run"))?runSpeed:walkSpeed;
		
		motion += Vector3.up * -8;
		
		controller.Move(motion * Time.deltaTime);
	}

}
