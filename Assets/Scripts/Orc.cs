using UnityEngine;

public class Orc : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 5f; // Phạm vi tuần tra
    [SerializeField] private float detectionRange = 7f; // Phạm vi phát hiện player
    [SerializeField] private float attackRange = 1.5f; // Phạm vi tấn công

    private Vector3 _startPos;
    private bool _moveRight = true;
    private Animator _animator;
    private Transform _player; // Tham chiếu đến player

    private void Awake()
    {
        _startPos = transform.position;
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform; // Tìm player với tag
        if (_animator == null)
        {
            Debug.LogWarning("Animator not found on Enemy! Animation will not work.", this);
        }
        if (_player == null)
        {
            Debug.LogWarning("Player not found! Ensure player has tag 'Player'.", this);
        }
    }

    private void Update()
    {
        if (_player == null || _animator == null) return;

        float leftBound = _startPos.x - distance;
        float rightBound = _startPos.x + distance;
        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Đuổi theo player
            float moveDirection = _player.position.x > transform.position.x ? 1f : -1f;
            transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);

            if (distanceToPlayer <= attackRange)
            {
                _animator.SetBool("isAttacking", true); // Bật animation tấn công
                _moveRight = false; // Dừng di chuyển khi tấn công
            }
            else
            {
                _animator.SetBool("isAttacking", false); // Tắt tấn công
                _moveRight = moveDirection > 0; // Cập nhật hướng di chuyển
            }

            // Flip hướng
            if (moveDirection != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(moveDirection), 1, 1);
            }
            _animator.SetBool("isRunning", true); // Bật animation chạy
        }
        else
        {
            // Tuần tra khi không thấy player
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
            _animator.SetBool("isRunning", true); // Chạy animation tuần tra
        }

        // Giới hạn vị trí để tránh di chuyển quá xa
        float xPos = Mathf.Clamp(transform.position.x, leftBound, rightBound);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        if (_animator != null)
        {
            _animator.SetBool("isRunning", true); // Đảm bảo animation tiếp tục
        }
    }

    // Reset attacking state khi animation kết thúc
    private void OnAttackAnimationEnd()
    {
        if (_animator != null)
        {
            _animator.SetBool("isAttacking", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_startPos, distance); // Phạm vi tuần tra
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Phạm vi phát hiện
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Phạm vi tấn công
    }
}