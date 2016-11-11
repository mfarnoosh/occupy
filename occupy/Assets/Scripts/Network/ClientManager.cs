using UnityEngine;
using System.Collections;
using UnityToolbag;
using System.Net.Sockets;
using System.Text;
using System.IO;

public class ClientManager {
	private static string Ip = "192.168.1.5";
	private static int Port = 4444;

	public Future<SocketMessage> SendToServer (SocketMessage message) {
		Future<SocketMessage> future = new Future<SocketMessage> ();
		future.Process (() => {
			TcpClient client = new TcpClient (Ip, Port);
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
