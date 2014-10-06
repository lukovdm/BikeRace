using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Networkmanger : MonoBehaviour {
	public string uniqueGameName = "Betabit.Network.test";
	public string gameName;
	public InputField hostInput;
	public InputField joinInput;
	public GameObject buttons;
	public GameObject bikePrefab;
	public GameObject groundPrefab;
	public GameObject camera;
	public GameObject quit;
	public Text status;

	private bool refreshing = false;
	private HostData[] hostData;
	private GameObject player;

	public void Update() {
		if (refreshing) {
			if (MasterServer.PollHostList ().Length > 0) {
				refreshing = false;
				hostData = MasterServer.PollHostList ();
				foreach (HostData hD in hostData) {
					if(hD.gameName == joinInput.value){
						Network.Connect(hD);
						Time.timeScale = 1f;
						break;
					}
				}
				status.text = "no server found";
			}
		}

		if (!(Network.isServer || Network.isClient)) {
			buttons.SetActive (true);
			quit.SetActive(false);
		} else {
			buttons.SetActive (false);
			quit.SetActive(true);
		}
	}

	public void Start(){
		Time.timeScale = 0f;
	}

	public void startServer(){
		status.text = "Starting server";
		Network.InitializeServer (32, 25001, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (uniqueGameName, hostInput.value, "luko test");
		Time.timeScale = 1f;
	}

	public void joinServer(){
		MasterServer.RequestHostList (uniqueGameName);
		refreshing = true;
	}

	public void disconnect(){
		if (Network.isServer) {
			Network.Disconnect ();
			MasterServer.UnregisterHost();
		} else {
			foreach(NetworkPlayer connection in Network.connections){
				status.text = "Disconnecting: " + connection.ipAddress + ":" + connection.port;
				Network.CloseConnection(connection, true);
			}
		}
		camera.GetComponent<FlollowPlayer> ().enabled = false;
	}

	public void spawnPlayer(GameObject playerPrefab, bool bike){
		player = (GameObject) Network.Instantiate (playerPrefab, Vector3.zero, Quaternion.identity, 0);
		camera.GetComponent<FlollowPlayer> ().enabled = true;
		if (bike) {
			camera.GetComponent<FlollowPlayer> ().playerTrans = player.transform;
		} else {
			camera.GetComponent<FlollowPlayer> ().playerTrans = player.transform.FindChild ("GPlayer").transform;
		}
	}

	//Messages
	public void OnServerInitialized(){
		status.text = "Server initialised";
		spawnPlayer (groundPrefab, false);
	}

	public void OnConnectedToServer(){
		spawnPlayer (bikePrefab, true);
	}

	public void OnMasterServerEvent(MasterServerEvent mse) {
		status.text = mse.ToString ();
	}

	public void OnDisconnectedFromServer(NetworkDisconnection info){
		foreach(GameObject disconnectedPlayer in GameObject.FindGameObjectsWithTag("Player"))
			Network.Destroy(disconnectedPlayer);

		if (Network.isServer) {
			status.text = "Local server connection disconnected";
		} else {
			if (info == NetworkDisconnection.LostConnection)
				status.text = "Lost connection to the server";
			else {
				status.text = "Successfully diconnected from the server";
			}
		}
		buttons.SetActive (true);
		Destroy(player);
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
}
