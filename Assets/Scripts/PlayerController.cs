using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PlayerController : AGameObject {

    [SyncVar]
    private string playerName;
    public string PlayerName { get; set; }

    private GameObject selector;
    private GameObject hexGrid;
    private GameObject userOverlay;

    // set authority for objectId
    [Command]
    private void CmdSetAuthority(NetworkInstanceId objectId)
    {
        NetworkServer.FindLocalObject(objectId).GetComponent<AGameObject>().CmdSetOwner(netId);
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
            userOverlay.GetComponent<UserOverlay>().RenderSelection(selector);
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
            HexCell cell = hexGrid.GetComponent<HexGrid>().GetCell(hit.point);
            if (!cell.hasAuthority)
            {
                CmdSetAuthority(cell.netId);
            }
        }
    }

    public override void OnStartClient()
    {
        hexGrid = GameObject.FindGameObjectWithTag("Map");
        userOverlay = GameObject.FindGameObjectWithTag("UserOverlay");
    }

    [Client]
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                HandleInput();
            }
        }
    }
}
