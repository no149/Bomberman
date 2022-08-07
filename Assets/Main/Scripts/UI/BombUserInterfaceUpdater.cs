using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using TMPro;



class BombUserInterfaceUpdater : MonoBehaviour
{
    public GameObject Bomb1Panel;
    public GameObject Bomb2Panel;
    public GameObject Bomb3Panel;
    static List<BombTypeCount> _bombtypesCount = new List<BombTypeCount>();
    static private void InitBombsCount()
    {
        _bombtypesCount = new List<BombTypeCount>(Player.Instance.Bombs);

    }

    public BombUserInterfaceData CurrentData
    {
        get
        {
            return new BombUserInterfaceData { BombTypesCount = _bombtypesCount };
        }
    }

    public void Init()
    {
        InitBombsCount();
        RefreshUI();
    }

    private void RefreshUI()
    {
        var bomb1CountTxt = Bomb1Panel.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        var bomb2CountTxt = Bomb2Panel.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        var bomb3CountTxt = Bomb3Panel.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();

        bomb1CountTxt.text = CurrentData[Bomb.BombType.Type1].ToString();
        bomb2CountTxt.text = CurrentData[Bomb.BombType.Type2].ToString();
        bomb3CountTxt.text = CurrentData[Bomb.BombType.Type3].ToString();
    }

    void Start()
    {
        Init();
        Player.Instance.BombSpawned += Bomb_Spawned;
    }

    private void Bomb_Spawned(object sender, Bomb bomb)
    {
        CurrentData.BombTypesCount= Player.Instance.Bombs;
        RefreshUI();
    }
}