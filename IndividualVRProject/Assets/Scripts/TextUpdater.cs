using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdater : MonoBehaviour
{
    private GameManager m_GameManager;
    private Text m_Text;
    private int val1;
    private int val2;
    private bool add;
    // Start is called before the first frame update
    void Start()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        m_Text = transform.Find("Text").gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setValues(int value1, int value2, bool a)
    {
        val1 = value1;
        val2 = value2;
        add = a;
        ConstructText();
    }

    public void Complete()
    {
        ConstructText(true);
    }

    private void ConstructText(bool complete = false)
    {
        m_Text.text = $"{val1}{(add ? " + " :  " x ")}{val2} = {(complete ? (add ? (val1+val2).ToString() : (val1*val2).ToString()) : "?")}";
    }
}
