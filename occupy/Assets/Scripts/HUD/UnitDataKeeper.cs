using UnityEngine;
using System.Collections;

public class UnitDataKeeper : MonoBehaviour {
	private UnitData _unitData;
	public UnitData CurrentUnitData{ 
		get { 
			return _unitData; 
		} 
		set { 
			_unitData = value; 
			if (_unitData != null)
				_unitConfigData = UnitManager.Current.GetUnitConfig (_unitData.Type, _unitData.Level);
			else
				_unitConfigData = null;
		} 
	}

	private UnitConfigData _unitConfigData;
	public UnitConfigData CurrentUnitConfigData { get { return _unitConfigData; } }
}
