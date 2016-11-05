using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Test : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		NetworkManager.Current.SendToServer ("Salam").OnSuccess ((data) => {
			Debug.Log (data.value);

			var go = GetComponent<Text> ();

			if (go != null) {
				go.text = data.value;
			}
		});
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
