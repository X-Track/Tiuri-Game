using UnityEngine;
using System.Collections;

//
// DemoRadioSimpleScript
// This script demonstrates the most basic extension of the
// Activate type of Interactable object. It just manages one
// state variable, and plays/stops a sound. See 
// DemoRadioComplexScript for a more involved version.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
[RequireComponent(typeof (AudioSource))]
public class DemoRadioSimpleScript : FPEInteractableActivateScript {

	private bool radioOn = false;
	
	public override void Awake(){
		
		// Always call back to base class Awake function
		base.Awake();

	}
	
	public override void activate(){
		
		if(radioOn){
			radioOn = false;
			interactionString = "Turn on simple radio";
			gameObject.GetComponent<AudioSource>().Stop();
		}else{
			radioOn = true;
			interactionString = "Turn off simple radio";
			gameObject.GetComponent<AudioSource>().Play();
		}
		
	}

}
