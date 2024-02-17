using UnityEngine;
using System.Collections.Generic;

public class LevelBuilder : MonoBehaviour
{
    public static LevelBuilder Instance;

    public GameObject[] SpawnerPrefabs;
    private List<GameObject> _spawners = new List<GameObject>();

    private void Start()
    {
        StartLevel();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    public void BuildLevel()
    {
        foreach (GameObject spawner in SpawnerPrefabs)
        {
            var currentSpawner = Instantiate(spawner);

            _spawners.Add(currentSpawner);

            if(currentSpawner.GetComponent<ILevelSpawner>() != null)
                currentSpawner.GetComponent<ILevelSpawner>().Build();
        }
    }
    public void ResetLevel()
    {
        foreach (GameObject spawner in _spawners)
        {
            if (spawner.GetComponent<ILevelSpawner>() != null)
                spawner.GetComponent<ILevelSpawner>().Clear();
            Destroy(spawner);
        }
        _spawners.Clear();
    }
    public void StartLevel()
    {
        ResetLevel();
        BuildLevel();
    }
}