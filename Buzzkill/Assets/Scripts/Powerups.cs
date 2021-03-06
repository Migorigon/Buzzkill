﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Powerups : MonoBehaviour {

	public enum PowerupList {shield, extraLife, powerup2, doublePoints}
	public PowerupList powerup;
	Transform tf;
	public float speed;

	// Use this for initialization
	void Start () {
		tf = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.isPaused) {
			return;
		}
		tf.position += tf.right * Time.deltaTime * speed * GameManager.instance.worldSpeed;
	}

	void OnTriggerEnter2D(Collider2D other){
		BeeMovement player = other.GetComponent<BeeMovement> ();
		BeeAttack_Melee Battack = other.GetComponent<BeeAttack_Melee>();

		if (player) {
			switch(powerup){
			case PowerupList.shield:
				player.hasShield = true;
				player.shieldSprite.SetActive (true);
				Destroy (gameObject);
				break;
			
			case PowerupList.doublePoints:
				Battack.Dtimer = 10f;
				Battack.hasCoin = true;
				Destroy(gameObject);
				break;

			case PowerupList.extraLife:
				GameManager.instance.pLives += 1;
				PurchaseItemRequest request = new PurchaseItemRequest ();
				request.ItemId = "ExtraLife";
				request.Price = 0;
				request.VirtualCurrency = "PU";
				PlayFabClientAPI.PurchaseItem (request, GrabbedExtraLife, OnPlayFabError); 
				Destroy (gameObject);
				break;
			}
		}
	}
	private void GrabbedExtraLife(PurchaseItemResult result){
		Debug.Log ("Extra life added to inventory");
		Debug.Log ("You have " + result.Items [0].RemainingUses + " extra lives remaining");
	}

	private void OnPlayFabError(PlayFabError error){
		Debug.LogError(error.GenerateErrorReport ());
	}
}
