using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Progressbar : MonoBehaviour {
	public Image ProgressbarImage;

	private UnitDataKeeper unitDataKeeper;
	public void Start(){
		unitDataKeeper = GetComponent<UnitDataKeeper> ();
	}
	public void Update(){
		if (unitDataKeeper != null && ProgressbarImage != null) {
			ProgressbarImage.fillAmount =  ((float)(unitDataKeeper.CurrentUnitData.CurrentHitPoint / unitDataKeeper.CurrentUnitConfigData.HitPoint));
		}
	}
}
