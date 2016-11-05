using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		new ClientManager ().SendToServer ("Salam").OnSuccess ((data)=>{
			Debug.Log(data.value);
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
