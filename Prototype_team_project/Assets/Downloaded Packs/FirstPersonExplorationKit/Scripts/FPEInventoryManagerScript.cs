using UnityEngine;
using System.Collections;

//
// FPEInventoryManagerScript
// This script handles all player inventory items and 
// inventory state.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
public class FPEInventoryManagerScript : MonoBehaviour {

	// To add new inventory items, simply add them to the enum and increase the inventoryItemCount variable value.
	// E.g. add "MY_NEW_ITEM=1", and make inventoryItemCount = 2.
	//
	// Note: KEYCARD can be replaced or modified.
	public enum eInventoryItems { Papers=0 };

	private int inventoryItemCount = 1;
	private int[] theInventory;

	void Awake(){
	
		theInventory = new int[inventoryItemCount];

		for(int i = 0; i < inventoryItemCount; i++){
			theInventory[i] = 0;
		}

	}
	
	void Start(){
	
	}
	
	void Update(){
	
	}

	// Returns quantity of specified inventory item type.
	// If there are no such items in the inventory, zero is returned;
	public int hasInventoryItem(FPEInventoryManagerScript.eInventoryItems itemToCheck){
		return theInventory[(int)itemToCheck];
	}

	// Gives specified quanity of specified inventory item type
	public void giveInventoryItem(FPEInventoryManagerScript.eInventoryItems itemToGive, int quantity=1){
		theInventory[(int)itemToGive] += quantity;
	}

	// Tries to use specified quanitity of inventory item type. If insufficient inventory quantity is
	// available, it is not used and returns false. Otherwise returns true.
	public bool useInventoryItem(FPEInventoryManagerScript.eInventoryItems itemToUse, int quantity=1){

		bool success = false;

		if(theInventory[(int)itemToUse] >= quantity){
			theInventory[(int)itemToUse] -= quantity;
			success = true;
		}

		return success;

	}

}
