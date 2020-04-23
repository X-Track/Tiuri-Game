using UnityEngine;
using System.Collections;

public class DemoSlidingDoorScript : MonoBehaviour {

	private GameObject thePlayer;
	private GameObject doorA;
	private GameObject doorB;
	private GameObject walkBlocker;

	private Vector3 doorAOpenPosition;
	private Vector3 doorAClosedPosition;
	private Vector3 doorBOpenPosition;
	private Vector3 doorBClosedPosition;

	private float doorMovementSpeed = 3.0f;
	private float doorAutoCloseTime = 3.0f;
	private float doorAutoCloseCountdown = 0.0f;
	private float doorAutoCloseZoneRadius = 2.0f;

	private bool doAutoOpenClose = false;
	private bool doorLocked = true;

	private enum eDoorState { CLOSED, CLOSING, OPENING, OPEN };
	private eDoorState currentDoorState = eDoorState.CLOSED;

	void Awake(){
	
		thePlayer = GameObject.FindGameObjectWithTag("Player");

		doorA = transform.Find("doubleSlidingDoor/DoorA").gameObject;
		doorB = transform.Find("doubleSlidingDoor/DoorB").gameObject;
		walkBlocker = transform.Find("WalkBlocker").gameObject;

		doorAClosedPosition = doorA.transform.position;
		doorAOpenPosition = doorA.transform.position;
		doorAOpenPosition.z -= 1.2f;

		doorBClosedPosition = doorB.transform.position;
		doorBOpenPosition = doorB.transform.position;
		doorBOpenPosition.z += 1.2f;

	}
	
	void Start(){
	
	}
	
	void Update(){

		// If the player is within the automated movement zone, always move to OPENING state
		if(Vector3.Distance(transform.position, thePlayer.transform.position) < doorAutoCloseZoneRadius){

			doAutoOpenClose = false;

			if(currentDoorState == eDoorState.CLOSED || currentDoorState == eDoorState.CLOSING){

				if(!doorLocked){
					gameObject.GetComponent<AudioSource>().Play();
					currentDoorState = eDoorState.OPENING;
				}

			}

		}else{

			doAutoOpenClose = true;

		}

		// State Management //
		if(currentDoorState == eDoorState.OPENING){

			doorA.transform.position = Vector3.Lerp(doorA.transform.position, doorAOpenPosition, doorMovementSpeed*Time.deltaTime);
			doorB.transform.position = Vector3.Lerp(doorB.transform.position, doorBOpenPosition, doorMovementSpeed*Time.deltaTime);

			if(Vector3.Distance(doorA.transform.position, doorAOpenPosition) < 0.65f){
				// we want to disable collider sooner than doors being all the way open
				walkBlocker.GetComponent<BoxCollider>().enabled = false;
			}

			if(Vector3.Distance(doorA.transform.position, doorAOpenPosition) < 0.2f){
				
				doorA.transform.position = doorAOpenPosition;
				doorB.transform.position = doorBOpenPosition;
				currentDoorState = eDoorState.OPEN;
				doorAutoCloseCountdown = doorAutoCloseTime;
				
			}

		}else if(currentDoorState == eDoorState.OPEN){

			// Only auto-close if it is safe to do so
			if(doAutoOpenClose){

				doorAutoCloseCountdown -= Time.deltaTime;

				if(doorAutoCloseCountdown <= 0.0f){

					currentDoorState = eDoorState.CLOSING;
					walkBlocker.GetComponent<BoxCollider>().enabled = true;
					gameObject.GetComponent<AudioSource>().Play();

				}

			}

		}else if(currentDoorState == eDoorState.CLOSING){

			doorA.transform.position = Vector3.Lerp(doorA.transform.position, doorAClosedPosition, doorMovementSpeed*Time.deltaTime);
			doorB.transform.position = Vector3.Lerp(doorB.transform.position, doorBClosedPosition, doorMovementSpeed*Time.deltaTime);
			
			if(Vector3.Distance(doorA.transform.position, doorAClosedPosition) < 0.2f){
				
				doorA.transform.position = doorAClosedPosition;
				doorB.transform.position = doorBClosedPosition;
				currentDoorState = eDoorState.CLOSED;

			}

		}

	}

	public bool isDoorLocked(){
		return doorLocked;
	}

	public void unlockTheDoor(){
		doorLocked = false;
	}

}
