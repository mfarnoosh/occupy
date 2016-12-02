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
		go.transform.localScale = new Vector3(
			go.transform.localScale.x * PlayerManager.Current.ObjectScaleMultiplier.x,
			go.transform.localScale.y * PlayerManager.Current.ObjectScaleMultiplier.y,
			go.transform.localScale.z * PlayerManager.Current.ObjectScaleMultiplier.z);

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


			string lat2 = data.value.Params[2];
			string lon2 = data.value.Params[3];
			Debug.Log("Building Moved: " + lat2 + "," + lon2);
			Location sLoc2 = new Location(float.Parse(lat2),float.Parse(lon2));

			CreateBuilding(tile,sLoc2,Color.red);

			string lat3 = data.value.Params[4];
			string lon3 = data.value.Params[5];
			Debug.Log("Building Moved: " + lat3 + "," + lon3);
			Location sLoc3 = new Location(float.Parse(lat3),float.Parse(lon3));

			CreateBuilding(tile,sLoc3,Color.gray);


			string lat4 = data.value.Params[6];
			string lon4 = data.value.Params[7];
			Debug.Log("Building Moved: " + lat4 + "," + lon4);
			Location sLoc4 = new Location(float.Parse(lat4),float.Parse(lon4));

			CreateBuilding(tile,sLoc4,Color.yellow);

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
		go.transform.localScale = new Vector3(
			go.transform.localScale.x * PlayerManager.Current.ObjectScaleMultiplier.x,
			go.transform.localScale.y * PlayerManager.Current.ObjectScaleMultiplier.y,
			go.transform.localScale.z * PlayerManager.Current.ObjectScaleMultiplier.z);
		
		go.GetComponent<Renderer> ().material.color = col;
		//TODO: Convert Latitude,Longitude to X,Y
		var pos = GeoUtils.LocationToXYZ(tile,loc);
		Debug.Log(pos.x + " | " + pos.z);
		go.transform.position = pos;
		go.transform.parent = tile.transform;

	}
	//TODO

}
