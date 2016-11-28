using UnityEngine;
using UnityEngine.Events;
using Lean.Touch;
using System.Collections;

public class DragHandler : MonoBehaviour {
	public GameObject Prefab;
	public GameObject Ghost;

	void Start(){
		TouchManager.Current.enabled = false;
	}
	void LateUpdate(){
		if (LeanTouch.Fingers == null || LeanTouch.Fingers.Count != 1)
			return;

		var screenPosition = LeanTouch.Fingers [0].ScreenPosition;

		var tempTarget = PlayerManager.Current.ScreenPointToMapPosition (screenPosition);
		if (tempTarget.HasValue == false)
			return;

		transform.position = tempTarget.Value;
	}

	public void Finish(Vector2 screenPosition){	
		var go = GameObject.Instantiate (Prefab);


		//Send Position to server
		Tile tile = MapManager.Current.GetTile(transform.position);

		Debug.Log(tile);
		Location loc = GeoUtils.XYZToLocation(tile,transform.position);

		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "saveBuilding";
		sm.Params.Add (loc.Latitude.ToString());
		sm.Params.Add (loc.Longitude.ToString());
		NetworkManager.Current.SendToServer (sm).OnSuccess((data)=>{
			string lat = data.value.Params[0];
			string lon = data.value.Params[1];
			Debug.Log("Building Moved: " + lat + "," + lon);
			Location sLoc = new Location(float.Parse(lat),float.Parse(lon));

			CreateBuilding(tile,sLoc,Color.green);
		});

		//End Sending position to server


		go.transform.position = transform.position;
		go.transform.parent = tile.transform;
		Destroy (this.gameObject);
	}
		
	void OnDestroy(){
		TouchManager.Current.enabled = true;
	}
	//TODO: Farnoosh remove these lines. just for test getting data from server
	private void CreateBuilding(Tile tile,Location loc,Color col){
		var go = GameObject.Instantiate(Prefab);
		go.transform.localScale = new Vector3(5,5,5);
		go.GetComponent<Renderer> ().material.color = col;
		//TODO: Convert Latitude,Longitude to X,Y
		var pos = GeoUtils.LocationToXYZ(tile,loc);
		Debug.Log(pos.x + " | " + pos.z);
		go.transform.position = pos;
		go.transform.parent = tile.transform;
	}
	//TODO

}
