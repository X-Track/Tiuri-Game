using UnityEngine;
using System.Collections;

//
// DemoCabinetInteractionScript
// This script is attached in two places: the left and right cabinet door handles. It is an example
// of an object that can only be interacted with if the player has nothing in their hand.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
[RequireComponent(typeof (AudioSource))]
public class DemoCabinetInteractionScript : FPEInteractableActivateScript {

	public AudioClip cabinetOpen;
	public AudioClip cabinetClose;
	private GameObject cabinet;

	public override void Awake(){

		// Always call back to base class Awake function
		base.Awake();

		// The cabinet can only be opened with two hands
		canInteractWithWhileHoldingObject = false;

		cabinet = GameObject.Find("cabinet");
		if(!cabinet){
			Debug.LogError("DemoCabinetInteractionScript:: Cannot find cabinet Game Object!");
		}

	}

	public override void activate(){

		if(cabinet.GetComponent<DemoCabinetScript>().isCabinetOpen()){
			cabinet.GetComponent<DemoCabinetScript>().closeCabinet();
			gameObject.GetComponent<AudioSource>().clip = cabinetClose;
			gameObject.GetComponent<AudioSource>().Play();
		}else{
			cabinet.GetComponent<DemoCabinetScript>().openCabinet();
			gameObject.GetComponent<AudioSource>().clip = cabinetOpen;
			gameObject.GetComponent<AudioSource>().Play();
		}

	}

}
