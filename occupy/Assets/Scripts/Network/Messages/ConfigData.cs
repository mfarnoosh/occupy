using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ConfigData : BaseObjectData{
	public string version;
	public MapConfigData mapConfig;
	public List<TowerConfigData> towers = new List<TowerConfigData>();
	public List<UnitConfigData> units = new List<UnitConfigData>();
}
