using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    private GameManager m_GameManager;
    private float waitTime = 5f;
    private float currentTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime < waitTime)
        {
            currentTime += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (currentTime < waitTime) return;
        //Debug.Log($"{gameObject.name} collided with {other.gameObject.name}");
        // TODO
        // Other gameobject name should be the name Collider__.
        // This means that the parent of this object (which is a block) should snap into position
        // at the parent of the other collider
        if (gameObject.name.StartsWith("Collider") && other.gameObject.name.StartsWith("Collider"))
        {
            m_GameManager.SnapObjects(gameObject, other.gameObject);
            //m_GameManager.MergeObjects(gameObject, other.gameObject);
        }
    }
}
