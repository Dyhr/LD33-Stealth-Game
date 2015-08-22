using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{
    public bool Generate;
    public float TileSize;
    public float TileLow;
    public float TileHigh;
    public int Width;
    public int Height;
    public Material WallMaterial;
    public Material FloorMaterial;

    private float TotalWidth { get { return TileSize * Width; } }
    private float TotalHeight { get { return TileSize * Height; } }

    public enum TileType
    {
        Empty,
        Wall,
        WallLow,
    }

    private void Start()
    {
        if (Generate) Remake();
    }
    private void OnDrawGizmos()
    {
        if (Generate) Remake();
    }

    private TileType[,] GetMap()
    {
        var map = new TileType[Width, Height];
        for (int i = 0; i < Width; ++i)
        {
            for (int j = 0; j < Height; ++j)
            {
                if (i == 0 || i == Width - 1 || j == 0 || j == Height - 1)
                {
                    map[i, j] = TileType.Wall;
                }
                else
                {
                    map[i, j] = Random.value < 0.1f ? TileType.Wall : TileType.Empty;
                }
            }
        }
        return map;
    }

    private void Remake()
    {
        Generate = false;
        while (transform.childCount > 0) DestroyImmediate(transform.GetChild(0).gameObject);

        var map = GetMap();

        var floor = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        floor.parent = transform;
        floor.localScale = new Vector3(TileSize * Width, 0.1f, TileSize * Height);
        floor.localPosition = new Vector3(0, -0.05f, 0);
        floor.localRotation = Quaternion.identity;
        floor.GetComponent<MeshRenderer>().sharedMaterial = FloorMaterial;

        for (int i = 0; i < Width; ++i)
        {
            for (int j = 0; j < Height; ++j)
            {
                if (map[i, j] == 0) continue;

                var h = map[i, j] == TileType.WallLow ? TileLow : TileHigh;

                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                cube.parent = transform;
                cube.localScale = new Vector3(TileSize, h, TileSize);
                cube.localPosition = new Vector3(
                    -TotalWidth / 2 + TileSize * i + (Width % 2 == 1 ? TileSize / 2 : 0),
                    h / 2,
                    -TotalWidth / 2 + TileSize * j + (Height % 2 == 1 ? TileSize / 2 : 0));
                cube.localRotation = Quaternion.identity;
                cube.GetComponent<MeshRenderer>().sharedMaterial = WallMaterial;
            }
        }
    }
}
