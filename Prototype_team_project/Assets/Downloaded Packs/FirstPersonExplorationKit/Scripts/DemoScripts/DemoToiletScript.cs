using UnityEngine;
using System.Collections;

//
// DemoToiletScript
// This script manages the core toilet state and animations.
//
// Copyright 2016 While Fun Games
// http://whilefun.com
//
public class DemoToiletScript : MonoBehaviour {

	private bool seatUp = false;
	private bool canMoveSeat = true;
	private float seatCooldown = 0.5f;
	private float seatCountdown = 0.0f;
	private bool canFlush = true;
	private float flushCooldown = 1.5f;
	private float reflushCountdown = 0.0f;
	
	void Update(){

		if(!canFlush){
			reflushCountdown -= Time.deltaTime;
			if(reflushCountdown <= 0.0f){
				canFlush = true;
			}
		}

		if(!canMoveSeat){
			seatCountdown -= Time.deltaTime;
			if(seatCountdown <= 0.0f){
				canMoveSeat = true;
			}
		}

	}

	public bool flushToilet(){

		bool flushResult = false;

		if(canFlush){
			gameObject.GetComponent<Animator>().SetTrigger("PressToiletHandle");
			canFlush = false;
			reflushCountdown = flushCooldown;
			flushResult = true;
		}

		return flushResult;

	}

	public bool openCloseToiletSeat(){

		bool seatResult = false;

		if(canMoveSeat){

			if(seatUp){
				gameObject.GetComponent<Animator>().SetTrigger("PutSeatDown");
			}else{
				gameObject.GetComponent<Animator>().SetTrigger("PutSeatUp");
			}

			seatCountdown = seatCooldown;
			canMoveSeat = false;
			seatResult = true;

		}

		return seatResult;

	}

	public void setSeatState(int state){

		if(state == 1){
			seatUp = true;
		}else{
			seatUp = false;
			gameObject.GetComponent<Animator>().SetBool("MoveSeatDown",false);
		}

	}

	public bool isSeatUp(){
		return seatUp;
	}

}
