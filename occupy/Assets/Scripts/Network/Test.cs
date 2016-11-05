using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Test : MonoBehaviour
{
	public Text InfoText;
	// Use this for initialization
	void Start ()
	{
		NetworkManager.Current.SendToServer ("Salam").OnSuccess ((data) => {
			Debug.Log (data.value);

			//var go = GetComponent<Text> ();

			if (InfoText != null) {
				InfoText.text = data.value;
			}
		});
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
