using UnityEngine;
using System.Collections;
using UnityToolbag;
using System.Net.Sockets;
using System.Text;
using System.IO;

public class NetworkManager : MonoBehaviour{
	public string ServerIp = "192.168.1.5";
	public int ServerPort = 4444;

	public static NetworkManager Current;
	public NetworkManager(){
		Current = this;
	}

	public Future<SocketMessage> SendToServer (SocketMessage message) {
		Future<SocketMessage> future = new Future<SocketMessage> ();
		future.Process (() => {
			TcpClient client = new TcpClient (ServerIp, ServerPort);
			string json = JsonUtility.ToJson(message);
			byte[] bytes = Encoding.UTF8.GetBytes (json);
			byte[] finished = Encoding.UTF8.GetBytes ("__FIN__");
			client.GetStream ().Write (bytes, 0, bytes.Length);
			client.GetStream ().Write (finished, 0, finished.Length);
			client.GetStream().Flush();
			StreamReader sr = new StreamReader(client.GetStream());
			string data = sr.ReadToEnd();

			return JsonUtility.FromJson<SocketMessage>(data);;
		});
		return future;	
	}
}
