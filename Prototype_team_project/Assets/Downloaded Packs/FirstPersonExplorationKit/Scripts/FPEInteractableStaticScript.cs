using UnityEngine;
using System.Collections;

//
// FPEInteractableStaticScript
// This script is for Static type Interactable objects. It simple performs the base
// highlight and interaction text for the object. These objects cannot be picked up 
// or moved or interacted with aside from "discovered"/"looked at".
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
public class FPEInteractableStaticScript : FPEInteractableBaseScript {

	public override void Awake(){
		
		base.Awake();
		interactionType = eInteractionType.STATIC;
		
	}

}
