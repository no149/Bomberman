using UnityEngine;
using System.Collections.Generic;
using TMPro;



class BombHUD : MonoBehaviour
{
    public GameObject Bomb1Panel;
    public GameObject Bomb2Panel;
    public GameObject Bomb3Panel;

    BombUserInterfaceData _currentData = new BombUserInterfaceData() { };
    private void InitBombsCount()
    {
        _currentData.BombTypesCount = new List<BombTypeCount>(Player.Instance.AvailableBombs);

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

        bomb1CountTxt.text = _currentData[Bomb.BombType.Type1].ToString();
        bomb2CountTxt.text = _currentData[Bomb.BombType.Type2].ToString();
        bomb3CountTxt.text = _currentData[Bomb.BombType.Type3].ToString();
    }

    void Start()
    {
        Init();
        Player.Instance.BombSpawned += Bomb_Spawned;
        Player.Instance.SelectedBombChanged += CurrentBombChanged;
    }

    void CurrentBombChanged(object sender, Bomb.BombType type)
    {
        switch (type)
        {
            case Bomb.BombType.Type1:
                Bomb1Panel.GetComponent<UnityEngine.UI.Image>().color = new Color32(0xEA, 0xE7, 0x29, 0xff);
                Bomb2Panel.GetComponent<UnityEngine.UI.Image>().color = new Color32(0xff, 0xff, 0xff, 0xff);
                Bomb3Panel.GetComponent<UnityEngine.UI.Image>().color = new Color32(0xff, 0xff, 0xff, 0xff);
                break;
            case Bomb.BombType.Type2:
                Bomb2Panel.GetComponent<UnityEngine.UI.Image>().color = new Color32(0xEA, 0xE7, 0x29, 0xff);
                Bomb1Panel.GetComponent<UnityEngine.UI.Image>().color = new Color32(0xff, 0xff, 0xff, 0xff);
                Bomb3Panel.GetComponent<UnityEngine.UI.Image>().color = new Color32(0xff, 0xff, 0xff, 0xff); 
                break;
            case Bomb.BombType.Type3:
                Bomb3Panel.GetComponent<UnityEngine.UI.Image>().color = new Color32(0xEA, 0xE7, 0x29, 0xff);
                Bomb2Panel.GetComponent<UnityEngine.UI.Image>().color = new Color32(0xff, 0xff, 0xff, 0xff);
                Bomb1Panel.GetComponent<UnityEngine.UI.Image>().color = new Color32(0xff, 0xff, 0xff, 0xff);
                break;
        }
    }
    private void Bomb_Spawned(object sender, Bomb bomb)
    {
        _currentData.BombTypesCount = Player.Instance.AvailableBombs;
        RefreshUI();
    }
}