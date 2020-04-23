using UnityEngine;
using System.Collections;

//
// FPEInteractableInventoryItemScript
// This script is the basis for all Inventory items. To create
// a new inventory item in the world, add this script, and choose
// the Inventory Type in the Inspector.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
[RequireComponent(typeof(AudioSource))]
public class FPEInteractableInventoryItemScript : FPEInteractableBaseScript {

	[Header("Inventory Type and Quantity")]
	[Tooltip("The type of inventory item this is.")]
	[SerializeField]
	private FPEInventoryManagerScript.eInventoryItems inventoryItemType;

	[Tooltip("The number of items of this type to give in inventory when this item is picked acquired (E.g. Box of 4 batteries would be 4). Default value is 1.")]
	[SerializeField]
	private int inventoryQuantity=1;

	[Header("Sound Management")]
	[Tooltip("Uncheck this if you don't want this object to make sounds")]
	[SerializeField]
	private bool enableSounds = true;
	[Tooltip("Inventory Get sound (optional). This sound is played when the inventory item is grabbed by the player. If no sound is specified, the generic inventory sound will be used instead.")]
	[SerializeField]
	private AudioClip inventoryGetSound;

	private bool hasBeenConsumed = false;

	public override void Awake(){

		base.Awake();
		interactionType = eInteractionType.INVENTORY;

		if(enableSounds){

			if(enableSounds && !gameObject.GetComponent<AudioSource>()){
				Debug.LogError("FPEInteractableInventoryItemScript:: Inventory object '" + gameObject.name + "' has sounds enabled, but the Game Object is missing an AudioSource. Either add an AudioSource component, or uncheck the enableSounds check box.");
			}

			gameObject.GetComponent<AudioSource>().loop = false;
			gameObject.GetComponent<AudioSource>().playOnAwake = false;
			
			// If no impact sounds are specified, just use the generic one
			if(!inventoryGetSound){
				inventoryGetSound = Resources.Load("genericInventoryGet") as AudioClip;
			}

		}

	}

	void Update(){

		if(hasBeenConsumed){

			if(enableSounds){

				if(!gameObject.GetComponent<AudioSource>().isPlaying){
					Destroy(gameObject);
				}

			}else{
				Destroy(gameObject);
			}

		}

	}

	public FPEInventoryManagerScript.eInventoryItems getInventoryItemType(){
		return inventoryItemType;
	}

	public int getInventoryQuantity(){
		return inventoryQuantity;
	}

	// Called when inventory item is "grabbed". Here you can do things like change it's appearance,
	// play sounds, etc.
	// E.g. Coin and Question Mark Blocks in mario turn slightly darker when they are emptied
	public void consumeInventoryItem(){

		if(!hasBeenConsumed){

			// Hide the object
			MeshRenderer[] childRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach(MeshRenderer mr in childRenderers){
				mr.enabled = false;
			}

			if(enableSounds){
				gameObject.GetComponent<AudioSource> ().clip = inventoryGetSound;
				gameObject.GetComponent<AudioSource> ().Play ();
			}

			hasBeenConsumed = true;

		}

	}

}
