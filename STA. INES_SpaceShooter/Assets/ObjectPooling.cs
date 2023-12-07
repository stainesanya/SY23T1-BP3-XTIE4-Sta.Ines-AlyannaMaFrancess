using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectsToPool
{
    playerBullet,
    enemyRed,
    enemyGreen,
    enemyYellow,
    enemyBullet,

    enemyNozzle,
}


[System.Serializable]
public struct ObjectStruct
{
    [SerializeField] private GameObject obj;
    [SerializeField] private int capacity;

    public GameObject Obj
    {
        get { return obj; }
        set { obj = value; }
    }

    public int Capacity
    {
        get { return Mathf.Abs(capacity); }
        set { capacity = Mathf.Abs(value); }
    }
}

public class ObjectPooling : Singleton<ObjectPooling>
{
    [SerializeField] private ObjectStruct[] ObjectPoolingList;

    private Dictionary<string, Queue<GameObject>> PoolingDict = new Dictionary<string, Queue<GameObject>>();

   public override void Awake()
{
    base.Awake();

    foreach (ObjectStruct _o in ObjectPoolingList)
    {
        Queue<GameObject> _objList = new Queue<GameObject>();

        for (int i = 0; i < _o.Capacity; i++)
        {
            GameObject _obj = Instantiate(_o.Obj);
            _obj.SetActive(false);
            _obj.name = _o.Obj.name;
            _objList.Enqueue(_obj);
        }

        PoolingDict.Add(_o.Obj.name, _objList);
    }

   
    PoolingDict.Add(ObjectsToPool.enemyNozzle.ToString(), new Queue<GameObject>());
}

    public GameObject InstanceCreate(ObjectsToPool _obj, Vector2 _pos)
    {
        if (PoolingDict.ContainsKey(_obj.ToString()) && PoolingDict[_obj.ToString()].Count < 1)
        {
            ObjectStruct objectStruct = ObjectPoolingList[(int)_obj];
            GameObject newObj = Instantiate(objectStruct.Obj);
            newObj.name = objectStruct.Obj.name;
            newObj.SetActive(true);
            newObj.transform.position = _pos;
            return newObj;
        }

        GameObject _getObj = PoolingDict[_obj.ToString()].Dequeue();
        _getObj.SetActive(true);
        _getObj.transform.position = _pos;

        return _getObj;
    }

    public GameObject InstanceCreate(ObjectsToPool _obj, Transform parentTransform)
    {
        GameObject newObj = InstanceCreate(_obj, parentTransform.position);
        newObj.transform.parent = parentTransform;
        return newObj;
    }

    public GameObject InstanceCreate(ObjectsToPool _obj, Vector2 _pos, float angle)
    {
        GameObject newObj = InstanceCreate(_obj, _pos);
        newObj.transform.rotation = Quaternion.Euler(0, 0, angle);
        return newObj;
    }

    public static ObjectPooling GetInstance()
    {
        return instance;
    }

    public void InstanceReturn(GameObject _obj)
    {
        _obj.SetActive(false);
        PoolingDict[_obj.name].Enqueue(_obj);
    }
}
