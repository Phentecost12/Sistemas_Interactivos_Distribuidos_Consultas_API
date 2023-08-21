using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Enemies;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance {get;private set;} = null;

    private Vector2 _screen_Position;
    private Vector2 _world_Position;

    private float time_Between_Touches = 0.2f;
    private float _timer;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && _timer <= 0)
        {
            _screen_Position = Input.GetTouch(0).position;
        }
        else if( Input.GetMouseButton(0) && _timer <= 0)
        {
            Vector3 mouse_Pos = Input.mousePosition;
            _screen_Position = new Vector2(mouse_Pos.x,mouse_Pos.y);
            //Poner un timer para optimizar/ Evitar llamadas inecesarias
        }
        else
        {
            _timer -= Time.deltaTime;
            return;
        }

        _world_Position = Camera.main.ScreenToWorldPoint(_screen_Position);

        RaycastHit2D hit = Physics2D.Raycast(_world_Position,Vector2.zero);

        if(hit.collider != null)
        {
            Enemy x = hit.collider.GetComponent<Enemy>();

            if(x != null)
            {
                x.OnTouched();
                _timer = time_Between_Touches;
            }
        }
    }
}
