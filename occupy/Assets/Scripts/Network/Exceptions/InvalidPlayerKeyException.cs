using UnityEngine;
using System.Collections;
using System;

public class InvalidPlayerKeyException : BaseException {
	public InvalidPlayerKeyException(String playerKey):base(GetMessage(playerKey)) {
	}
	private static string GetMessage(string playerKey){
		return string.Format ("Invalid player key : {0}", playerKey);
	}
}
