using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTileInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private Tilemap visualTilemap;
    
    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        _CheckHeadHit();
    }
    
    private void _CheckHeadHit()
    {
        float distance = 0.1f * _boxCollider.bounds.size.y;
        float angle = 0.0f;
        float castSize = 0.9f;
        
        RaycastHit2D hit = Physics2D.BoxCast(
            _boxCollider.bounds.center,
            _boxCollider.bounds.size * castSize,
            angle,
            Vector2.up,
            distance,
            brickLayer
        );

        if (hit.collider != null)
        {
            Tilemap tilemap = hit.collider.GetComponent<Tilemap>();
            
            if (tilemap != null)
            {
                Vector3 hitPos = (Vector3)hit.point + Vector3.up * distance;
                Vector3Int cellPos = tilemap.WorldToCell(hitPos);
                
                TileBase tile = tilemap.GetTile(cellPos);
            
                if (tile is BrickTile brick)
                {
                    brick.OnHit(cellPos, tilemap, visualTilemap);
                    _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0.0f);
                }
            }
        }
    }
}
