using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BlockStack : MonoBehaviour
{
    public float Speed;

    private List<Block> _blocks = new List<Block>();

    private void Start()
    {
        if (PlayerScore.Instance.Score * 0.05f > 0.5f)
            Speed = 0.5f;
        else
            Speed += PlayerScore.Instance.Score * 0.05f;
    }
    private void FixedUpdate()
    {
        float verticalMovement = -Speed * Time.fixedDeltaTime;

        Vector3 currentPosition = transform.position;

        Vector3 newPosition = currentPosition + new Vector3(0f, verticalMovement, 0f);

        transform.position = newPosition;
    }
    public void RemoveMe(Block block)
    { 
        _blocks.Remove(block);

        if (_blocks.Count == 0)
        {
            PlayerScore.Instance.AddScore();
            BlockSpawner.Instance.Clear();
            BlockSpawner.Instance.Build();

            Destroy(gameObject);
        }
    }
    public void AddMe(Block block)
    { 
        _blocks.Add(block);
    }
    public void DestroyAll()
    {
        GameState.Instance.FinishGame();
        foreach (var block in _blocks)
        {
            block.DestroyMe(true);
        }
    }
}
