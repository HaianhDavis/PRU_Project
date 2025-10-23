using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 5f;

    private Vector3 _startPos;

    private bool _moveRight = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float leftBound = _startPos.x - distance;
        float rightBound = _startPos.x + distance;
        if (_moveRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                _moveRight = false;
                Flip(); 
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                _moveRight = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}