using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//
// FPEInteractionManagerScript
// This script handles all player actions with respect to Interactable
// objects in the game world. All hitscan detection, keyboard/mouse 
// input object manipulation, etc. occurs in this script.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
public class FPEInteractionManagerScript : MonoBehaviour {

	[Header("Examination Options")]
	[Tooltip("When examining an object, it will only rotate if axis input is greater than this deadzone value. Default is 0.1.")]
	public float examinationDeadzone = 0.1f;
	[Tooltip("When examining an object, this value acts as a multiplier to Input Mouse X/Y values. Default is 5.8.")]
	public float examineRotationSpeed = 5.8f;
	[Header("Reticle")]
	[Tooltip("Uncheck this to disable reticle completely")]
	public bool reticleEnabled = true;
	[Tooltip("Uncheck this to disable Interaction Text completely")]
	public bool interactionTextEnabled = true;
	[Tooltip("Reticle sprite when it is inactive")]
	public Sprite inactiveReticle;
	[Tooltip("Reticle sprite when it is active")]
	public Sprite activeReticle;
	[Header("Special Interaction Masks")]
	[Tooltip("This should be assigned to FPEPutbackObjects layer")]
	public LayerMask putbackLayerMask;
	[Tooltip("This should be assigned to be mixed to include everything except the FPEPutbackObjects and FPEIgnore layers")]
	public LayerMask interactionLayerMask;

	// Max range you can interact with an object. Note that interactions are governed ultimately by the Interactable Object's interactionDistance.
	private float interactionRange = Mathf.Infinity;
	private RectTransform interactionLabel;
	private Vector3 interactionLabelTargetScale = Vector3.zero;
	private Vector3 interactionLabelLargestScale = Vector3.zero;
	private Vector3 interactionLabelSmallestScale = Vector3.zero;
	private RectTransform reticle;
	// Journal stuff
	private RectTransform journalCloseButton;
	private RectTransform journalPreviousButton;
	private RectTransform journalNextButton;
	private RectTransform journalBackground;
	private RectTransform journalPage;
	private Sprite[] currentJournalPages = null;
	private int currentJournalPageIndex = 0;
	//Audio diary stuff
	private RectTransform audioDiaryLabel;
	private Vector3 audioDiaryLabelTargetScale = Vector3.zero;
	private Vector3 audioDiaryLabelLargestScale = Vector3.zero;
	private Vector3 audioDiaryLabelSmallestScale = Vector3.zero;
	private bool playingAudioDiary = false;
	private GameObject currentAudioDiary = null;
	private bool fadingDiaryText = false;
	private Color defaultDiaryColor;
	private Color diaryFadeColor;
	// Object Interaction Stuff
	private GameObject currentInteractableObject = null;
	private GameObject currentHeldObject = null;
	private GameObject currentPutbackObject = null;
	private GameObject interactionObjectPickupLocation = null;
	private GameObject interactionObjectExamineLocation = null;
	private GameObject interactionObjectTossLocation = null;
	private GameObject audioDiaryPlayer = null;
	private GameObject journalSFXPlayer = null;

	private bool examiningObject = false;

	// Camera zoom and mouse stuff
	[Header("Mouse Zoom and Sensitivity Options")]
	[Tooltip("This is the FOV the camera will use when player Right-Clicks to zoom in. Un-zoomed FOV is set to be that of Main Camera when scene starts. If you change FOV in Main Camera, also change it in ExaminationCamera.")]
	public float zoomedFOV = 24.0f;
	private float unZoomedFOV = 0.0f;
	private bool cameraZoomedIn = false;
	private float cameraZoomChangeRate = 0.1f;
	[Tooltip("Mouse sensitivity when zoomed")]
	public Vector2 zoomedMouseSensitivity = new Vector2(1.5f,1.5f);
	[Tooltip("Apply special mouse sensitivity when reticle is over an interactable object")]
	public bool slowMouseOnInteractableObjectHighlight = true;
	[Tooltip("Mouse sensitivity when reticle is over an interactable object")]
	public Vector2 highlightedMouseSensitivity = new Vector2(1.5f, 1.5f);
	private Vector2 startingMouseSensitivity = Vector2.zero;
	// Used to ensure smooth sensitivity changes when mouse is slowed on reticle highlight of object vs. unhighlighted
	private Vector2 targetMouseSensitivity = Vector2.zero;
	private bool smoothMouseChange = false;
	private float smoothMouseChangeRate = 0.5f;
	private GameObject thePlayer = null;
	private bool mouseLookEnabled = true;

	// Examination stuff
	Quaternion lastObjectHeldRotation = Quaternion.identity;
	// This is multiplied with tossStrength of held object. Seems to be an okay value.
	private float tossImpulseFactor = 2.5f;

	// Journal stuff
	[Header("The sounds journals make when used. Use 2D sounds for best effect.")]
	public AudioClip journalOpen;
	public AudioClip journalClose;
	public AudioClip journalPageTurn;
	private GameObject currentJournal = null;

