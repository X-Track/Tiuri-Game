using UnityEngine;
using System.Collections;

//
// DemoLevelBoxTriggerIndicator
// This script demonstrates how to create a trigger for 
// a Pickup type Interactable object. This type of script
// is useful for detecting if an object was put back or
// simply moved into a location in the game world.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
public class DemoLevelBoxTriggerIndicator : MonoBehaviour {

    private Vector3 myRotation = Vector3.zero;
	private bool destroyMe = false;
	public AudioClip alarmSound;
	private GameObject indicatorMesh;

	void Start(){

        myRotation.y = 0.8f;

		Transform[] ct = gameObject.GetComponentsInChildren<Transform> ();
		foreach (Transform t in ct) {
			if(t.name == "IndicatorMesh"){
				indicatorMesh = t.gameObject;
			}
		}

		if(!indicatorMesh){
			Debug.LogError("DemoLevelBoxTriggerIndicator:: Indicator Mesh is missing.");
		}

	}
	
	void Update(){

		indicatorMesh.transform.Rotate(myRotation);

		if(destroyMe && !gameObject.GetComponent<AudioSource>().isPlaying){
			Destroy(gameObject);
		}

	}

    void OnTriggerStay(Collider other){

		if(other.gameObject.name == "demoCardboardBoxSpecial"){

			if(other.GetComponent<FPEInteractablePickupScript>().isCurrentlyPickedUp() == false && !destroyMe){
				gameObject.GetComponent<AudioSource>().clip = alarmSound;
				gameObject.GetComponent<AudioSource>().Play();
				destroyMe = true;
			}

        }

    }
}
