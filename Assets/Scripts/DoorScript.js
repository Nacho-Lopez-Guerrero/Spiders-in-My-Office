#pragma strict

var open : boolean = false;

var openAnimationString : String;
var closeAnimationString : String;

var buttonTransform : Transform;
var distToOpen : float = 4;

@HideInInspector
var playerTransform : Transform;
@HideInInspector
var cameraTransform : Transform;

var openSound : AudioClip;
var closeSound : AudioClip;
var openDoorHUDText : TextMesh;

function Awake ()
{
	playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
	if(open)
		GetComponent.<Animation>().Play(openAnimationString);
}

function Start () {

}

function Update () 
{
	if(playerTransform != null)
	{
		var alreadyChecked : boolean = false;
		var angle : float = Vector3.Angle(buttonTransform.position - cameraTransform.position, buttonTransform.position + (cameraTransform.right * buttonTransform.localScale.magnitude) - cameraTransform.position);
		if(Vector3.Distance(playerTransform.position, buttonTransform.position) <= distToOpen)
		{
			if(Vector3.Angle(buttonTransform.position - cameraTransform.position, cameraTransform.forward) < angle)
			{
				openDoorHUDText.gameObject.SetActive(true);
				if(Input.GetButton("Use") && !GetComponent.<Animation>().isPlaying)
					{
						if(open)
						{
							openDoorHUDText.text = "Press  'e'  to   open   door";

							GetComponent.<Animation>().Play(closeAnimationString);
							open = false;
							alreadyChecked = true;
							if(closeSound)
								GetComponent.<AudioSource>().PlayOneShot(closeSound);
						}
						if(!open && !alreadyChecked)
						{
							openDoorHUDText.text = "Press  'e'  to   close   door";
							GetComponent.<Animation>().Play(openAnimationString);
							open = true;
							if(openSound)
								GetComponent.<AudioSource>().PlayOneShot(openSound);
						}
				}
			}
		}
		else
			openDoorHUDText.gameObject.SetActive(false);

	}
}