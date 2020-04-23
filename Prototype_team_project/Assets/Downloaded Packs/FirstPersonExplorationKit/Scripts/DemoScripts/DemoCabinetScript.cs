using UnityEngine;
using System.Collections;

//
// DemoCabinetScript
// This script handles the core state management and animations for
// the cabinet.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
public class DemoCabinetScript : MonoBehaviour {

	private bool cabinetOpen = false;
	private GameObject doorOpenerLeft = null;
	private GameObject doorOpenerRight = null;

	void Awake(){

		doorOpenerLeft = GameObject.Find ("DoorOpenerLeft");
		doorOpenerRight = GameObject.Find ("DoorOpenerRight");

	}

	void Update(){

		/*
		if(Input.GetKeyDown(KeyCode.C)){

			if(cabinetOpen){
				closeCabinet();
			}else{
				openCabinet();
			}

		}
		*/

	}

	public void openCabinet(){
		doorOpenerLeft.GetComponent<FPEInteractableActivateScript>().interactionString = "Close cabinet";
		doorOpenerRight.GetComponent<FPEInteractableActivateScript>().interactionString = "Close cabinet";
		gameObject.GetComponent<Animator>().SetTrigger("OpenCabinet");
	}

	public void closeCabinet(){
		doorOpenerLeft.GetComponent<FPEInteractableActivateScript>().interactionString = "Open cabinet";
		doorOpenerRight.GetComponent<FPEInteractableActivateScript>().interactionString = "Open cabinet";
		gameObject.GetComponent<Animator>().SetTrigger("CloseCabinet");
	}

	public void setCabinetOpen(){
		cabinetOpen = true;
	}

	public void setCabinetClosed(){
		cabinetOpen = false;
	}

	public bool isCabinetOpen(){
		return cabinetOpen;
	}

}
