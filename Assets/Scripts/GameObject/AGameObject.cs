using UnityEngine;
using UnityEngine.Networking;

public abstract class AGameObject : NetworkBehaviour {

    [SerializeField]
    [SyncVar]
    private int owner;
    public int Owner {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }
    
    [SerializeField]
    [SyncVar]
    private Color color;
    public Color Color {
        get
        {
            return color;
        }
        set
        {
            color = value;
        }
    }

    // Set new owner for this game object and change color
    [Command]
    public void CmdSetOwner(NetworkInstanceId netId)
    {
        if (Owner == 0)
        {
            GameObject player = NetworkServer.FindLocalObject(netId);
            GetComponent<NetworkIdentity>().AssignClientAuthority(player.GetComponent<NetworkIdentity>().connectionToClient);
            Owner = player.GetComponent<PlayerController>().Owner;
            Color = player.GetComponent<PlayerController>().Color;
        }
    }
}
