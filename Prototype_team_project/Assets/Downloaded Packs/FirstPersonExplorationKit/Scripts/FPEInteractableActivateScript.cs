using UnityEngine;
using System.Collections;

//
// FPEInteractableActivateScript
// This script is the basis for all Activate type Interactable
// objects. This should never be assigned to a game object in
// your scene, but instead used as a base class for objects that
// can be activated. See Demo scripts for examples on how to make
// your own activate child type objects.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
public class FPEInteractableActivateScript : FPEInteractableBaseScript {

	public override void Awake(){

		base.Awake ();
		interactionType = eInteractionType.ACTIVATE;
		// You'll want to specify canInteractWithWhileHoldingObject value in child classes, depending on the object

	}

	public virtual void activate(){

		// Child classes of ACTIVATE must implement an override of this activate function. If you forget, you'll see this debug warning
		Debug.LogWarning("FPEInteractableActivateScript.activeate() - looks like you forgot to override in a child class.");
	
	}

}
