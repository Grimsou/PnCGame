using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ObjectToSpawnData", fileName = "New ObjectDataToSpawn")]
public class ObjectToSpawnData : ScriptableObject
{
    public enum ObjectSize
    {
        SMALL,
        MEDIUM,
        BIG,
        EMPTY
    }
    
    [SerializeField] private GameObject _objectPrefab;
    [SerializeField] private ObjectSize _sizeOfTheObject;
    
    public GameObject ObjectPrefab
    {
        get => _objectPrefab;
        private set => value = _objectPrefab;
    }

    public ObjectSize SizeOfTheObject
    {
        get => _sizeOfTheObject;
        private set => value = _sizeOfTheObject;
    }
}
    