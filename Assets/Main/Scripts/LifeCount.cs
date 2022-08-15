using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LifeCount : MonoBehaviour
{
    private static LifeCount _instance;

    internal static LifeCount Instance
    {
        get
        {
            return _instance;
        }
    }

    internal string Text
    {
        set
        {
            var tmp = GetComponent<TextMeshProUGUI>();
            tmp.text = value;
        }
    }
    void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
