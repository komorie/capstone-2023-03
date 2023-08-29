using DataStructs;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LibraryData : Singleton<LibraryData>
{

    public List<CardStruct> Library { get; set; }

    protected override void Awake()
    {
        base.Awake();   
        DontDestroyOnLoad(this);
        
        Library = GameData.Instance.CardList
                .Where(card => card.type == "Attack" || card.type == "Skill" || card.type == "Viewer")
                .ToList();
    }
}
