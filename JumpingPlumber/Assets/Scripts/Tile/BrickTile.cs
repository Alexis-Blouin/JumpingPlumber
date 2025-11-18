using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tiles/BrickTile")]
public class BrickTile : Tile
{
    public enum Type
    {
        GrowMushroom,
        LifeMushroom,
        Star,
        Coin,
        Breakable,
        Unbreakable,
    }

    public Type type = Type.Unbreakable;
    public Sprite usedSprite;
    public GameObject itemPrefab;

    // Function similar to Start function for GameObjects
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        Debug.Log(type.ToString() + " Position: " + position);
        return false;
    }

    public void OnHit(Vector3Int cellPos, Tilemap tilemap)
    {
        switch (type)
        {
            case Type.GrowMushroom:
                Debug.Log("Growing mushroom");
                Vector3 worldPosition = tilemap.CellToWorld(cellPos);
                Vector3 position = new Vector3(worldPosition.x, worldPosition.y + 0.5f, worldPosition.z);
                Instantiate(itemPrefab, position, Quaternion.identity);
                break;
            
            case Type.LifeMushroom:
                Debug.Log("Life mushroom");
                break;
            
            case Type.Star:
                Debug.Log("Star");
                break;
            
            case Type.Coin:
                Debug.Log("Coin");
                break;
            
            case Type.Breakable:
                Debug.Log("Breakable");
                tilemap.SetTile(cellPos, null);
                break;
            
            case Type.Unbreakable:
                Debug.Log("Unbreakable");
                break;
        }
    }
}
