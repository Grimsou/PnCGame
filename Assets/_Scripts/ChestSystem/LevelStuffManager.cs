using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelStuffManager : MonoBehaviour
{
    public static LevelStuffManager Instance;
    
    [Header("--- RoomManagers in levels ---")]
    public List<RoomStuffManager> roomManagers;
    
    [Header("--- Chest prefabs ---")]
    public RandomizedChestManager smallChestPrefab;
    public RandomizedChestManager mediumChestPrefab;
    public RandomizedChestManager largeChestPrefab;
    
    [Header(" --- Objects Data ---")]
    [Tooltip("Each object needs an ObjectToSpawnData scriptable object")]
    [SerializeField] private List<ObjectsInLevel> _objectsInLevel;
    
    [Header(" --- Fuse objects to spawn ---")]
    [Tooltip("Needs an ObjectToSpawnData scriptable object")]
    [SerializeField] private ObjectsInLevel _fuseData;
    
    [HideInInspector] public List<Transform> itemPositionsOnMap;
    
    private List<GameObject> _chestList = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    private void Start()
    {
        _chestList.Add(Instance.smallChestPrefab.gameObject);
        _chestList.Add(Instance.mediumChestPrefab.gameObject);
        _chestList.Add(Instance.largeChestPrefab.gameObject);
        
        if(PhotonNetwork.IsMasterClient)
        {
            Shuffle(roomManagers);
            
            for (int i = 0; i < _fuseData.numberToSpawn; i++)
            {
                var randIndex = Random.Range(0, roomManagers[i].cratePositions.Count);
                
                var chest = PhotonNetwork.Instantiate(largeChestPrefab.gameObject.name, roomManagers[i].cratePositions[randIndex].position, roomManagers[i].cratePositions[randIndex].rotation);
                
                var chestManager = chest.GetComponent<RandomizedChestManager>();

                chestManager.isFuseBox = true;
                chestManager.objectInsideChest = _fuseData.ObjectData;
                chestManager.FillChest();
                roomManagers[i].cratePositions.Remove(roomManagers[i].cratePositions[randIndex]);
            }
            
            foreach (var roomManager in roomManagers)
            {
                itemPositionsOnMap.AddRange(roomManager.cratePositions);
            }
            
            Shuffle(itemPositionsOnMap);
            FillRoomWithStuff();
        }
    }

    public ObjectToSpawnData GetStuffFromManager()
    {
        if (_objectsInLevel.Count <= 0)
        {
            return null;
        }
        
        int randomIndex = Random.Range(0, _objectsInLevel.Count);
        ObjectToSpawnData objData = null;
        objData = _objectsInLevel[randomIndex].ObjectData;
        
        if (_objectsInLevel[randomIndex].numberToSpawn <= 1)
        {
            _objectsInLevel[randomIndex].numberToSpawn--;
            _objectsInLevel.Remove(_objectsInLevel[randomIndex]);
        }
        else
        {
            _objectsInLevel[randomIndex].numberToSpawn--;
        }
        return objData;
    }

    public void FillRoomWithStuff()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            var iterationCount = GetItemsNumber(_objectsInLevel);
            
            for (int i = 0; i < iterationCount; i++)
            {
                //Fetch a random position inside
                int cratePosIndex = Random.Range(0, Instance.itemPositionsOnMap.Count);
                var crate = Instance.itemPositionsOnMap[cratePosIndex];
                
                RandomizedChestManager chest = null;
                Shuffle(_objectsInLevel);
                var objectToSpawn = Instance.GetStuffFromManager();
                
                GameObject generatedChest;
                GameObject chestObj;
                int chestIndex;
                
                Debug.Log($"Size of the object {objectToSpawn.SizeOfTheObject.ToString()}");
                
                switch (objectToSpawn.SizeOfTheObject)
                {
                    case ObjectToSpawnData.ObjectSize.EMPTY:
                        var randIndex = Random.Range(0, _chestList.Count);
                        chestObj = _chestList[randIndex];
                        generatedChest = PhotonNetwork.Instantiate(chestObj.name, crate.position, crate.rotation);
                        break;
                    
                    case ObjectToSpawnData.ObjectSize.SMALL:
                        chestIndex = Random.Range(_chestList.IndexOf(smallChestPrefab.gameObject), _chestList.IndexOf(mediumChestPrefab.gameObject));
                        chestObj = _chestList[chestIndex];
                        generatedChest = PhotonNetwork.Instantiate(chestObj.name, crate.position, crate.rotation);
                        chest = generatedChest.GetComponent<RandomizedChestManager>();
                        break;
                    
                    case ObjectToSpawnData.ObjectSize.MEDIUM | ObjectToSpawnData.ObjectSize.SMALL:
                        chestIndex = Random.Range(_chestList.IndexOf(mediumChestPrefab.gameObject), _chestList.IndexOf(largeChestPrefab.gameObject));
                        chestObj = _chestList[chestIndex];
                        generatedChest = PhotonNetwork.Instantiate(chestObj.name, crate.position, crate.rotation);
                        chest = generatedChest.GetComponent<RandomizedChestManager>();
                        break;
                    
                    case ObjectToSpawnData.ObjectSize.BIG:
                        chestObj = _chestList[_chestList.IndexOf(largeChestPrefab.gameObject)];
                        generatedChest = PhotonNetwork.Instantiate(chestObj.name, crate.position, crate.rotation);
                        chest = generatedChest.GetComponent<RandomizedChestManager>();
                        break;
                    
                    default:
                        generatedChest = new GameObject();
                        Debug.LogError($"This object size is not handled yet {objectToSpawn.SizeOfTheObject.ToString()}", generatedChest);
                        break;
                }

                if (chest != null)
                {
                    chest.objectInsideChest = objectToSpawn;
                    chest.FillChest();
                }
                
                itemPositionsOnMap.Remove(itemPositionsOnMap[cratePosIndex]);
            }
        }
    }

    private static void Shuffle<T>(List<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private int GetItemsNumber(List<ObjectsInLevel> objectsInLevels)
    {
        int sum = 0;
        foreach (var inLevel in objectsInLevels)
        {
            sum += inLevel.numberToSpawn;
        }
        return sum;
    }
}

[Serializable]
public class ObjectsInLevel
{
    public int numberToSpawn;    
    [SerializeField] private ObjectToSpawnData _objectData;

    public ObjectToSpawnData ObjectData
    {
        get => _objectData;
        private set => value = _objectData;
    }
}
