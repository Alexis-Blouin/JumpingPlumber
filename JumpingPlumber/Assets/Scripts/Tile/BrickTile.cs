using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/BrickTile")]
public class BrickTile : Tile
{
    public enum Type
    {
        GrowMushroom,
        LifeMushroom,
        FireFlower,
        Star,
        Coin,
        Breakable,
        Unbreakable,
    }

    public Type type = Type.Unbreakable;
    public Tile usedTile;
    public GameObject itemPrefab;
    public uint numberOfActivation = 1;

    // Function similar to Start function for GameObjects
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        // Debug.Log(type.ToString() + " Position: " + position);
        return false;
    }

    public void OnHit(Vector3Int cellPos, Tilemap tilemap)
    {
        Vector3 worldPosition = new Vector3();
        Vector3 position = new Vector3();
        switch (type)
        {
            case Type.GrowMushroom:
                worldPosition = tilemap.CellToWorld(cellPos);
                position = new Vector3(worldPosition.x, worldPosition.y + 1.5f, worldPosition.z);
                Instantiate(itemPrefab, position, Quaternion.identity);

                // --numberOfActivation;
                // if (numberOfActivation == 0)
                // {
                tilemap.SetTile(cellPos, usedTile);
                // }
                break;
            
            case Type.LifeMushroom:
                worldPosition = tilemap.CellToWorld(cellPos);
                position = new Vector3(worldPosition.x, worldPosition.y + 1.5f, worldPosition.z);
                Instantiate(itemPrefab, position, Quaternion.identity);

                // --numberOfActivation;
                // if (numberOfActivation == 0)
                // {
                tilemap.SetTile(cellPos, usedTile);
                // }
                break;
            
            case Type.FireFlower:
                worldPosition = tilemap.CellToWorld(cellPos);
                position = new Vector3(worldPosition.x + 0.5f, worldPosition.y + 1.5f, worldPosition.z);
                Instantiate(itemPrefab, position, Quaternion.identity);

                // --numberOfActivation;
                // if (numberOfActivation == 0)
                // {
                tilemap.SetTile(cellPos, usedTile);
                // }
                break;
            
            case Type.Star:
                break;
            
            case Type.Coin:
                --numberOfActivation;
                if (numberOfActivation == 0)
                {
                    tilemap.SetTile(cellPos, usedTile);
                }
                
                break;
            
            case Type.Breakable:
                tilemap.SetTile(cellPos, null);
                break;
            
            case Type.Unbreakable:
                break;
        }
    }
}
