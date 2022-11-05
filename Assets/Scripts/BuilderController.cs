using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuilderController : MonoBehaviour {
    [Header("General")]
    public PlayerController owner;


    [Header("Building Preview")]
    [SerializeField]
    private GameObject preview;
    [SerializeField]
    public GameObject sampleBuilding;
    [SerializeField]
    private float yOffset;


    [Header("Tiles")]
    [SerializeField]
    private GameObject sampleTile;
    [SerializeField]
    private Material blueMaterial;
    [SerializeField]
    private Material redMaterial;
    [SerializeField]
    private Vector2 tileDisplayOffset;


    private Tilemap map;
    private readonly Dictionary<Vector3Int, GameObject> occupiedTiles = new();
    private MeshRenderer previewRenderer;
    private readonly Dictionary<Vector3Int, GameObject> hoverTiles = new();

    void Start() {
        map = GetComponent<Tilemap>();
        previewRenderer = preview.GetComponent<MeshRenderer>();
        PlaceTile(Vector3Int.zero);
    }

    GameObject PlaceTile(Vector3Int position) {
        var displayOffset = new Vector3(tileDisplayOffset.x, 0, tileDisplayOffset.y);
        var displayPosition = map.CellToWorld(position) + displayOffset;
        return Instantiate(sampleTile, displayPosition, Quaternion.identity);
    }

    void Update() {
        Vector3 mouseCellCenter = MovePreview();
        Cleanup();
        GetBaseMinMaxCells(out Vector3Int baseMinCell, out Vector3Int baseMaxCell);

        bool canBuildHere = CanBuildHere(baseMinCell, baseMaxCell);
        if (!canBuildHere) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            // Build
            Build(mouseCellCenter, baseMinCell, baseMaxCell);
        }
    }

    Vector3 MovePreview() {
        // USE LAYER MASK???
        CameraController.RaycastMouse(out var hit);
        var clickPos = hit.point;
        var clickedCell = map.WorldToCell(clickPos);

        var cellCenter = map.GetCellCenterWorld(clickedCell);
        preview.transform.position = cellCenter + yOffset * Vector3.up;
        return cellCenter;
    }


    void Cleanup() {
        foreach (var tile in hoverTiles.Values) {
            Destroy(tile);
        }
        foreach (var tile in occupiedTiles.Values) {
            tile.GetComponent<MeshRenderer>().material = blueMaterial;
        }
        hoverTiles.Clear();
    }

    void GetBaseMinMaxCells(out Vector3Int baseMinCell, out Vector3Int baseMaxCell) {
        var bounds = previewRenderer.bounds;
        Vector3 baseMin = bounds.min;
        Vector3 baseMax = bounds.max - bounds.size.y * Vector3.up;
        baseMinCell = map.WorldToCell(baseMin);
        baseMaxCell = map.WorldToCell(baseMax);
    }

    bool CanBuildHere(Vector3Int baseMinCell, Vector3Int baseMaxCell) {
        bool canBuildHere = true;
        for (int row = baseMinCell.x; row <= baseMaxCell.x; ++row) {
            for (int col = baseMinCell.y; col <= baseMaxCell.y; ++col) {
                var cell = new Vector3Int(row, col, 0);

                if (occupiedTiles.TryGetValue(cell, out var occupiedTile)) {
                    canBuildHere = false;
                    occupiedTile.GetComponent<MeshRenderer>().material = redMaterial;
                } else {
                    var newTile = PlaceTile(cell);
                    hoverTiles.Add(cell, newTile);
                }
            }
        }
        return canBuildHere;
    }

    void Build(Vector3 buildPos, Vector3Int baseMinCell, Vector3Int baseMaxCell) {
        var building = Instantiate(sampleBuilding, buildPos, Quaternion.identity);
        var spawner = building.GetComponentsInChildren<SpawnerController>().First();
        spawner.owner = this.owner;

        for (int row = baseMinCell.x; row <= baseMaxCell.x; ++row) {
            for (int col = baseMinCell.y; col <= baseMaxCell.y; ++col) {
                var cell = new Vector3Int(row, col, 0);
                var newTile = PlaceTile(cell);
                occupiedTiles.Add(cell, newTile);
            }
        }
    }

}
