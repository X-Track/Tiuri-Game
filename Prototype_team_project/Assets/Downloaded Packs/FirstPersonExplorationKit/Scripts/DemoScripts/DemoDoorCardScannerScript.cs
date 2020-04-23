using UnityEngine;
using System.Collections;

public class DemoDoorCardScannerScript : FPEInteractableActivateScript {

	[SerializeField]
	private Material lockedMaterial;
	[SerializeField]
	private Material errorMaterial;
	[SerializeField]
	private Material unlockedMaterial;

	[SerializeField]
	private AudioClip doorUnlockFailure;
	[SerializeField]
	private AudioClip doorUnlockSuccess;

	private GameObject statusLight;
	private Light lightSource;

	private float errorLightTime = 0.0f;
	private float errorLightBlinkInterval = 0.2f;

	private float doorOpenDelay = 0.0f;

	public override void Awake(){
		
		// Always call back to base class Awake function
		base.Awake();

		statusLight = transform.parent.Find("doubleSlidingDoor/StatusLight").gameObject;
		lightSource = transform.Find("StatusLightSource").GetComponent<Light>();

	}

	void Start(){
	
		statusLight.GetComponent<Renderer>().material = lockedMaterial;
		lightSource.color = Color.red;

	}
	
	void Update(){

		if(errorLightTime > 0.0f){

			errorLightTime -= Time.deltaTime;

			if(errorLightBlinkInterval > 0.0f){

				errorLightBlinkInterval -= Time.deltaTime;

				if(errorLightBlinkInterval <= 0.0f){

					lightSource.enabled = !lightSource.enabled;
					errorLightBlinkInterval = 0.2f;

					if(statusLight.GetComponent<Renderer>().material == errorMaterial){

						statusLight.GetComponent<Renderer>().material = lockedMaterial;
						lightSource.color = Color.red;

					}else{

						statusLight.GetComponent<Renderer>().material = errorMaterial;
						lightSource.color = Color.yellow;

					}

				}

			}

			if(errorLightTime <= 0.0f){
				lightSource.enabled = true;
				statusLight.GetComponent<Renderer>().material = lockedMaterial;
				lightSource.color = Color.red;
			}

		}

		if(doorOpenDelay > 0.0f) {
			
			doorOpenDelay -= Time.deltaTime;
			
			if(doorOpenDelay <= 0.0f){
				gameObject.transform.parent.GetComponent<DemoSlidingDoorScript>().unlockTheDoor();
			}

		}

	}

	public override void activate(){

		// If the player has a keycard, unlock the door and open it
		if(GameObject.Find("FPEInteractionManager").GetComponent<FPEInventoryManagerScript>().hasInventoryItem(FPEInventoryManagerScript.eInventoryItems.Papers) > 0){

			if(gameObject.transform.parent.GetComponent<DemoSlidingDoorScript>().isDoorLocked()){

				doorOpenDelay = 1.0f;

				errorLightTime = 0.0f;
				statusLight.GetComponent<Renderer>().material = unlockedMaterial;
				lightSource.enabled = true;
				lightSource.color = Color.green;

				gameObject.GetComponent<AudioSource>().clip = doorUnlockSuccess;
				gameObject.GetComponent<AudioSource>().Play();

				interactionString = "";

			}

		}else{

			errorLightTime = 3.0f;
			gameObject.GetComponent<AudioSource>().clip = doorUnlockFailure;
			gameObject.GetComponent<AudioSource>().Play();

			interactionString = "I need a key card";

		}

	}

}
