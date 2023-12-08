using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomizedChestManager : MonoBehaviourPun
{
    public List<Transform> itemTransform;
    public Transform fusePos;
    public ObjectToSpawnData objectInsideChest;
    public bool multipleItemsPositions;
    public bool isFuseBox;
    public List<Mesh> chestMesh;

    public void FillChest() 
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (objectInsideChest != null)
            {
                if (!multipleItemsPositions)
                {
                    if(isFuseBox)
                    {
                        PhotonNetwork.Instantiate(objectInsideChest.ObjectPrefab.name, fusePos.position, fusePos.rotation);
                    }
                    else
                    {
                        
                        PhotonNetwork.Instantiate(objectInsideChest.ObjectPrefab.name, itemTransform[0].position, itemTransform[0].rotation);
                    }
                }
                else
                {
                    var index = Random.Range(0, itemTransform.Count);
                    var itemPos = itemTransform[index];
                    
                    if(isFuseBox)
                    {
                        PhotonNetwork.Instantiate(objectInsideChest.ObjectPrefab.name, fusePos.position, fusePos.rotation);
                    }
                    else
                    {
                        
                        PhotonNetwork.Instantiate(objectInsideChest.ObjectPrefab.name, itemPos.position, itemPos.rotation);
                    }
                }
            }
            else
            {
                Debug.LogError($"Chest can't be filled", this.transform);
            }
        }
    }
}
