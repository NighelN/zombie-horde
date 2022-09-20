using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceSystem : MonoBehaviour
{
    public static ResourceSystem instance;
    [SerializeField, Range(1, 5)] float gatherRange = 2f;
    [SerializeField] private LayerMask layerMask;
    
    public class Resource
    {
        public Resource(ResourceObject resourceObject, Vector3 Position)
        {
            durability = resourceObject.durability;
            this.resourceObject = resourceObject;
            position = Position;
        }

        public int durability;
        public ResourceObject resourceObject;
        public Vector3 position;
    }

    [System.Serializable]
    public class ItemGiven
    {
        public Item item;
        public int amount;
    }

    [SerializeField] private float harvestCooldown = 0.3f;
    [Header("Tilemaps")]
    [SerializeField] private Tilemap resourceHighTilemap;
    [SerializeField] private Tilemap resourceMediumTilemap;
    [SerializeField] private Tilemap resourceLowTilemap;
    [SerializeField] private Tilemap shadowTilemap;
    [SerializeField] private Tilemap structuresTilemap;
    [Space]
    [Header("Posible Resources")]
    [SerializeField] private ResourceObject[] resourceObjects;

    private List<Resource> resources = new List<Resource>();
    private Player player;
    private Transform playerTrans;
    private float harvestDelay = 0;

    private void Start()
    {
        instance = this;
        player = GameManager.playerObject.GetComponent<Player>();
        playerTrans = GameManager.playerObject.transform;
    }

    public Resource GetResource()
    {
        //Checks if the players raycast hit any of the resources available
        RaycastHit2D hit = Physics2D.Raycast(playerTrans.position, Vector2FromAngle(playerTrans.eulerAngles.z + 90), gatherRange, layerMask);
        if (hit.collider)
        {
            //Grabs the hit position of the ray cast
            //Also grabs the grid position
            var position = hit.point + Vector2FromAngle(playerTrans.eulerAngles.z + 90) * new Vector2(0.1f, 0.1f);
            var gridPosition = resourceHighTilemap.WorldToCell(position);
            
            //Checks if any of the resource tilemaps for the position isnt null
            if (resourceHighTilemap.GetTile(gridPosition) != null || resourceMediumTilemap.GetTile(gridPosition) != null || resourceLowTilemap.GetTile(gridPosition) != null)
                return resources.FirstOrDefault(resource => resource.position == gridPosition);
        }
        return null;
    }

    public bool CorrectTool(ToolData tool)
    {
        //Grabs the resource the player is trying to hit
        //And checks if the resource isnt null and the tool is in the source object
        var resource = GetResource();
        return resource != null && tool.resourceType.Contains(resource.resourceObject);
    }

    // This spawns a resource and keeps track of it in a list
    public void SpawnResource(Vector3 position, Tile tile)
    {
        Vector3Int gridPosition = resourceHighTilemap.WorldToCell(position);
        if (resourceHighTilemap.GetTile(gridPosition) == null && structuresTilemap.GetTile(gridPosition) == null && resourceLowTilemap.GetTile(gridPosition) == null && resourceMediumTilemap.GetTile(gridPosition) == null)
        {
            //Loops though all resource objects
            foreach (var resourceObject in resourceObjects)
            {
                //Loops though all the tiles within that resource object
                foreach (var resourceTile in resourceObject.tiles)
                {
                    //Checks if the resource tile matches the tile
                    if (resourceTile == tile)
                    {
                        //Grabs the height of the resource object
                        switch (resourceObject.height)
                        {
                            case ResourceObject.Heights.High:
                                tile.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, Random.Range(0, 360)), Vector3.one);
                                resourceHighTilemap.SetTile(gridPosition, tile);
                                if (resourceObject.hasShadows)
                                {
                                    shadowTilemap.SetTile(shadowTilemap.WorldToCell(position + shadowTilemap.transform.position), tile);
                                }
                                resources.Add(new Resource(resourceObject, gridPosition));
                                return;

                            case ResourceObject.Heights.Medium:
                                tile.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, Random.Range(0, 360)), Vector3.one);
                                resourceMediumTilemap.SetTile(gridPosition, tile);
                                if (resourceObject.hasShadows)
                                {
                                    shadowTilemap.SetTile(shadowTilemap.WorldToCell(position + shadowTilemap.transform.position), tile);
                                }
                                resources.Add(new Resource(resourceObject, gridPosition));
                                return;

                            case ResourceObject.Heights.Low:
                                tile.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, Random.Range(0, 360)), Vector3.one);
                                resourceLowTilemap.SetTile(gridPosition, tile);
                                if (resourceObject.hasShadows)
                                {
                                    shadowTilemap.SetTile(shadowTilemap.WorldToCell(position + shadowTilemap.transform.position), tile);
                                }
                                resources.Add(new Resource(resourceObject, gridPosition));
                                return;
                        }
                    }
                }
            }
        }
    }

    // Destroys resource if durability is 0 and gives resources
    public void DestroyResource(int damage)
    {
        //Checks if the harvest delay has ended
        if (Time.time > harvestDelay)
        {
            //Checks if the raycast from the player has hit any resource object
            RaycastHit2D hit = Physics2D.Raycast(playerTrans.position, Vector2FromAngle(playerTrans.eulerAngles.z + 90), gatherRange, layerMask);
            if (hit.collider)
            {
                //Grabs the tool the player is wearing
                var tool = player.tool.Get(player.tool.GetWeapon(player.inventorySlot), player.inventorySlot);
                
                //Grabs the position of where the raycast hit
                Vector3 position = hit.point + Vector2FromAngle(playerTrans.eulerAngles.z + 90) * new Vector2(0.1f, 0.1f);

                //Grabs the position of the hit
                Vector3Int gridPosition = resourceHighTilemap.WorldToCell(position);
                //Checks if there is any resource on the grid position
                if (resourceHighTilemap.GetTile(gridPosition) != null || resourceLowTilemap.GetTile(gridPosition) == null || resourceMediumTilemap.GetTile(gridPosition) == null)
                {
                    //Loops though the resources count
                    for (int i = 0; i < resources.Count; i++)
                    {
                        //Checks if the resource position matches the grid position
                        if (resources[i].position == gridPosition)
                        {
                            //Checks if the resource requires a tool
                            if (resources[i].resourceObject.tool != null)
                            {
                                //Checks if the player his tool is null
                                if (tool == null) return;
                                //Checks if the tool matches the resource tool
                                if (!tool.tool.Equals(resources[i].resourceObject.tool)) return;
                            }
                            //Checks if the new durability is lower then 1
                            if (resources[i].durability - damage <= 0)
                                damage += resources[i].durability - damage;

                            resources[i].durability -= damage;
                            Instantiate(resources[i].resourceObject.particleEffect, resources[i].position + new Vector3(0.5f,0.5f,0), Quaternion.identity);
                            
                            // Adds items to player inventory
                            foreach (var item in resources[i].resourceObject.itemsGivenPerHit)
                                player.inventory.Add(item.item.itemId, item.amount * damage);

                            //Checks if the resource your hitting is a large tree
                            //Large trees give the ability to give the player an apple
                            if (resources[i].resourceObject.name.Equals("Tree Large"))
                            {
                                var random = Random.Range(0, 100);
                                if (random < 5) player.inventory.Add(10);
                            }

                            //Check if the durability of the structure is lower then 1
                            if (resources[i].durability <= 0)
                            {
                                //Checks the height of the resource object
                                switch (resources[i].resourceObject.height)
                                {
                                    case ResourceObject.Heights.High:
                                        resourceHighTilemap.SetTile(gridPosition, null);
                                        if (resources[i].resourceObject.hasShadows)
                                        {
                                            shadowTilemap.SetTile(shadowTilemap.WorldToCell(position + shadowTilemap.transform.position), null);
                                        }
                                        resources.RemoveAt(i);
                                        break;
                                    case ResourceObject.Heights.Medium:
                                        resourceMediumTilemap.SetTile(gridPosition, null);
                                        if (resources[i].resourceObject.hasShadows)
                                        {
                                            shadowTilemap.SetTile(shadowTilemap.WorldToCell(position + shadowTilemap.transform.position), null);
                                        }
                                        resources.RemoveAt(i);
                                        break;
                                    case ResourceObject.Heights.Low:
                                        resourceLowTilemap.SetTile(gridPosition, null);
                                        if (resources[i].resourceObject.hasShadows)
                                        {
                                            shadowTilemap.SetTile(shadowTilemap.WorldToCell(position + shadowTilemap.transform.position), null);
                                        }
                                        resources.RemoveAt(i);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            //Sets the harvest delay
                            harvestDelay = Time.time + harvestCooldown;
                        }
                    }
                }
            }
        }
    }

    private Vector2 Vector2FromAngle(float a)
    {
        a *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
    }
}
