using UnityEngine;
using System.Collections;

//
// DemoToiletSeatScript
// This script is attached to the toilet seat and facilitates 
// interaction with it
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
[RequireComponent(typeof (AudioSource))]
public class DemoToiletSeatScript : FPEInteractableActivateScript {

	public AudioClip toiletSeatUp;
	public AudioClip toiletSeatDown;
	private GameObject toiletRoot;

	public override void Awake(){

		// Always call back to base class Awake function
		base.Awake();

		toiletRoot = GameObject.Find("toiletRoot");
		if(!toiletRoot){
			Debug.LogError("DemoToiletFlushScript:: Cannot find toiletRoot Game Object!");
		}

	}

	public override void activate(){

		if(toiletRoot.GetComponent<DemoToiletScript>().openCloseToiletSeat()){

			if(toiletRoot.GetComponent<DemoToiletScript>().isSeatUp()){
				gameObject.GetComponent<AudioSource>().clip = toiletSeatDown;
				gameObject.GetComponent<AudioSource>().Play();
				interactionString = "Lift seat";
			}else{
				gameObject.GetComponent<AudioSource>().clip = toiletSeatUp;
				gameObject.GetComponent<AudioSource>().Play();
				interactionString = "Put seat down";
			}

		}

	}

}
