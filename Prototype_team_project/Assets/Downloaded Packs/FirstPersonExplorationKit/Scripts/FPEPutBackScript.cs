using UnityEngine;
using System.Collections;

//
// FPEPutBackScript
// This script is for put back objects. In its simplist form, it's a 
// trigger collider. If the collider is not set to be a trigger, this
// will be toggled on Awake. The physics layer is also set to
// be FPEPutBackObjects.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//

[RequireComponent (typeof (Collider))]
public class FPEPutBackScript : MonoBehaviour {

	[Tooltip("If set in the inspector, this put back position will be tied to the assigned object. This allows for drag and drop assignment in the Scene editor.")]
	public GameObject myPickupObject = null;
	private int pickupObjectID = 0;

	void Awake(){

		if(!gameObject.GetComponent<Collider>().isTrigger){
			gameObject.GetComponent<Collider>().isTrigger = true;
		}

		gameObject.layer = LayerMask.NameToLayer("FPEPutBackObjects");

		if(myPickupObject != null) {
			pickupObjectID = myPickupObject.GetInstanceID();
		}

	}

	public int getPickupObjectID(){
		return pickupObjectID;
	}

	public void setPickupObjectID(int id){
		pickupObjectID = id;
	}

}
