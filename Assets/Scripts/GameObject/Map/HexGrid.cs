using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class HexGrid : NetworkBehaviour {

    [SyncVar]
    public int width = 6;
    [SyncVar]
    public int height = 6;
    public GameObject cellPrefab;
    public Text cellLabelPrefab;
    public Color defaultColor = Color.white;

    private HexCell[] cells;
    public HexCell[] Cells {
        get { return cells; }
    }
    private Canvas gridCanvas;
    private HexMesh hexMesh;

    // Add label with coordinates on "cell"
    private void CreateCellLabel(HexCell cell)
    {
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(cell.transform.localPosition.x, cell.transform.localPosition.z);
        label.text = cell.Coordinates.ToStringOnSeparateLines();
    }

    // Create all cell, server side
    [Server]
    private GameObject CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        GameObject cellGO = Instantiate<GameObject>(cellPrefab);
        HexCell cell = cells[i] = cellGO.GetComponent<HexCell>();
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.Init(0, defaultColor);

        CreateCellLabel(cell);

        return cellGO;
    }

    // Get cell object by position
    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;

        return cells[index];
    }

    void Update()
    {
        hexMesh.Triangulate(cells);
    }

    public override void OnStartServer()
    {
        cells = new HexCell[height * width];
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellGO = CreateCell(x, z, i++);
                NetworkServer.Spawn(cellGO);
            }
        }
    }

    public override void OnStartClient()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = GameObject.FindObjectsOfType<HexCell>();
        Array.Reverse(cells);
        foreach (HexCell cell in cells)
        {
            cell.transform.SetParent(transform, false);
        }
    }
}
