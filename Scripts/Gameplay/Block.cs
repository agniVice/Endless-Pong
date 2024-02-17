using DG.Tweening;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockType CurrentType;

    [SerializeField] private GameObject _particlePrefab;

    private Collider2D _collider;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        GetComponent<SpriteRenderer>().color = BlockSpawner.Instance.GetBlockColor(CurrentType);
        GetComponentInParent<BlockStack>().AddMe(this);
    }
    private void SpawnParticle()
    {
        var particle = Instantiate(_particlePrefab).GetComponent<ParticleSystem>();
        particle.transform.position = transform.position;
        particle.Play();

        Destroy(particle.gameObject, 3f);
    }
    public void DestroyMe(bool gameCompleted = false)
    {
        if(!gameCompleted)
            GetComponentInParent<BlockStack>().RemoveMe(this);

        transform.DOScale(0, 0.2f).SetEase(Ease.InBack).SetLink(gameObject);

        GetComponent<SpriteRenderer>().DOFade(0, 0.2f).SetLink(gameObject);

        _collider.enabled = false;

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.ScoreAdd, Random.Range(1f, 1.15f));

        SpawnParticle();
        Destroy(gameObject, 0.4f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FinishBlock"))
        {
            GetComponentInParent<BlockStack>().DestroyAll();
        }
    }
}
