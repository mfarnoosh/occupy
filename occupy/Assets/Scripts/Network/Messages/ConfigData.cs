using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ConfigData : BaseObjectData {
	public string version;
	public MapConfigData mapConfig;
	public List<TowerData> towers = new List<TowerData>();
	public List<UnitData> units = new List<UnitData>();
}
