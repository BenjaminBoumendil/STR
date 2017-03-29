using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PlayerController : AGameObject {

    [SerializeField]
    [SyncVar]
    private string playerName;
    public string PlayerName {
        get
        {
            return playerName;
        }
        set
        {
            playerName = value;
        }
    }

    private GameObject selector;
    private GameObject hexGrid;
    private GameObject userOverlay;

    // set authority for objectId
    [Command]
    private void CmdSetAuthority(NetworkInstanceId objectId)
    {
        NetworkServer.FindLocalObject(objectId).GetComponent<AGameObject>().CmdSetOwner(netId);
    }

    [Client]
    private void ClickEvent(Vector3 position)
    {
        HexCell cell = hexGrid.GetComponent<HexGrid>().GetCell(position);
        if (!cell.hasAuthority)
        {
            CmdSetAuthority(cell.netId);
        }
    }

    // Select a game object and render UI
    [Client]
    private void Select(RaycastHit hit)
    {
        if (hit.transform.gameObject.tag == "MapCollider")
        {
            selector = hexGrid.GetComponent<HexGrid>().GetCell(hit.point).gameObject != selector ? hexGrid.GetComponent<HexGrid>().GetCell(hit.point).gameObject : null;
        }
        else
        {
            selector = hit.transform.gameObject != selector ? hit.transform.gameObject : null;
        }
        
        if (selector != null)
        {
            userOverlay.GetComponent<UserOverlay>().RenderSelection(selector, delegate { ClickEvent(hit.point); });
        }
        else
        {
            userOverlay.GetComponent<UserOverlay>().ClearSelection();
        }
    }

    // Handle left click
    [Client]
    private void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            Select(hit);
        }
    }

    public override void OnStartClient()
    {
        hexGrid = GameObject.FindGameObjectWithTag("Map");
        userOverlay = GameObject.FindGameObjectWithTag("UserOverlay");
    }

    [Client]
    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                HandleInput();
            }
        }
    }
}
