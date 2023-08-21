using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class Enemy_Spawn_System : MonoBehaviour
{
    [Header("Bachitombo")]
    [Space(5)]
    [SerializeField] private Bachitombo _bachitombo_Prefab;
    [SerializeField] private int _bachitombo_Pool_Default_Capacity;
    [SerializeField] private int _bachitombo_Pool_Max_Capacity;
    [SerializeField] private bool _bachitombo_Pool_Collection_Check;
    private ObjectPool<Bachitombo> _bachitombo_Pool;

    [Header("Tombo")]
    [Space(5)]
    [SerializeField] private Tombo _tombo_Prefab;
    [SerializeField] private int _tombo_Pool_Default_Capacity;
    [SerializeField] private int _tombo_Pool_Max_Capacity;
    [SerializeField] private bool _tombo_Pool_Collection_Check;
    private ObjectPool<Tombo> _tombo_Pool;

    [Header("Tombo Tactico")]
    [Space(5)]
    [SerializeField] private Tombo_Tactico _tombo_Tactico_Prefab;
    [SerializeField] private int _tombo_Tactico_Pool_Default_Capacity;
    [SerializeField] private int _tombo_Tactico_Pool_Max_Capacity;
    [SerializeField] private bool _tombo_Tactico_Pool_Collection_Check;
    private ObjectPool<Tombo_Tactico> _tombo_Tactico_Pool;

    [Header("Tombo Con Perro")]
    [Space(5)]
    [SerializeField] private Tombo_Con_Perro _tombo_Con_Perro_Prefab;
    [SerializeField] private int _tombo_Con_Pero_Pool_Default_Capacity;
    [SerializeField] private int _tombo_Con_Pero_Pool_Max_Capacity;
    [SerializeField] private bool _tombo_Con_Perro_Pool_Collection_Check;
    private ObjectPool<Tombo_Con_Perro> _tombo_Con_Pero_Pool;
    
    [Header("Esmad")]
    [Space(5)]
    [SerializeField] private Esmad _esmad_Prefab;
    [SerializeField] private int _esmad_Pool_Default_Capacity;
    [SerializeField] private int _esmad_Pool_Max_Capacity;
    [SerializeField] private bool _esmad_Pool_Collection_Check;
    private ObjectPool<Esmad> _esmad_Pool;

    private List<Enemy> _spawned_Enemy_Wave;
    private bool _active_Wave = false;

    public static Enemy_Spawn_System Instance {get; private set;} = null;
    public static event Action OnWaveEnd;

    public TextMeshProUGUI text;
    private int puntaje= 0;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        _bachitombo_Pool = new ObjectPool<Bachitombo>(()=> {
            return Instantiate(_bachitombo_Prefab);
        }, bachitombo => {
            bachitombo.gameObject.SetActive(true);
        }, bachitombo =>{
            bachitombo.gameObject.SetActive(false);
        }, bachitombo => {
            Destroy(bachitombo.gameObject);
        },_bachitombo_Pool_Collection_Check, _bachitombo_Pool_Default_Capacity,_bachitombo_Pool_Max_Capacity);

        _tombo_Pool = new ObjectPool<Tombo>(() => {
            return Instantiate(_tombo_Prefab);
        }, tombo => {
            tombo.gameObject.SetActive(true);
        }, tombo => {
            tombo.gameObject.SetActive(false);
        }, tombo => {
            Destroy(tombo.gameObject);
        }, _tombo_Pool_Collection_Check, _tombo_Pool_Default_Capacity,_tombo_Pool_Max_Capacity);

        _tombo_Tactico_Pool = new ObjectPool<Tombo_Tactico>(()=> {
            return Instantiate(_tombo_Tactico_Prefab);
        }, tombo_Tactico => {
            tombo_Tactico.gameObject.SetActive(true);
        }, tombo_Tactico => {
            tombo_Tactico.gameObject.SetActive(false);
        }, tombo_Tactico => {
            Destroy(tombo_Tactico.gameObject);
        }, _tombo_Tactico_Pool_Collection_Check,_tombo_Tactico_Pool_Default_Capacity,_tombo_Tactico_Pool_Max_Capacity);

        _tombo_Con_Pero_Pool = new ObjectPool<Tombo_Con_Perro>(()=> {
            return Instantiate(_tombo_Con_Perro_Prefab);
        }, tombo_Con_perro => {
            tombo_Con_perro.gameObject.SetActive(true);
        }, tombo_Con_perro =>{
            tombo_Con_perro.gameObject.SetActive(false);
        }, tombo_Con_Perro => {
            Destroy(tombo_Con_Perro.gameObject);
        }, _tombo_Con_Perro_Pool_Collection_Check, _tombo_Con_Pero_Pool_Default_Capacity, _tombo_Con_Pero_Pool_Max_Capacity);

        _esmad_Pool = new ObjectPool<Esmad>(() =>{
            return Instantiate(_esmad_Prefab);
        }, esmad => {
            esmad.gameObject.SetActive(true);
        }, esmad => {
            esmad.gameObject.SetActive(false);
        }, esmad=> {
            Destroy(esmad.gameObject);
        },_esmad_Pool_Collection_Check, _esmad_Pool_Default_Capacity, _esmad_Pool_Max_Capacity);
        
        _spawned_Enemy_Wave = new List<Enemy>();

        //Fill_Pools();

        text.text = "Points: " + puntaje;
    }

    void Update()
    {
        if(!_active_Wave) return;

        if(_spawned_Enemy_Wave.Count <= 0)
        {
            Debug.Log("Ronda Terminada");
            OnWaveEnd();
            _active_Wave = false;
        }
    }

    void Fill_Pools()
    {
        for (int i = 0; i<_bachitombo_Pool_Default_Capacity;i++)
        {
            var enemy = _bachitombo_Pool.Get();
            _bachitombo_Pool.Release(enemy);
        }

        for (int j = 0; j< _tombo_Pool_Default_Capacity;j++)
        {
            var enemy = _tombo_Pool.Get();
            _tombo_Pool.Release(enemy);
        }

        for (int k = 0; k< _tombo_Tactico_Pool_Default_Capacity;k++)
        {
            var enemy = _tombo_Tactico_Pool.Get();
            _tombo_Tactico_Pool.Release(enemy);
        }

        for (int l = 0; l< _tombo_Con_Pero_Pool_Default_Capacity;l++)
        {
            var enemy = _tombo_Con_Pero_Pool.Get();
            _tombo_Con_Pero_Pool.Release(enemy);
        }

        for (int m = 0; m< _esmad_Pool_Default_Capacity;m++)
        {
            var enemy = _esmad_Pool.Get();
            _esmad_Pool.Release(enemy);
        }
    }

    public IEnumerator SpawnWave (Wave wave)
    {
        
        if(wave._bachitombo_Count >0)
        {
            for (int i = 0; i< wave._bachitombo_Count;i++)
            {
                var enemy = _bachitombo_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach);
                yield return new WaitForSeconds(0.2f);
            }
        }

        if(wave._tombo_Count >0)
        {
            for (int i = 0; i< wave._tombo_Count;i++)
            {
                var enemy = _tombo_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach);
                yield return new WaitForSeconds(0.2f);
            }
        }

        if(wave._tombo_Tactico_Count >0)
        {
            for (int i = 0; i< wave._tombo_Tactico_Count;i++)
            {
                var enemy = _tombo_Tactico_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach);
                yield return new WaitForSeconds(0.2f);
            }
        }

        if(wave._tombo_Con_Perro_Count >0)
        {
            for (int i = 0; i< wave._tombo_Con_Perro_Count;i++)
            {
                var enemy = _tombo_Con_Pero_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach);
                yield return new WaitForSeconds(0.2f);
            }
        }

        if(wave._esmad_Count >0)
        {
            for (int i = 0; i< wave._esmad_Count;i++)
            {
                var enemy = _esmad_Pool.Get();
                _spawned_Enemy_Wave.Add(enemy);
                enemy.Config(OnDeath,OnReach);
                yield return new WaitForSeconds(0.2f);
            }
        }
        _active_Wave = true;
    }

    private void OnDeath(Enemy enemy, int i)
    {
        puntaje += i;
        text.text = "Points: " + puntaje;

        RemoveEnemy(enemy);
    }

    private void OnReach(Enemy enemy, int i)
    {
        RemoveEnemy(enemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _spawned_Enemy_Wave.Remove(enemy);
        if(enemy is Bachitombo){_bachitombo_Pool.Release((Bachitombo)enemy);}
        else if (enemy is Tombo){_tombo_Pool.Release((Tombo)enemy);}
        else if (enemy is Tombo_Tactico){_tombo_Tactico_Pool.Release((Tombo_Tactico)enemy);}
        else if (enemy is Tombo_Con_Perro){_tombo_Con_Pero_Pool.Release((Tombo_Con_Perro)enemy);}
        else if (enemy is Esmad){_esmad_Pool.Release((Esmad)enemy);}
    }
}

[System.Serializable]
public class Wave
{
   public int _bachitombo_Count;
   public int _tombo_Count;
   public int _tombo_Tactico_Count;
   public int _tombo_Con_Perro_Count;
   public int _esmad_Count;
}
