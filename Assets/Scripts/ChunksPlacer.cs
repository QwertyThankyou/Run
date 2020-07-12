using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChunksPlacer : MonoBehaviour
{
    public Transform player;
    public Chunk[] chunkPrefabs;
    public Chunk firstChunk;

    private int _lastChunk;
    
    private List<Chunk> _spawnedChunks = new List<Chunk>();

    private void Start()
    {
        _spawnedChunks.Add(firstChunk);
        _lastChunk = 2;
    }

    private void Update()
    {
        if (player.position.x > _spawnedChunks[_spawnedChunks.Count - 1].end.position.x - 20)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        var chunk = Random.Range(0, chunkPrefabs.Length);
        if (chunk == _lastChunk)
            if (chunk > 0)
                chunk--;

        _lastChunk = chunk;
        
        Chunk newChunk = Instantiate(chunkPrefabs[chunk]);
        newChunk.transform.position = _spawnedChunks[_spawnedChunks.Count - 1].end.position - newChunk.begin.localPosition;
        _spawnedChunks.Add(newChunk);

        if (_spawnedChunks.Count >= 5)
        {
            Destroy(_spawnedChunks[0].gameObject);
            _spawnedChunks.RemoveAt(0);
        }
    }
}