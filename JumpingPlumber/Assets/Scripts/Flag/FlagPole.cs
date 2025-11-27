using UnityEngine;

public class FlagPole : MonoBehaviour
{
    [SerializeField] private Transform flagTransform;
    [SerializeField] private Transform flagBottomTransform;
    [SerializeField] private float flagDropSpeed = 4.0f;
    
    private Vector3 _flagPosition;
    private Vector3 _flagBottomPosition;

    public bool canGoDown = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _flagPosition = flagTransform.position;
        _flagBottomPosition = flagBottomTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (canGoDown && _flagPosition.y > _flagBottomPosition.y)
        {
            flagTransform.position = new Vector3(_flagPosition.x, flagTransform.position.y - flagDropSpeed * Time.deltaTime, _flagPosition.z);
            _flagPosition.y = flagTransform.position.y;
        }
    }
}
