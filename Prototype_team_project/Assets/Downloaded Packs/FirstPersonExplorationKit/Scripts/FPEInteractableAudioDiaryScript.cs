using UnityEngine;
using System.Collections;

//
// FPEInteractableAudioDiaryScript
// This script is similar to the Static type, but it triggers the 
// playback of an Audio Diary or Log recording. This is ideal for
// significant game moments when story plot points or world lore 
// needs to be explained, voice overs are needed for tutorials or 
// player guidance, etc. When the audio is done playing or skipped, 
// the stop function is called, and the object can take on a new
// interaction string to correspond to the lore or plot points 
// explained in the audio.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//

public class FPEInteractableAudioDiaryScript : FPEInteractableBaseScript {

	[Tooltip("The audio diary title is displayed on screen when the diary is playing.")]
	public string audioDiaryTitle = "DEFAULT DIARY TITLE";
	[Tooltip("The actual audio clip the diary represents. This is played when the diary is triggered.")]
	public AudioClip audioDiaryClip;
	[Tooltip("The interacton string assigned after the audio diary has finished/been skipped. Leave blank to keep the same pre-diary interaction string.")]
	public string postPlaybackInteractionString = "";
	private bool hasBeenPlayed = false;
	private GameObject interactionManager = null;

	public override void Awake(){
		
		base.Awake();
		interactionType = eInteractionType.AUDIODIARY;

	}

	void Start(){

		interactionManager = GameObject.Find("FPEInteractionManager");
		if(!interactionManager){
			Debug.LogError("FPEInteractableAudioDiaryScript:: Cannot find Interaction Manager in FPE Interaction Manager!");
		}

	}
	
	public override void discoverObject(){

		// Always call base function
		base.discoverObject();

		if(!hasBeenPlayed){
			hasBeenPlayed = true;
			interactionManager.GetComponent<FPEInteractionManagerScript>().playNewAudioDiary(gameObject);
		}

	}

	public void stopAudioDiary(){

		if(postPlaybackInteractionString != ""){
			interactionString = postPlaybackInteractionString;
		}

	}

}
