using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Prototype.NetworkLobby
{
    // Subclass this and redefine the function you want
    // then add it to the lobby prefab
    public class LobbyHook : MonoBehaviour
    {
        private static int playerNb = 1;

        public virtual void OnLobbyServerSceneChanged(NetworkManager manager, string sceneName)
        {

        }

        public virtual void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
        {
            LobbyPlayer lobbyInfo = lobbyPlayer.GetComponent<LobbyPlayer>();
            PlayerController gameInfo = gamePlayer.GetComponent<PlayerController>();

            gameInfo.Color = lobbyInfo.playerColor;
            gameInfo.PlayerName = lobbyInfo.playerName;
            gameInfo.Owner = playerNb++;
        }
    }
}
