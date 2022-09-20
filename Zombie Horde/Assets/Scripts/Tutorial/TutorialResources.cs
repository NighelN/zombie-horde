using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TutorialResources : MonoBehaviour
{
    [SerializeField] private ResourceSystem resourceSystem;
    [SerializeField] private Tile tree;
    [SerializeField] private Vector3 treePosition;
    [SerializeField] private Tile rock;
    [SerializeField] private Vector3 rockPosition;

    // Start is called before the first frame update
    void Start()
    {
        resourceSystem.SpawnResource(treePosition, tree);
        resourceSystem.SpawnResource(rockPosition, rock);
    }
}
