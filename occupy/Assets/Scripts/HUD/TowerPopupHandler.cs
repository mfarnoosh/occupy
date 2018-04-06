using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TowerPopupHandler : MonoBehaviour {
	public Image HitpointProgressImage;
	private Tower _selectedTower = null;
	public Tower SelectedTower{ get { return _selectedTower; } }

	public void Start(){
		//TowerDeselect ();
	}

	public void Update(){
		if (HitpointProgressImage != null && SelectedTower != null)
			HitpointProgressImage.fillAmount = SelectedTower.currentHitPoint / SelectedTower.Config.HitPoint;
	}
	public void TowerSelect(Tower tower){
		if (tower == null) {
			TowerDeselect ();
			return;
		}
		if (_selectedTower != null && tower != null && _selectedTower.id.Equals (tower.id))
			return;
		_selectedTower = tower;

		var pos = SelectedTower.gameObject.transform.position;
		gameObject.transform.position = new Vector3(pos.x + 11 ,pos.y + 35 ,pos.z);
		//gameObject.transform.localEulerAngles = new Vector3(-130f,0f,-180f);
		gameObject.transform.localScale = new Vector3 (0.2f,0.2f,0.2f);
		//for sticking to the tower position in map movement
		gameObject.transform.SetParent (tower.gameObject.transform,true);

		gameObject.SetActive (true);
	}

	public void TowerDeselect(){
		_selectedTower = null;
		gameObject.transform.SetParent (null);
		gameObject.SetActive (false);
	}

	public void RepairNow(){
		if (SelectedTower == null)
			return;

		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "repairTower";
		sm.Params.Add (SelectedTower.id);

		NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
			if(string.IsNullOrEmpty(data.value.ExceptionMessage))
				Debug.Log("tower repaired.");
			else
				Debug.Log(data.value.ExceptionMessage);
		});
	}
	public void Sell(){
		if (SelectedTower == null)
			return;
		Debug.Log ("we are here, tower id: " + SelectedTower.id);
	}
	public void Upgrade(){
		if (SelectedTower == null)
			return;
		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "upgradeTower";
		sm.Params.Add (SelectedTower.id);

		NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
			if(string.IsNullOrEmpty(data.value.ExceptionMessage))
				Debug.Log("tower upgraded.");
			else
				Debug.Log(data.value.ExceptionMessage);
		});
	}

	public void CreateUnit(GameObject unitPrefab){
		if (SelectedTower == null)
			return;
		if (unitPrefab == null) {
			Debug.LogError ("unit prefab is null for creating unit in tower");
			return;
		}

		Unit unit = unitPrefab.GetComponent<Unit> ();
		if (unit == null) {
			Debug.LogError ("given unit prefab has no Unit script attached to creating new unit in tower");
			return;
		}
		SocketMessage sm = new SocketMessage ();
		sm.Cmd = "createUnit";
		sm.Params.Add (SelectedTower.id);
		sm.Params.Add (unit.type.ToString());

		NetworkManager.Current.SendToServer (sm).OnSuccess ((data) => {
			if(string.IsNullOrEmpty(data.value.ExceptionMessage))
				Debug.Log("unit created.");
			else
				Debug.Log(data.value.ExceptionMessage);
		});
	}
}