	[Header("Control Hints UI")]
	[Tooltip("Toggle mouse control hints UI on and off")]
	public bool showMouseControlHints = true;
	[Tooltip("Text hint for Pick Up action")]
	public string mouseHintPickUpText = "Pick Up";
	[Tooltip("Text hint for Put Back action")]
	public string mouseHintPutBackText = "Put Back";
	[Tooltip("Text hint for Examine action")]
	public string mouseHintExamineText = "Examine";
	[Tooltip("Text hint for Drop action")]
	public string mouseHintDropText = "Drop";
	[Tooltip("Text hint for Zoom In action")]
	public string mouseHintZoomText = "Zoom In";
	[Tooltip("Text hint for Activate action")]
	public string mouseHintActivateText = "Activate";
	[Tooltip("Text hint for Journal Read action")]
	public string mouseHintJournalText = "Read";
	private RectTransform mouseLMBHelperIcon;
	private RectTransform mouseLMBHelperText;
	private RectTransform mouseRMBHelperIcon;
	private RectTransform mouseRMBHelperText;

	// Audio Diary stuff
	[Tooltip("Volume fade out amount per 100ms (0.0 to 1.0, with 1.0 being 100% of the volume)")]
	public float fadeAmountPerTenthSecond = 0.1f;
	private bool fadingDiaryAudio = false;
	private float fadeCounter = 0.0f;

	void Awake(){

	}

	void Start(){
	
		RectTransform[] childObjects = gameObject.GetComponentsInChildren<RectTransform>();

		foreach(RectTransform rt in childObjects){

			if(rt.transform.name == "Reticle"){
				reticle = rt;
			}else if(rt.transform.name == "InteractionTextLabel"){
				interactionLabel = rt;
			}else if(rt.transform.name == "CloseButton"){
				journalCloseButton = rt;
			}else if(rt.transform.name == "PreviousButton"){
				journalPreviousButton = rt;
			}else if(rt.transform.name == "NextButton"){
				journalNextButton = rt;
			}else if(rt.transform.name == "JournalBackground"){
				journalBackground = rt;
			}else if(rt.transform.name == "JournalPage"){
				journalPage = rt;
			}else if(rt.transform.name == "AudioDiaryTitleLabel"){
				audioDiaryLabel = rt;
			}else if(rt.transform.name == "MouseLMBHelperIcon"){
				mouseLMBHelperIcon = rt;
			}else if(rt.transform.name == "MouseLMBHelperText"){
				mouseLMBHelperText = rt;
			}else if(rt.transform.name == "MouseRMBHelperIcon"){
				mouseRMBHelperIcon = rt;
			}else if(rt.transform.name == "MouseRMBHelperText"){
				mouseRMBHelperText = rt;
			}

		}

		// UI component error check
		if(!reticle || ! interactionLabel || !audioDiaryLabel || !journalCloseButton || !journalPreviousButton || !journalNextButton || !journalBackground || !journalPage){
			Debug.LogError("FPEInteractionManagerScript:: UI and/or Journal Components are missing. Did you change the FPEInteractionManager prefab?");
		}

		if(!reticleEnabled){
			reticle.GetComponentInChildren<Image>().enabled = false;
		}

		if(!interactionTextEnabled){
			interactionLabel.GetComponentInChildren<Text>().enabled = false;
		}

		// Mouse controls hint UI error check
		if(!mouseLMBHelperIcon || !mouseLMBHelperText || !mouseRMBHelperIcon || !mouseRMBHelperText){
			Debug.LogError("FPEInteractionManagerScript:: UI Components for Mouse Control hints are missing. Did you change the FPEInteractionManager prefab?");
		}

		interactionObjectPickupLocation = GameObject.Find("ObjectPickupLocation");
		interactionObjectExamineLocation = GameObject.Find("ObjectExamineLocation");
		interactionObjectTossLocation = GameObject.Find("ObjectTossLocation");
		thePlayer = GameObject.FindGameObjectWithTag("Player");
		audioDiaryPlayer = GameObject.Find("FPEAudioDiaryPlayer");
		journalSFXPlayer = GameObject.Find("FPEJournalSFX");

		// Note: If you wish, you can remove the Mesh children "Sphere" game objects from these 3 parent game objects.
		// However, it is probably a good idea to just keep them and perhaps disable them in the inspector so you can
		// tweak positions in the scene editor visually, rather than by trial and error.
		interactionObjectPickupLocation.GetComponentInChildren<MeshRenderer>().enabled = false;
		interactionObjectExamineLocation.GetComponentInChildren<MeshRenderer>().enabled = false;
		interactionObjectTossLocation.GetComponentInChildren<MeshRenderer>().enabled = false;

		if(!thePlayer || !interactionObjectPickupLocation || !interactionObjectExamineLocation || !interactionObjectTossLocation){
			Debug.LogError("FPEInteractionManagerScript:: Player or its components are missing. Did you change the Player Controller prefab, or forget to tag with 'Player' tag?");
		}

		if(!audioDiaryPlayer || !journalSFXPlayer){
			Debug.LogError("FPEInteractionManagerScript:: FPEAudioDiaryPlayer and/or FPEJournalSFX are missing from Player Controller. Did you break the FPEPlayerController prefab or forget to add one or both of these prefabs to your player controller?");
		}

		rememberStartingMouseSensitivity();

		interactionLabelLargestScale = new Vector3(1.0f,1.0f,1.0f);
		interactionLabelSmallestScale = new Vector3(0.0f,0.0f,0.0f);

		audioDiaryLabelLargestScale = new Vector3(1.1f,1.1f,1.1f);
		audioDiaryLabelSmallestScale = new Vector3(0.9f,0.9f,0.9f);

		defaultDiaryColor = audioDiaryLabel.GetComponent<Text>().color;
		diaryFadeColor = audioDiaryLabel.GetComponent<Text>().color;
		diaryFadeColor.a = 0.0f;

		unZoomedFOV = Camera.main.fieldOfView;

		closeJournal(false);
		hideAudioDiaryTitle();
		setMouseHints("","");

	}
	
