using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TowerPopup : EventAction
{
	public int startingHitpoint = 100;
	public int currentHitpoint;
	public Slider hitpointSlider;

	bool isDestroyed;

	void Awake ()
	{
		// Set the initial health of the player.
		currentHitpoint = startingHitpoint;
	}
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public override void PointerDown (Vector2 position)
	{
	}

	public override void PointerUp (Vector2 position)
	{
	}

	public override void PointerDragging (Vector2 position, Vector2 delta)
	{
	}

	public override void PointerClick ()
	{
	}


	public void TakeDamage (int amount)
	{
		// Reduce the current health by the damage amount.
		currentHitpoint -= amount;

		// Set the health bar's value to the current health.
		hitpointSlider.value = currentHitpoint;

		if (currentHitpoint <= 0 && !isDestroyed) {
			Death ();
		}
	}


	void Death ()
	{
		// Set the death flag so this function won't be called again.
		isDestroyed = true;
	}
}