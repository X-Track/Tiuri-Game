using UnityEngine;
using System.Collections;

//
// DemoIdolPickupScript
//
// This script is an example of how you make more elaborate and specialized
// Pickup type objects. This idol is a special artifact that triggers a
// sequence of theatrical events in the game world to advance the game.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
public class DemoIdolPickupScript : FPEInteractablePickupScript {

	[Header("Custom Idol Items")]
	public Material lightOn;
	public Material lightOff;
	public AudioClip stoneScrape;
	public AudioClip trapStartSound;
	public AudioClip trapReleaseSound;

	private GameObject trapTriggerPlate = null;
	private GameObject trapSign = null;
	private GameObject trapLight = null;
	private GameObject trapLightbulb = null;
	private GameObject trapBars = null;
	private bool idolRemoved = false;
	private bool startTrap = false;
	private Vector3 signPosition = Vector3.zero;
	private Vector3 platePosition = Vector3.zero;
	private bool plateDoneMoving = false;
	private float barsDisableCounter = 2.5f;
	private bool trapStartSoundPlayed = false;
	private bool trapReleaseSoundPlayed = false;
	private bool releaseBars = false;
	private Vector3 releasedBarsPosition = Vector3.zero;
	private Vector3 barsLockedPosition = Vector3.zero;

	public override void Awake(){

		// Always call base Awake
		base.Awake();

		trapTriggerPlate = GameObject.Find("TrapTriggerPlate");
		trapSign = GameObject.Find("TrapSign");
		trapLight = GameObject.Find("TrapLight");
		trapLightbulb = GameObject.Find("TrapLightBulb");
		trapBars = GameObject.Find("TrapBars");

		if(!trapTriggerPlate || !trapSign || !trapLight || !trapLightbulb || !trapBars){
			Debug.LogError("DemoIdolPickupScript:: Objects are missing. Did you break or delete the demoTrap prefab?");
		}

		signPosition = trapSign.transform.position;
		signPosition.z += 0.8f;

		platePosition = trapTriggerPlate.transform.position;
		platePosition.y -= 0.1f;

		barsLockedPosition = trapBars.transform.position;
		releasedBarsPosition = trapBars.transform.position;
		releasedBarsPosition.y -= 3.0f;

		trapBars.transform.position = releasedBarsPosition;

	}

	// This update function handles the base Update call, and does some other fancy custom state and event stuff for the idol
	public override void Update(){

		// Always call base Update
		base.Update();

		// The idol trips a trap, and here we handle the states and events for the trap sequence
		if(startTrap){

			trapTriggerPlate.transform.position = Vector3.Lerp(trapTriggerPlate.transform.position, platePosition, 0.015f);

			if(releaseBars){
				trapBars.transform.position = Vector3.Lerp(trapBars.transform.position, releasedBarsPosition, 0.25f);
			}else{
				trapBars.GetComponent<BoxCollider>().enabled = true;
				trapBars.transform.position = barsLockedPosition;
			}

			if(!plateDoneMoving && Vector3.Distance(trapTriggerPlate.transform.position, platePosition) < 0.01f){
				plateDoneMoving = true;
				trapLight.GetComponent<Light>().color = Color.red;
				trapLightbulb.GetComponent<MeshRenderer>().material = lightOff;
			}

			if(plateDoneMoving){

				if(!trapStartSoundPlayed){
					trapStartSoundPlayed = true;
					trapTriggerPlate.GetComponent<AudioSource>().clip = trapStartSound;
					trapTriggerPlate.GetComponent<AudioSource>().Play();
				}

				trapSign.transform.position = Vector3.Lerp(trapSign.transform.position, signPosition, 0.25f);

			}

			if(Vector3.Distance(trapSign.transform.position, signPosition) < 0.01f){

				barsDisableCounter -= Time.deltaTime;

				if(barsDisableCounter <= 0.0f){

					if(!trapReleaseSoundPlayed){
						trapReleaseSoundPlayed = true;
						trapTriggerPlate.GetComponent<AudioSource>().clip = trapReleaseSound;
						trapTriggerPlate.GetComponent<AudioSource>().Play();
					}

					releaseBars = true;

					trapLight.GetComponent<Light>().enabled = false;

				}

			}

		}

	}

	public override void doPickupPutdown(bool putback){

		// Always call base doPickupPutdown
		base.doPickupPutdown(putback);

		// Once base pickup/putdown is handled, let's do something unique for this object.
		// When the idol is picked up, let's set off a trap
		if(pickedUp && !idolRemoved){

			idolRemoved = true;
			startTrap = true;

			if(!trapTriggerPlate.GetComponent<AudioSource>().isPlaying){
				trapTriggerPlate.GetComponent<AudioSource>().clip = stoneScrape;
				trapTriggerPlate.GetComponent<AudioSource>().Play();
			}

		}

		// Here, we replace the interactionString when we return the idol to reflect
		// the harrowing events that resulted in its retrieval :)
		if(putback){
			interactionString = "It's the artifact I returned. Nearly died for this thing.";
		}

	}

}
