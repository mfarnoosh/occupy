using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Scripting;
using System;

[Serializable]
public class SocketMessage {
	public string Id = System.Guid.NewGuid().ToString();
	public string PlayerKey = "";
	public string Cmd = "";
	public List<string> Params = new List<string>();
	public string ExceptionMessage="";

}
	