	void Update(){

		Ray rayInteractable = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		RaycastHit hitInteractable;
		
		// If we see an Interactable object, check to see if we can in fact interact with it
		if(Physics.Raycast(rayInteractable, out hitInteractable, interactionRange, interactionLayerMask)){
			
			if(hitInteractable.transform.gameObject.GetComponent<FPEInteractableBaseScript>() && (hitInteractable.distance < hitInteractable.transform.gameObject.GetComponent<FPEInteractableBaseScript>().getInteractionDistance())){
				
				//Unhighlight current interactable object, if it existed
				if(currentInteractableObject){
					currentInteractableObject.GetComponent<FPEInteractableBaseScript>().unHighlightObject();
					currentInteractableObject = null;
				}

				currentInteractableObject = hitInteractable.transform.gameObject;

				// If am not holding anything, or I am holding something but the highlighted object allows for interactions when holding objects, then we highlight the object
				if((!currentHeldObject) || (currentHeldObject && currentInteractableObject.GetComponent<FPEInteractableBaseScript>().interactionsAllowedWhenHoldingObject())){
						
					activateReticle(hitInteractable.transform.gameObject.GetComponent<FPEInteractableBaseScript>().interactionString);

					currentInteractableObject.GetComponent<FPEInteractableBaseScript>().highlightObject();
					
					if(slowMouseOnInteractableObjectHighlight){
						setMouseSensitivity(highlightedMouseSensitivity);
					}

					if(showMouseControlHints){
						
						FPEInteractableBaseScript.eInteractionType tempInteractionType = hitInteractable.transform.gameObject.GetComponent<FPEInteractableBaseScript>().getInteractionType();

						if(currentHeldObject){

							// When holding something, our mouse hints still always remain "drop"/"examine"
							// They become "put back" if we see a put back spot later in this script.
							switch(tempInteractionType){
								
								case FPEInteractableBaseScript.eInteractionType.ACTIVATE:
								case FPEInteractableBaseScript.eInteractionType.JOURNAL:
								case FPEInteractableBaseScript.eInteractionType.PICKUP:
								case FPEInteractableBaseScript.eInteractionType.STATIC:
                                case FPEInteractableBaseScript.eInteractionType.PAPERS:
                                default:
									setMouseHints(mouseHintDropText,mouseHintExamineText);
									break;
								
							}

						}else{

							switch(tempInteractionType){
								
								case FPEInteractableBaseScript.eInteractionType.ACTIVATE:
									setMouseHints(mouseHintActivateText,mouseHintZoomText);
									break;
								case FPEInteractableBaseScript.eInteractionType.JOURNAL:
									setMouseHints(mouseHintJournalText,mouseHintZoomText);
									break;
								case FPEInteractableBaseScript.eInteractionType.PICKUP:
									setMouseHints(mouseHintPickUpText,mouseHintZoomText);
									break;
                                case FPEInteractableBaseScript.eInteractionType.PAPERS:
                                    setMouseHints(mouseHintPickUpText, mouseHintZoomText);
                                    break;
                                case FPEInteractableBaseScript.eInteractionType.STATIC:
								default:
									setMouseHints("",mouseHintZoomText);
									break;
								
							}

						}
						
					}

				}

			}else{
				
				if(currentInteractableObject){
					currentInteractableObject.GetComponent<FPEInteractableBaseScript>().unHighlightObject();
					currentInteractableObject = null;
				}
				
				deactivateReticle();
				
				if(showMouseControlHints){
					if(currentJournal == null){
						setMouseHints("",mouseHintZoomText);
					}else{
						setMouseHints("","");
					}
				}
				
				if(!cameraZoomedIn){
					restorePreviousMouseSensitivity(true);
				}
				
			}
			
		}else{

			if(currentInteractableObject){
				currentInteractableObject.GetComponent<FPEInteractableBaseScript>().unHighlightObject();
				currentInteractableObject = null;
			}

			currentPutbackObject = null;
			
			deactivateReticle();
			
			if(showMouseControlHints){
				if(currentJournal == null){
					setMouseHints("",mouseHintZoomText);
				}else{
					setMouseHints("","");
				}
			}
			
			if(!cameraZoomedIn){
				restorePreviousMouseSensitivity(true);
			}
			
		}

		// Behaviours when holding an object //
		if(currentHeldObject){

			if(examiningObject){

				// Examination logic: Position, Rotation, etc.
				float examinationOffsetUp = currentHeldObject.GetComponent<FPEInteractablePickupScript>().examinationOffsetUp;
				float examinationOffsetForward = currentHeldObject.GetComponent<FPEInteractablePickupScript>().examinationOffsetForward;
				currentHeldObject.transform.position = interactionObjectExamineLocation.transform.position + Vector3.up * examinationOffsetUp + interactionObjectExamineLocation.transform.forward * examinationOffsetForward;

				float rotationInputX = 0.0f;
				float rotationInputY = 0.0f;

				// If mouse input is zero, use gamepad to rotate examination object instead
				float examinationChangeX = Input.GetAxis("Mouse X");
				if(examinationChangeX == 0.0f){
					examinationChangeX = Input.GetAxis("Gamepad Look X");
				}

				float examinationChangeY = Input.GetAxis("Mouse Y");
				if(examinationChangeY == 0.0f){
					examinationChangeY = -Input.GetAxis("Gamepad Look Y");
				}

				if(Mathf.Abs(examinationChangeX) > examinationDeadzone){
					rotationInputX = -(examinationChangeX * examineRotationSpeed);
				}
				
				if(Mathf.Abs(examinationChangeY) > examinationDeadzone){
					rotationInputY = (examinationChangeY * examineRotationSpeed);
				}

				switch(currentHeldObject.GetComponent<FPEInteractablePickupScript>().rotationLockType){

					case FPEInteractablePickupScript.eRotationType.FREE:
						currentHeldObject.transform.Rotate(interactionObjectExamineLocation.transform.up, rotationInputX, Space.World);
						currentHeldObject.transform.Rotate(interactionObjectExamineLocation.transform.right, rotationInputY, Space.World);
						break;
					case FPEInteractablePickupScript.eRotationType.HORIZONTAL:
						currentHeldObject.transform.Rotate(interactionObjectExamineLocation.transform.up, rotationInputX, Space.World);
						break;
					case FPEInteractablePickupScript.eRotationType.VERTICAL:
						currentHeldObject.transform.Rotate(interactionObjectExamineLocation.transform.right, rotationInputY, Space.World);
						break;
					case FPEInteractablePickupScript.eRotationType.NONE:
					default:
						break;

				}

				if(showMouseControlHints){
					setMouseHints("","");
				}

			}else{

				// Update position of object to be that of holding position
				currentHeldObject.transform.position = interactionObjectPickupLocation.transform.position;
				// Lerp a bit so it feels less rigid and more like holding something in real life
				currentHeldObject.transform.rotation = Quaternion.Slerp(currentHeldObject.transform.rotation, interactionObjectPickupLocation.transform.rotation * Quaternion.Euler(lastObjectHeldRotation.eulerAngles), 0.2f);

				Ray rayPutBack = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
				RaycastHit hitPutBack;

				// If we see a Put Back location, see if it matches our currently held object
				if(Physics.Raycast(rayPutBack, out hitPutBack, interactionRange, putbackLayerMask)){

					if(hitPutBack.transform.gameObject.GetComponent<FPEPutBackScript>() && hitPutBack.transform.gameObject.GetComponent<FPEPutBackScript>().getPickupObjectID() == currentHeldObject.GetInstanceID()){
						currentPutbackObject = hitPutBack.transform.gameObject;
						activateReticle(currentHeldObject.GetComponent<FPEInteractablePickupScript>().putBackString);
						if(showMouseControlHints){
							setMouseHints(mouseHintPutBackText,mouseHintExamineText);
						}
					}else{
						currentPutbackObject = null;
						deactivateReticle();
						if(showMouseControlHints){
							setMouseHints(mouseHintDropText,mouseHintExamineText);
						}
					}
					
				}else{

					currentPutbackObject = null;

					// If I didn't see a put back location, and also don't see another interactable, clear reticle and mouse hints as required
					if(!currentInteractableObject){
						deactivateReticle();
						if(showMouseControlHints){
							setMouseHints(mouseHintDropText,mouseHintExamineText);
						}
					}

					if(!cameraZoomedIn){
						restorePreviousMouseSensitivity(true);
					}

				}

			}

		}

		// Pick up / Put down / Interact / Read / Activate //
		if((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0) || Input.GetButtonDown("Gamepad Interact")) && !examiningObject){

			if(currentHeldObject){

				// If I am looking at the put back spot for the object in hand
				if(currentPutbackObject){

					currentHeldObject.GetComponent<FPEInteractablePickupScript>().doPickupPutdown(true);

					currentHeldObject.transform.position = currentPutbackObject.transform.position;
					currentHeldObject.transform.rotation = currentPutbackObject.transform.rotation;
					currentHeldObject.transform.parent = null;
					currentHeldObject.GetComponent<Collider>().isTrigger = false;
					currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
					currentHeldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
					currentHeldObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

					Transform[] objectTransforms = currentHeldObject.GetComponentsInChildren<Transform>();
					foreach(Transform t in objectTransforms){
						t.gameObject.layer = LayerMask.NameToLayer("FPEPickupObjects");
					}

					currentHeldObject = null;

					// Set scale of text to smallest size to create a pseudo animation between seeing "pick up" and "put back" strings
					interactionLabel.localScale = interactionLabelSmallestScale;
						
				}else if(currentInteractableObject && currentInteractableObject.GetComponent<FPEInteractableBaseScript>().interactionsAllowedWhenHoldingObject()){

                    if (currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.STATIC)
                    {

                        currentInteractableObject.GetComponent<FPEInteractableBaseScript>().interact();

                    }
                    else if (currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.ACTIVATE)
                    {

                        currentInteractableObject.GetComponent<FPEInteractableActivateScript>().activate();

                    }
                    else if (currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.PICKUP)
                    {
                        // No action for Pickup when holding something
                    }
                    else if (currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.AUDIODIARY)
                    {
                        // No action for Audio Diary when holding something
                    }
                    else if (currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.JOURNAL)
                    {
                        // No action for Journal when holding something
                    }
                    else if (currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.INVENTORY)
                    {

                        gameObject.GetComponent<FPEInventoryManagerScript>().giveInventoryItem(currentInteractableObject.GetComponent<FPEInteractableInventoryItemScript>().getInventoryItemType(), currentInteractableObject.GetComponent<FPEInteractableInventoryItemScript>().getInventoryQuantity());
                        currentInteractableObject.GetComponent<FPEInteractableInventoryItemScript>().consumeInventoryItem();

                    }
                    else if (currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.PAPERS)
                    {
                        gameObject.GetComponent<FPEInteractablePaperScript>().GivePapersToMe();
                    }

                    /*
					// Note: You can add a case here to handle any new interaction types you create that should also allow interactions when object is being held. Be sure to add it to the eInteractionType enum in FPEInteractableBaseScript
					else if(currentInteractionObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.YOUR_NEW_TYPE_HERE){
						// YOUR CUSTOM INTERACTION LOGIC HERE
					}
					*/
                    else
                    {
                        Debug.LogWarning("FPEInteractionManagerScript:: Unhandled eInteractionType '" + currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() + "' for object that allows interaction when object being held. No case exists to manage this.");
                    }

				}else{

					currentHeldObject.GetComponent<FPEInteractablePickupScript>().doPickupPutdown(false);

					// If we are not putting back, just toss the object
					currentHeldObject.transform.parent = null;
					currentHeldObject.GetComponent<Collider>().isTrigger = false;
					currentHeldObject.GetComponent<Rigidbody>().isKinematic = false;
					// Note: We move objects to a special toss location to prevent clipping into the player if the player tosses the object while walking forward
					float tossStrength = currentHeldObject.GetComponent<FPEInteractablePickupScript>().tossStrength;
					float tossOffsetUp = currentHeldObject.GetComponent<FPEInteractablePickupScript>().tossOffsetUp;
					float tossOffsetForward = currentHeldObject.GetComponent<FPEInteractablePickupScript>().tossOffsetForward;
					currentHeldObject.transform.position = interactionObjectTossLocation.transform.position + Vector3.up * tossOffsetUp + Camera.main.transform.forward * tossOffsetForward;
					currentHeldObject.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * tossImpulseFactor * tossStrength, ForceMode.Impulse);

					Transform[] objectTransforms = currentHeldObject.GetComponentsInChildren<Transform>();
					foreach(Transform t in objectTransforms){
						t.gameObject.layer = LayerMask.NameToLayer("FPEPickupObjects");
					}

					currentHeldObject = null;

				}

			}else{

				// If we're looking at an object, we need to handle the various interaction types (pickup, activate, etc.)
				if(currentInteractableObject){

					lastObjectHeldRotation = Quaternion.identity;

					if(currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.PICKUP){

						currentInteractableObject.GetComponent<FPEInteractablePickupScript>().doPickupPutdown(false);

						currentInteractableObject.GetComponent<FPEInteractableBaseScript>().unHighlightObject();
						currentInteractableObject.GetComponent<Rigidbody>().isKinematic = true;
						currentInteractableObject.GetComponent<Collider>().isTrigger = true;

						currentInteractableObject.transform.position = interactionObjectPickupLocation.transform.position;

						currentHeldObject = currentInteractableObject;
						currentInteractableObject = null;

						interactionLabel.localScale = interactionLabelSmallestScale;

						// Move to examination layer so object being held/examined doesn't clip through other objects.
						Transform[] objectTransforms = currentHeldObject.GetComponentsInChildren<Transform>();
						foreach(Transform t in objectTransforms){
							t.gameObject.layer = LayerMask.NameToLayer("FPEObjectExamination");
						}

						// Un-zoom, restore mouse state
						cameraZoomedIn = false;
						restorePreviousMouseSensitivity(false);

					}else if(currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.STATIC){

						currentInteractableObject.GetComponent<FPEInteractableBaseScript>().interact();

					}else if(currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.JOURNAL){

						currentInteractableObject.GetComponent<FPEInteractableJournalScript>().activateJournal();
						currentJournal = currentInteractableObject;
						cameraZoomedIn = false;
						restorePreviousMouseSensitivity(false);
						openJournal();
						
					}else if(currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.ACTIVATE){

						currentInteractableObject.GetComponent<FPEInteractableActivateScript>().activate();
						
					}else if(currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.INVENTORY){

						gameObject.GetComponent<FPEInventoryManagerScript>().giveInventoryItem(currentInteractableObject.GetComponent<FPEInteractableInventoryItemScript>().getInventoryItemType(),currentInteractableObject.GetComponent<FPEInteractableInventoryItemScript>().getInventoryQuantity());
						currentInteractableObject.GetComponent<FPEInteractableInventoryItemScript>().consumeInventoryItem();
						
					}
                    else if (currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.ACTIVATE)
                    {

                        currentInteractableObject.GetComponent<FPEInteractableActivateScript>().activate();

                    }
                    else if (currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.PAPERS)
                    {

                        currentInteractableObject.GetComponent<FPEInteractablePaperScript>().GivePapersToMe();

                    }
                    /*
					// Note: You can add a case here to handle any new interaction types you create. Be sure to add it to the eInteractionType enum in FPEInteractableBaseScript
					else if(currentInteractionObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() == FPEInteractableBaseScript.eInteractionType.YOUR_NEW_TYPE_HERE){
						// YOUR CUSTOM INTERACTION LOGIC HERE
					}
					*/
                    else
                    {
						Debug.LogWarning("FPEInteractionManagerScript:: Unhandled eInteractionType '" + currentInteractableObject.GetComponent<FPEInteractableBaseScript>().getInteractionType() + "'. No case exists to manage this.");
					}

				}

			}

		}

		// Examine held object
		if((Input.GetMouseButtonDown(1) || Input.GetButtonDown("Gamepad Examine")) && currentHeldObject){

			hideReticleAndInteractionLabel();
			examiningObject = true;
			disableMouseLook();
			disableMovement();

			if(currentHeldObject.GetComponent<FPEInteractablePickupScript>().postExaminationInteractionString != ""){
				currentHeldObject.GetComponent<FPEInteractablePickupScript>().interactionString = currentHeldObject.GetComponent<FPEInteractablePickupScript>().postExaminationInteractionString;
			}

			currentHeldObject.transform.position = interactionObjectExamineLocation.transform.position;

			if(currentHeldObject.GetComponent<FPEInteractablePickupScript>().rotationLockType == FPEInteractablePickupScript.eRotationType.FREE){

				if(lastObjectHeldRotation == Quaternion.identity){
					
					Vector3 relativePos = Camera.main.transform.position - currentHeldObject.transform.position;
					Quaternion rotation = Quaternion.LookRotation(relativePos);
					currentHeldObject.transform.rotation = rotation;
					
				}else{
					currentHeldObject.transform.rotation = lastObjectHeldRotation;
				}

			}else{

				Vector3 relativePos = Camera.main.transform.position - currentHeldObject.transform.position;
				Quaternion rotation = Quaternion.LookRotation(relativePos);
				currentHeldObject.transform.rotation = rotation;

			}

		}

		// Stop examining held object //
		if((Input.GetMouseButtonUp(1) || Input.GetButtonUp("Gamepad Examine")) && currentHeldObject){

			lastObjectHeldRotation = currentHeldObject.transform.rotation;
			showReticleAndInteractionLabel();
			examiningObject = false;
			enableMouseLook();
			enableMovement();

		}

		// Skip Audio Diary //
		if(Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Gamepad Close")){

			if(currentAudioDiary){

				currentAudioDiary.GetComponent<FPEInteractableAudioDiaryScript>().stopAudioDiary();
				fadingDiaryAudio = true;
				fadingDiaryText = true;

			}

		}

		// Camera Zoom - don't allow when holding an object or reading a journal //
		if(currentHeldObject == null && currentJournal == null){

			if(Input.GetMouseButtonDown(1) || Input.GetButtonDown("Gamepad Examine")){
				setMouseSensitivity(zoomedMouseSensitivity);
			}
			if(Input.GetMouseButtonUp(1) || Input.GetButtonUp("Gamepad Examine")){
				restorePreviousMouseSensitivity(false);
			}
			if(Input.GetMouseButton(1) || Input.GetButton("Gamepad Examine")){
				cameraZoomedIn = true;
			}else{
				cameraZoomedIn = false;
			}

		}

		// Change actual camera FOV based on zoom state //
		if(cameraZoomedIn){
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomedFOV, cameraZoomChangeRate);
		}else{
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, unZoomedFOV, cameraZoomChangeRate);
		}

		// Animate the size of the interaction text //
		interactionLabel.localScale = Vector3.Lerp(interactionLabel.localScale, interactionLabelTargetScale, 0.25f);

		// Animate audio diary title when visible //
		if(playingAudioDiary){

			audioDiaryLabel.localScale = Vector3.Lerp(audioDiaryLabel.localScale, audioDiaryLabelTargetScale, 0.01f);

			if((audioDiaryLabelTargetScale == audioDiaryLabelLargestScale) && (Vector3.Distance(audioDiaryLabel.localScale, audioDiaryLabelLargestScale) < 0.1f)){
				audioDiaryLabelTargetScale = audioDiaryLabelSmallestScale;
			}else if((audioDiaryLabelTargetScale == audioDiaryLabelSmallestScale) && (Vector3.Distance(audioDiaryLabel.localScale, audioDiaryLabelSmallestScale) < 0.1f)){
				audioDiaryLabelTargetScale = audioDiaryLabelLargestScale;
			}

		}

		// Fade out diary text when done playing //
		if(fadingDiaryText){

			audioDiaryLabel.GetComponent<Text>().color = Color.Lerp(audioDiaryLabel.GetComponent<Text>().color, diaryFadeColor, 0.1f);

			if(audioDiaryLabel.GetComponent<Text>().color.a <= 0.1f){
				audioDiaryLabel.GetComponent<Text>().text = "";
				audioDiaryLabel.GetComponent<Text>().color = defaultDiaryColor;
				fadingDiaryText = false;
			}

		}

		// Fading out diary audio track //

		// If playback has got to the end, stop diary and hide text
		if(playingAudioDiary && !audioDiaryPlayer.GetComponent<AudioSource>().isPlaying){

			if(currentAudioDiary){
				currentAudioDiary.GetComponent<FPEInteractableAudioDiaryScript>().stopAudioDiary();
			}

			hideAudioDiaryTitle();

		}
		
		// If we skipped it, and are currently still fading out the audio volume
		if(fadingDiaryAudio){

			fadeCounter += Time.deltaTime;

			if(fadeCounter >= 0.1f){

				audioDiaryPlayer.GetComponent<AudioSource>().volume -= fadeAmountPerTenthSecond;

				if(audioDiaryPlayer.GetComponent<AudioSource>().volume <= 0.0f){
					audioDiaryPlayer.GetComponent<AudioSource>().Stop();
					fadingDiaryAudio = false;
				}

				fadeCounter = 0.0f;

			}

		}

		// Mouse sensitivity transition smoothing. When slowMouseOnInteractableObjectHighlight is true, we want the mouse sensitivity to somewhat smoothly restore
		// to "full" sensitivity, but not be jarring or "pop" as soon as an object is no longer highlighted by the reticle.
		// Note: Replace this as required if using a different FPS Character Controller like UFPS, etc. that has other integrated mouse sensitivity management and aim assist.
		if(smoothMouseChange){
			thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().XSensitivity = Mathf.Lerp(thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().XSensitivity, targetMouseSensitivity.x, smoothMouseChangeRate);
			thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().YSensitivity = Mathf.Lerp(thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().YSensitivity, targetMouseSensitivity.y, smoothMouseChangeRate);
		}

	}

	private void activateReticle(string interactionString){
		if(reticleEnabled){
			reticle.GetComponent<Image>().overrideSprite = activeReticle;
		}
		if(interactionTextEnabled){
			interactionLabel.GetComponent<Text>().text = interactionString;
			interactionLabelTargetScale = interactionLabelLargestScale;
		}
	}

	private void deactivateReticle(){
		if(reticleEnabled){
			reticle.GetComponent<Image>().overrideSprite = inactiveReticle;
		}
		if(interactionTextEnabled){
			interactionLabel.GetComponent<Text>().text = "";
			interactionLabelTargetScale = interactionLabelSmallestScale;
		}
	}

	private void hideReticleAndInteractionLabel(){
		if(reticleEnabled){
			reticle.GetComponentInChildren<Image>().enabled = false;
		}
		if(interactionTextEnabled){
			interactionLabel.GetComponentInChildren<Text>().text = "";
		}
	}

	private void showReticleAndInteractionLabel(){
		if(reticleEnabled){
			reticle.GetComponentInChildren<Image>().enabled = true;
		}
		if(interactionTextEnabled){
			interactionLabel.GetComponentInChildren<Text>().text = "";
		}
	}

	public void playNewAudioDiary(GameObject diary){

		// if currently playing an audio diary, stop current one, reset text for new one
		if(playingAudioDiary){
			currentAudioDiary.GetComponent<FPEInteractableAudioDiaryScript>().stopAudioDiary();
			audioDiaryLabel.GetComponent<Text>().text = "";
			audioDiaryLabel.GetComponent<Text>().color = defaultDiaryColor;
		}

		currentAudioDiary = diary.gameObject;
		audioDiaryLabel.GetComponent<Text>().color = defaultDiaryColor;
		audioDiaryLabel.GetComponent<Text>().text = "Playing '"+diary.GetComponent<FPEInteractableAudioDiaryScript>().audioDiaryTitle+"' - Press 'X' to Skip";
		playingAudioDiary = true;
		audioDiaryLabelTargetScale = audioDiaryLabelLargestScale;
		fadingDiaryAudio = false;
		fadingDiaryText = false;
		audioDiaryPlayer.GetComponent<AudioSource>().clip = diary.GetComponent<FPEInteractableAudioDiaryScript>().audioDiaryClip;
		audioDiaryPlayer.GetComponent<AudioSource>().volume = 1.0f;
		audioDiaryPlayer.GetComponent<AudioSource>().Play();

	}

	public void hideAudioDiaryTitle(){

		fadingDiaryText = true;
		currentAudioDiary = null;
		playingAudioDiary = false;

	}

	public void openJournal(){

		disableMovement();
		disableMouseLook();
		setCursorVisibility(true);

		journalSFXPlayer.GetComponent<AudioSource>().clip = journalOpen;
		journalSFXPlayer.GetComponent<AudioSource>().Play();

		journalCloseButton.transform.gameObject.GetComponentInChildren<Image>().enabled = true;
		journalPreviousButton.transform.gameObject.GetComponentInChildren<Image>().enabled = true;
		journalNextButton.transform.gameObject.GetComponentInChildren<Image>().enabled = true;
		journalBackground.transform.gameObject.GetComponentInChildren<Image>().enabled = true;
		journalPage.transform.gameObject.GetComponentInChildren<Image>().enabled = true;

		currentJournalPages = currentJournal.GetComponent<FPEInteractableJournalScript>().journalPages;
		if(currentJournalPages.Length > 0){
			journalPage.transform.gameObject.GetComponentInChildren<Image>().overrideSprite = currentJournalPages[currentJournalPageIndex];
		}else{
			Debug.LogError("Journal '" + currentJournal.name + "' opened, but was assigned no pages. Assign Sprites to journalPages array in the Inspector.");
		}
	
	}

	public void nextJournalPage(){

		currentJournalPageIndex++;
		if(currentJournalPageIndex > currentJournalPages.Length - 1){
			currentJournalPageIndex = currentJournalPages.Length - 1;
		}else{
			journalSFXPlayer.GetComponent<AudioSource>().clip = journalPageTurn;
			journalSFXPlayer.GetComponent<AudioSource>().Play();
		}

		journalPage.transform.gameObject.GetComponentInChildren<Image>().overrideSprite = currentJournalPages[currentJournalPageIndex];

	}

	public void previousJournalPage(){

		currentJournalPageIndex--;
		if(currentJournalPageIndex < 0){
			currentJournalPageIndex = 0;
		}else{
			journalSFXPlayer.GetComponent<AudioSource>().clip = journalPageTurn;
			journalSFXPlayer.GetComponent<AudioSource>().Play();
		}

		journalPage.transform.gameObject.GetComponentInChildren<Image>().overrideSprite = currentJournalPages[currentJournalPageIndex];

	}

	public void closeJournal(bool playSound=true){

		if(playSound){
			journalSFXPlayer.GetComponent<AudioSource>().clip = journalClose;
			journalSFXPlayer.GetComponent<AudioSource>().Play();
		}

		journalCloseButton.transform.gameObject.GetComponentInChildren<Image>().enabled = false;
		journalPreviousButton.transform.gameObject.GetComponentInChildren<Image>().enabled = false;
		journalNextButton.transform.gameObject.GetComponentInChildren<Image>().enabled = false;
		journalBackground.transform.gameObject.GetComponentInChildren<Image>().enabled = false;
		journalPage.transform.gameObject.GetComponentInChildren<Image>().enabled = false;

		if(currentJournal){
			currentJournal.GetComponent<FPEInteractableJournalScript>().deactivateJournal();
		}

		currentJournal = null;
		currentJournalPageIndex = 0;
		currentJournalPages = null;

		setCursorVisibility(false);
		enableMouseLook();
		enableMovement();

	}

	// Sets mouse hints for LMB and RMB. If empty string passed in,
	// text and icon are set to invisible for associate LMB/RMB hint.
	private void setMouseHints(string LMBHintText, string RMBHintText){

		if(LMBHintText == ""){
			mouseLMBHelperIcon.GetComponent<Image>().enabled = false;
			mouseLMBHelperText.GetComponent<Text>().text = "";
			mouseLMBHelperText.GetComponent<Text>().enabled = false;
		}else{
			mouseLMBHelperIcon.GetComponent<Image>().enabled = true;
			mouseLMBHelperText.GetComponent<Text>().text = LMBHintText;
			mouseLMBHelperText.GetComponent<Text>().enabled = true;
		}

		if(RMBHintText == ""){
			mouseRMBHelperIcon.GetComponent<Image>().enabled = false;
			mouseRMBHelperText.GetComponent<Text>().text = "";
			mouseRMBHelperText.GetComponent<Text>().enabled = false;
		}else{
			mouseRMBHelperIcon.GetComponent<Image>().enabled = true;
			mouseRMBHelperText.GetComponent<Text>().text = RMBHintText;
			mouseRMBHelperText.GetComponent<Text>().enabled = true;
		}

	}

	// Note: If you are using this package for Unity 4.x, and wish to upgrade your project
	// to Unity 5.x later, you must change "Screen.showCursor" to "Cursor.visible", as 
	// "Screen.showCursor" was deprecated in Unity 5.
	private void setCursorVisibility(bool visible){
		Cursor.visible = visible;
		//Screen.showCursor = visible;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Customize the body of these functions as required for your Character Controller code of choice. If using something //
	// like UFPS, you may want to overhaul these completely. If you need help, please email support@whilefun.com          //
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	// This function records our starting sensitivity.
	// The Standard Asset version of the MouseLook script uses X on Character Controller and Y on Camera.
	private void rememberStartingMouseSensitivity(){
		startingMouseSensitivity.x = thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().XSensitivity;
		startingMouseSensitivity.y = thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().YSensitivity;
	}
	// Set sensitivity directly, and ensure smoothMouseChange is off.
	private void setMouseSensitivity(Vector2 sensitivity){
		thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().XSensitivity = sensitivity.x;
		thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().YSensitivity = sensitivity.y;
		smoothMouseChange = false;
	}
	// Restores mouse sensitivity to starting Mouse sensitivity
	// Vector2 is desired sensitivity. If smoothTransition is true, sensitivity 
	// change is gradual. Otherwise, it is changed immediately.
	private void restorePreviousMouseSensitivity(bool smoothTransition){
		if(smoothTransition){
			targetMouseSensitivity.x = startingMouseSensitivity.x;
			targetMouseSensitivity.y = startingMouseSensitivity.y;
			smoothMouseChange = true;
		}else{
			thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().XSensitivity = startingMouseSensitivity.x;
			thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().YSensitivity = startingMouseSensitivity.y;
			smoothMouseChange = false;
		}
	}
	// Locks mouse look, so we can move mouse to rotate objects when examining them.
	// If using another Character Controller (UFPS, etc.) substitute mouselook disable functionality
	private void disableMouseLook(){
		thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().enableMouseLook = false;
		mouseLookEnabled = false;
	}
	// Unlocks mouse look so we can move mouse to look when walking/moving normally.
	// If using another Character Controller (UFPS, etc.) substitute mouselook enable functionality
	private void enableMouseLook(){
		thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.MouseLook>().enableMouseLook = true;
		mouseLookEnabled = true;
	}
	// Locks movement of Character Controller. 
	// If using another Character Controller (UFPS, etc.) substitute disable functionality
	private void disableMovement(){
		thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enableMovement = false;
	}
	// Unlocks movement of Character Controller. 
	// If using another Character Controller (UFPS, etc.) substitute enable functionality
	private void enableMovement(){
		thePlayer.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enableMovement = true;
	}

	public bool isMouseLookEnabled(){
		return mouseLookEnabled;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
