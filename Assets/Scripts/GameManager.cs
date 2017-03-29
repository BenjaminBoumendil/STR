using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class GameManager : NetworkBehaviour {

    private GameObject map;
    private GameObject[] playerList;
    private GameObject lobbyManager;
    private bool isInit = false;

    private void InitGame()
    {
        int[] startPos = { 0, (map.GetComponent<HexGrid>().width - 1), (map.GetComponent<HexGrid>().Cells.Length - map.GetComponent<HexGrid>().width), (map.GetComponent<HexGrid>().Cells.Length - 1) };
        int i = 0;

        foreach (GameObject player in playerList)
        {
            HexCell cell = map.GetComponent<HexGrid>().Cells[startPos[i++]];
            cell.CmdSetOwner(player.GetComponent<NetworkIdentity>().netId);
        }
    }

    public override void OnStartServer()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        playerList = GameObject.FindGameObjectsWithTag("Player");
        lobbyManager = GameObject.FindGameObjectWithTag("LobbyManager");
    }

    private void Update()
    {
        playerList = GameObject.FindGameObjectsWithTag("Player");
        if (isServer && !isInit && lobbyManager.GetComponent<LobbyManager>()._playerNumber == playerList.Length)
        {
            InitGame();
            isInit = true;
        }
    }
}
