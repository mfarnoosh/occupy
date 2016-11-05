using UnityEngine;
using System.Collections;
using UnityToolbag;
using System.Net.Sockets;
using System.Text;
using System.IO;
public class ClientManager {
	private string Ip = "192.168.1.10";
	private int Port = 4444;

	public Future<string> SendToServer (string command) {
		Future<string> future = new Future<string> ();
		future.Process (() => {
			TcpClient client = new TcpClient (Ip, Port);
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
