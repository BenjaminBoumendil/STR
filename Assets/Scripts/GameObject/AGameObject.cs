using UnityEngine;
using UnityEngine.Networking;

public abstract class AGameObject : NetworkBehaviour {

    [SyncVar]
    private int owner;
    public int Owner { get; set; }
    
    [SyncVar]
    private Color color;
    public Color Color { get; set; }

    // Set new owner for this game object and change color
    [Command]
    public void CmdSetOwner(NetworkInstanceId netId)
    {
        if (owner == 0)
        {
            GameObject player = NetworkServer.FindLocalObject(netId);
            GetComponent<NetworkIdentity>().AssignClientAuthority(player.GetComponent<NetworkIdentity>().connectionToClient);
            this.owner = player.GetComponent<PlayerController>().owner;
            this.color = player.GetComponent<PlayerController>().color;
        }
    }

    // Init game object
    public void Init(int owner, Color color)
    {
        this.owner = owner;
        this.color = color;
    }
}
