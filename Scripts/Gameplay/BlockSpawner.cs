using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public static BlockSpawner Instance;

    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private Color32[] _blockColors;
    [SerializeField] private Transform _spawnPosition;

    private List<GameObject> _blocks = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        Build();
    }
    public void Build()
    {
        GameObject block = Instantiate(_prefabs[Random.Range(0, _prefabs.Length)], _spawnPosition.position, Quaternion.identity);

        Vector3 startScale = block.transform.localScale;
        Vector2 startPosition = block.transform.localPosition;

        block.transform.localScale = Vector3.zero;
        block.transform.position = startPosition + (Vector2.up * 3f);

        block.transform.DOScale(startScale, 0.3f).SetEase(Ease.OutBack).SetLink(block);
        block.transform.DOMove(startPosition, 0.4f).SetEase(Ease.OutBack).SetLink(block);

        _blocks.Add(block);
    }
    public void Clear()
    {
        foreach (var block in _blocks)
        {
            Destroy(block);
        }
    }
    public Color32 GetBlockColor(BlockType type)
    {
        return _blockColors[(int)type];
    }
}