using UnityEngine;
using System.Collections;
using UnityToolbag;
using System.Net.Sockets;
using System.Text;
using System.IO;

public class NetworkManager : MonoBehaviour{
	public string ServerAddress = "192.168.1.5";
	public int ServerPort = 4444;

	public static NetworkManager Current = null; 

	public NetworkManager(){
		Current = this;
	}

	public Future<string> SendToServer (string command) {
		Debug.Log ("we are here");
		Future<string> future = new Future<string> ();
		future.Process (() => {
			TcpClient client = new TcpClient (ServerAddress, ServerPort);
			byte[] bytes = Encoding.UTF8.GetBytes (command);
			byte[] finished = Encoding.UTF8.GetBytes ("__FIN__");
			client.GetStream ().Write (bytes, 0, bytes.Length);
			client.GetStream ().Write (finished, 0, finished.Length);
			StreamReader sr = new StreamReader(client.GetStream());
			string data = sr.ReadToEnd();
			return data;
		});
		return future;	
	}

}
