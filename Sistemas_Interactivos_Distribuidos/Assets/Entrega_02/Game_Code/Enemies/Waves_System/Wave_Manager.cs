using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wave_Manager : MonoBehaviour
{
    public static Wave_Manager Instance {get;private set;} = null;

    private int _wave_Index = 0;

    private Wave _wave;

    public TextMeshProUGUI txt;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        _wave = new Wave();
    }

    void Start()
    {
        Enemy_Spawn_System.OnWaveEnd += OnWaveEnds;
    }

    public void startGame()
    {
        StartCoroutine(NextWave());
    }

    void OnDisable()
    {
        Enemy_Spawn_System.OnWaveEnd -= OnWaveEnds;
    }

    void StartWave()
    {
        _wave_Index++;
        txt.text = "Round: " + _wave_Index;
        Increase_Dificulty();
        StartCoroutine(Enemy_Spawn_System.Instance.SpawnWave(_wave));
    }

    void OnWaveEnds()
    {
        StartCoroutine(NextWave());
    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(2);
        StartWave();
    }

    void Increase_Dificulty()
    {
        _wave._bachitombo_Count = _wave_Index;
        _wave._tombo_Count = _wave_Index;
        _wave._tombo_Tactico_Count = _wave_Index;
        _wave._tombo_Con_Perro_Count = _wave_Index;
        _wave._esmad_Count = _wave_Index;

        //Aumentar la dificultad
    }
}
