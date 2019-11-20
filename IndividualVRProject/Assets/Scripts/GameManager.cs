using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextUpdater m_TextUpdater;
    [SerializeField] private GameObject m_Block1;
    [SerializeField] private GameObject m_Block2;

    [SerializeField] private GameObject m_Empty;
    public int[] m_Range = {2, 10};
    private int val1;
    private int val2;

    private bool isComplete = true;

    private System.Random random = new System.Random();

    private bool isAdd;

    private bool shouldSnap = false;
    private float completeTime = 5f;
    private GameObject objectToDelete;
    private List<string> multSnapping = new List<string>();

    private float m_size;
    
    // Start is called before the first frame update
    void Start()
    {
        m_size = m_Block1.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (isComplete)
        {
            // start new round
            isAdd = random.Next(0, 2) == 0;
            val1 = random.Next(m_Range[0], m_Range[1]);
            val2 = random.Next(m_Range[0], m_Range[1]);

            m_TextUpdater.setValues(val1, val2, isAdd);

            SpawnObjects(val1, val2, isAdd); // TODO


            isComplete = false;
        }
    }

    private IEnumerator CompleteCoroutine()
    {
        yield return new WaitForSeconds(completeTime);
        if (objectToDelete != null)
        {
            Destroy(objectToDelete);
        }
        isComplete = true;
        multSnapping.Clear();

        
    }

    public void Complete()
    {
        m_TextUpdater.Complete();
        StartCoroutine("CompleteCoroutine");
    }

    private void SpawnObjects(int value1, int value2, bool addition)
    {
        Vector3 pos = m_Block1.transform.position;
        pos.y -= 2;
        // TODO
        if (addition)
        {
            
            GameObject obj1 = CreateGameObject(pos, value1, "Add1");
            GameObject obj2 = CreateGameObject(new Vector3(pos.x, pos.y, pos.z+2), value2, "Add2");

            for (var i = 0; i < value1; ++i)
            {
                GameObject new_Block = Instantiate(m_Block1, obj1.transform);
                new_Block.GetComponent<MeshRenderer>().enabled = true;
                new_Block.transform.localPosition = new Vector3(i * m_size, 0, 0);
                new_Block.name = $"Add1b{i}";
                ToggleAddColliders(new_Block, i, value1);

            }
            for (var i = 0; i < value2; ++i)
            {
                GameObject new_Block = Instantiate(m_Block1, obj2.transform);
                new_Block.GetComponent<MeshRenderer>().enabled = true;
                new_Block.transform.localPosition = new Vector3(i * m_size, 0, 0);
                new_Block.name = $"Add2b{i}";
                ToggleAddColliders(new_Block, i, value2);

            }
        }
        else
        {
            for (var i = 0; i < value1; ++i)
            {
                GameObject obj1 = CreateGameObject(new Vector3(pos.x + value2 / 2f * (float) random.NextDouble(), pos.y, pos.z+(i*1.5f)), value2, $"Mult{i}");
                for (var j = 0; j < value2; ++j)
                {
                    GameObject new_Block = Instantiate(m_Block1, obj1.transform);
                    new_Block.GetComponent<MeshRenderer>().enabled = true;
                    new_Block.transform.localPosition = new Vector3(j * m_size, 0, 0);
                    new_Block.name = $"b{j}";
                    ToggleMultColliders(new_Block);

                }
            }
            
        }
    }

    private void ToggleAddColliders(GameObject o, int index, int total)
    {
        Dictionary<string, bool> colliders = DefaultCollidersDict();
        if (index == 0)
        {
            colliders["X1"] = true;
        }
        if (index == total - 1)
        {
            colliders["X2"] = true;
        }
        ToggleColliders(o, colliders);
    }

    private void ToggleMultColliders(GameObject o)
    {
        ToggleColliders(o, new Dictionary<string,bool>()
        {
            {"X1", false},
            {"X2", false},
            {"Y1", true},
            {"Y2", true},
            {"Z1", true},
            {"Z2", true}
        });
    }

    private Dictionary<string, bool> DefaultCollidersDict()
    {
        return new Dictionary<string, bool>()
        {
            {"X1", false},
            {"X2", false},
            {"Y1", false},
            {"Y2", false},
            {"Z1", false},
            {"Z2", false}
        };
    }

    private void ToggleColliders(GameObject block, Dictionary<string, bool> toggle)
    {
        foreach(Transform child in block.transform)
        {
            child.gameObject.GetComponent<BoxCollider>().enabled = toggle[child.gameObject.name.Substring(8)];
        }
    }

    private GameObject CreateGameObject(Vector3 pos, int size, string name = "Blocks")
    {
        GameObject o = Instantiate(m_Empty);
        o.transform.position = pos;
        BoxCollider bc = o.GetComponent<BoxCollider>();
       
        bc.size = new Vector3(size * m_size, m_size, m_size);
        bc.center = new Vector3((size/2.0f - 0.5f )* m_size, 0, 0);

        o.GetComponent<Rigidbody>().isKinematic = false;

        if (name == "Blocks")
        {
            name = $"Blocks{size}";
        }
        o.name = name;
        return o;
    }

    public void SnapObjects(GameObject o1, GameObject o2)
    {
        // assume that o1 is the object we are mergin into
        // if add then
        //    if o2.name == "ColliderX1" then
        //       add objects in o2.parent.parent to o1.parent.parent starting at x-distance of (# of objects in o1.parent.parent)
        //    else
        //       do the opposite of above and prepend these objects

        if (!shouldSnap) {
            shouldSnap = true;
            return;
        }
        

        String name1 = o1.gameObject.name.Substring(8);
        String name2 = o2.gameObject.name.Substring(8);
        GameObject o1Parent = o1.transform.parent.parent.gameObject;
        GameObject o2Parent = o2.transform.parent.parent.gameObject;
        //Debug.Log($"o1: {o1.gameObject.name}, o1.parent: {o1.transform.parent.gameObject.name}, o1.parent.parent: {o1.transform.parent.parent.gameObject.name}");
        //Debug.Log($"o2: {o2.gameObject.name}, o2.parent: {o2.transform.parent.gameObject.name}, o2.parent.parent: {o2.transform.parent.parent.gameObject.name}");
        if (isAdd) // add
        {
            if (name1 == "X1") 
            {
                if (name2 == "X1" || name2 == "X2")
                {
                    Transform[] buffer = new Transform[o2Parent.transform.childCount];
                    int index = 0;
                    // add to left of o1
                    foreach (Transform child in o2Parent.transform)
                    {
                        buffer[index++] = child;
                    }

                    index = 0;
                    foreach (Transform child in buffer)
                    {
                        //Debug.Log(child.gameObject.name);
                        child.parent = o1Parent.transform;
                        child.localPosition = new Vector3(--index * m_size, 0, 0);
                        child.localRotation = Quaternion.identity;
                    }

                   
                    BoxCollider bc = o1Parent.GetComponent<BoxCollider>();
                    Vector3 oldCenter = bc.center;
                    bc.size = new Vector3(o1Parent.transform.childCount * m_size, m_size, m_size);
                    bc.center = oldCenter + new Vector3(index * m_size/2f, 0, 0);

                    // Vector3 pos;
                    // // slide everything so it is centered
                    // foreach (Transform child in o1Parent.transform)
                    // {
                    //     pos = child.localPosition;
                    //     pos.x -= index;
                    //     child.localPosition = pos;
                    // }

                    // pos = o1Parent.transform.localPosition;
                    
                    // pos.x += index;
                    // o1Parent.transform.localPosition = pos;
                    //o1Parent.transform.position = o1Parent.transform.TransformPoint(pos);

                }
            }
            else if (name1 == "X2")
            {
                if (name2 == "X1" || name2 == "X2")
                {
                    Transform[] buffer = new Transform[o2Parent.transform.childCount];
                    int index = 0;
                    // add to left of o1
                    foreach (Transform child in o2Parent.transform)
                    {
                        buffer[index++] = child;
                    }

                    index = o1Parent.transform.childCount - 1;

                    foreach (Transform child in buffer)
                    {
                        //Debug.Log(child.gameObject.name);
                        child.parent = o1Parent.transform;
                        child.localPosition = new Vector3(++index * m_size, 0, 0);
                        child.localRotation = Quaternion.identity;
                    }
                }

                BoxCollider bc = o1Parent.GetComponent<BoxCollider>();
                bc.size = new Vector3(o1Parent.transform.childCount * m_size, m_size, m_size);
                bc.center = new Vector3((o1Parent.transform.childCount / 2f - .5f) * m_size, 0, 0);
            }

            // turn off all colliders
            Destroy(o2Parent);
            foreach (Transform child in o1Parent.transform)
            {
                ToggleColliders(child.gameObject, DefaultCollidersDict());
            }



            // send complete message
            objectToDelete = o1Parent;
            Complete();
        }
        else // multiply
        {
            
            if (o1Parent.name != o2Parent.name)
            {
                if (multSnapping.Contains(o1Parent.name) && multSnapping.Contains(o2Parent.name)) return;
                multSnapping.Clear();
                multSnapping.Add(o1Parent.name);
                multSnapping.Add(o2Parent.name);
               
                if (name1 == "Z1" || name1 == "Z2")
                {
                    var lowestPos = FindLowestPosition(o1Parent, "Z");
                    var highestPos = FindHighestPosition(o1Parent, "Z");
                    foreach (Transform child in o1Parent.transform)
                    {
                        var defaultDict = DefaultCollidersDict();
                        if (name1 == "Z1")
                        {
                            defaultDict["Z2"] = true;
                        }
                        else
                        {
                            defaultDict["Z1"] = true;
                        }
                        ToggleColliders(child.gameObject, defaultDict);
                    }
                    var o2ChildCount = o2Parent.transform.childCount;
                    for (var i = 1; i <= Math.Floor((double) o2ChildCount / val2); ++i)
                    {
                        Debug.Log(i);
                        for (var j = 0; j < val2; ++j)
                        {
                            Transform child = o2Parent.transform.GetChild(0);
                            child.parent = o1Parent.transform;
                            Vector3 pos = o1.transform.parent.localPosition;
                            if (name1 == "Z1")
                                pos.z -= i * m_size + lowestPos;
                            else if (name1 == "Z2")
                                pos.z += i * m_size + highestPos;
                            pos.x = j * m_size;
                            child.localPosition = pos;
                            child.localRotation = Quaternion.identity;

                            if (i == Math.Floor((double)o2ChildCount / val2))
                            {
                                var defaultDict = DefaultCollidersDict();

                                
                                defaultDict[name1] = true;
                                ToggleColliders(child.gameObject, defaultDict);
                            }
                        }
                    }

                    BoxCollider bc = o1Parent.GetComponent<BoxCollider>();
                    bc.size = new Vector3(val2 * m_size, m_size, (o1Parent.transform.childCount / val2) * m_size);
                    //bc.center = new Vector3((o1Parent.transform.childCount / 2f - .5f) * m_size, 0, 0);
                    
                }
                
                if (name1 == "Y1" || name1 == "Y2")
                {
                    var lowestPos = FindLowestPosition(o1Parent, "Y");
                    var highestPos = FindHighestPosition(o1Parent, "Y");
                    foreach (Transform child in o1Parent.transform)
                    {
                        var defaultDict = DefaultCollidersDict();
                        if (name1 == "Y1")
                        {
                            defaultDict["Y2"] = true;
                        }
                        else
                        {
                            defaultDict["Y1"] = true;
                        }
                        ToggleColliders(child.gameObject, defaultDict);
                    }
                    var o2ChildCount = o2Parent.transform.childCount;
                    for (var i = 1; i <= Math.Floor((double)o2ChildCount / val2); ++i)
                    {
                        for (var j = 0; j < val2; ++j)
                        {
                            Transform child = o2Parent.transform.GetChild(0);
                            child.parent = o1Parent.transform;
                            Vector3 pos = o1.transform.parent.localPosition;
                            if (name1 == "Y1")
                                pos.y -= i * m_size + lowestPos;
                            else if (name1 == "Y2")
                                pos.y += i * m_size + highestPos;
                            pos.x = j * m_size;
                            child.localPosition = pos;
                            child.localRotation = Quaternion.identity;

                            if (i == Math.Floor((double)o2ChildCount / val2))
                            {
                                var defaultDict = DefaultCollidersDict();


                                defaultDict[name1] = true;
                                ToggleColliders(child.gameObject, defaultDict);
                            }
                        }
                    }

                    BoxCollider bc = o1Parent.GetComponent<BoxCollider>();
                    bc.size = new Vector3(val2 * m_size, (o1Parent.transform.childCount / val2) * m_size, m_size);
                }

                Destroy(o2Parent);
                if (o1Parent.transform.childCount == val1 * val2)
                {
                    objectToDelete = o1Parent;
                    Complete();
                }
            }


            
        }

        shouldSnap = false;
        
    }

    private float FindLowestPosition(GameObject o, string dim)
    {
        float m = 100;
        if (dim == "Y")
        {
            foreach (Transform child in o.transform)
            {
                m = Math.Min(child.localPosition.y, m);
            }
        }
        else if (dim == "Z")
        {
            foreach (Transform child in o.transform)
            {
                m = Math.Min(child.localPosition.z, m);
            }
        }
        return m;
    }

    private float FindHighestPosition(GameObject o, string dim)
    {
        float m = -100;
        if (dim == "Y")
        {
            foreach (Transform child in o.transform)
            {
                m = Math.Max(child.localPosition.y, m);
            }
        }
        else if (dim == "Z")
        {
            foreach (Transform child in o.transform)
            {
                m = Math.Max(child.localPosition.z, m);
            }
        }
        return m;
    }
}
