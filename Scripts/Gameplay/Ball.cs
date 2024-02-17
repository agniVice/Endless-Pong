using DG.Tweening;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Prefabs")]

    [SerializeField] private GameObject _particlePrefab;

    [Space]
    [Header("BallSettings")]

    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rigidbody;

    private Vector3 _direction;

    private bool _isMoving;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _direction = new Vector2(Random.Range(-3f, 3f), Random.Range(2f, 5f)).normalized;
        _isMoving = true;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (_isMoving)
            _rigidbody.velocity = _direction.normalized * _speed;
    }
    private void SpawnParticle()
    {
        var particle = Instantiate(_particlePrefab).GetComponent<ParticleSystem>();
        particle.transform.position = transform.position;
        particle.Play();

        Destroy(particle.gameObject, 3f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Plane")||
            collision.gameObject.CompareTag("Block"))
        {
            AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.BallSound, Random.Range(0.85f, 1.1f));
            _direction = Vector3.Reflect(_direction, collision.contacts[0].normal);
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            Camera.main.DOShakePosition(0.1f, 0.1f, fadeOut: true).SetUpdate(true);
            Camera.main.DOShakeRotation(0.1f, 0.1f, fadeOut: true).SetUpdate(true);

            collision.gameObject.GetComponent<Block>().DestroyMe();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            if (GameState.Instance.CurrentState == GameState.State.InGame)
            {
                GameState.Instance.FinishGame();

                GameObject.FindObjectOfType<BlockStack>().DestroyAll();

                SpawnParticle();

                _rigidbody.velocity = Vector2.zero;
                _isMoving = false;
            }
        }
    }
}
