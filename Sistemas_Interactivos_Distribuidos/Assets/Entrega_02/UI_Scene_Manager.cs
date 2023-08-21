using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Scene_Manager : MonoBehaviour
{
    public static UI_Scene_Manager Instance {get; private set;} = null;

    [SerializeField] private RectTransform Register_To_Login_Panel,Login_To_Game_Panel, Score_Panel,Main_Panel,Game_Panel,End_Panel,Exit_Sesion_Panel,UI_Center,UI_Rigth,UI_Left,FadeOute, FadeOute_Final;
    [SerializeField] private UI_Content Register_To_Login_Panel_Content, Login_To_Game_Panel_Content, Score_Panel_Content, Exit_Sesion_Panel_Content, End_Panel_Content;
    private bool _game,_score,_main,_end;

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
        Sequence fade_Sequence = DOTween.Sequence();

        fade_Sequence.PrependInterval(1)
        .Append(FadeOute.DOMove(FadeOute_Final.position,1));
        
        _game = false;
        _score = false;
        _main = true;
    }
    
    public void Register_To_Login(string username, string id)
    {
        Register_To_Login_Panel_Content._content[0].text = "Usuario: " + username;
        Register_To_Login_Panel_Content._content[1].text = "ID: " + id;

        Sequence animation_Sequence = DOTween.Sequence();

        animation_Sequence.Append(Register_To_Login_Panel.DOMove(UI_Center.position,1))
        .AppendInterval(2)
        .Append(Register_To_Login_Panel.DOMove(UI_Left.position,1))
        .AppendInterval(0.5f)
        .Append(FadeOute.DOMove(UI_Center.position,1)).OnComplete(Change_To_Login);
    }

    public void Login_To_Game(string username, string id)
    {
        Login_To_Game_Panel_Content._content[0].text = "Usuario: " + username;
        Login_To_Game_Panel_Content._content[1].text = "ID: " + id;

        Sequence animation_Sequence = DOTween.Sequence();

        animation_Sequence.Append(Login_To_Game_Panel.DOMove(UI_Center.position,1))
        .AppendInterval(2)
        .Append(Login_To_Game_Panel.DOMove(UI_Left.position,1))
        .AppendInterval(0.5f)
        .Append(FadeOute.DOMove(UI_Center.position,1)).OnComplete(Change_To_Game);
    }

    public void Login_To_Register()
    {
        Sequence animation_Sequence = DOTween.Sequence();

        animation_Sequence.Append(FadeOute.DOMove(UI_Center.position,1)).OnComplete(Change_To_Register);
    }

    public void Game_To_Login(string username)
    {
        Exit_Sesion_Panel_Content._content[0].text = username;

        Sequence animation_Sequence = DOTween.Sequence();

        animation_Sequence.Append(Exit_Sesion_Panel.DOMove(UI_Center.position,1))
        .AppendInterval(2)
        .Append(Exit_Sesion_Panel.DOMove(UI_Left.position,1))
        .AppendInterval(0.5f)
        .Append(FadeOute.DOMove(UI_Center.position,1)).OnComplete(Change_To_Login);
    }

    public void Game_Panel_Activate()
    {
        if(_game)
        {
            Game_Panel.gameObject.SetActive(false);
            _game = false;
            
        }
        else
        {
            Game_Panel.gameObject.SetActive(true);
            _game = true;
        }
    }

    public void End_Panel_Activate( int i)
    {
        if(_end)
        {
            End_Panel.gameObject.SetActive(false);
            _end = false;
            
        }
        else
        {
            End_Panel.gameObject.SetActive(true);
            End_Panel_Content._content[0].text = i.ToString();
            _end = true;
        }
    }

    public void Main_Panel_Activate()
    {
        if(_main)
        {
            Main_Panel.gameObject.SetActive(false);
            _main = false;
            
        }
        else
        {
            Main_Panel.gameObject.SetActive(true);
            _main = true;
        }
    }

    public void Score_Panel_Activation(UserData[] users)
    {
        if(_score)
        {
            Score_Panel.gameObject.SetActive(false);
            _score = false;
            
        }
        else
        {
            for(int i = 0; i< users.Length;i++)
            {
                Score_Panel_Content._content[i].text = users[i].username + ": " + users[i].data.score + " Puntos";
            }

            Score_Panel.gameObject.SetActive(true);
            _score = true;
        }
    }

    public void Score_Panel_Activation()
    {
        if(_score)
        {
            Score_Panel.gameObject.SetActive(false);
            _score = false;
            
        }
        else
        {

            Score_Panel.gameObject.SetActive(true);
            _score = true;
        }
    }

    private void Change_To_Login()
    {
        DOTween.Clear();
        SceneManager.LoadScene(1);
    }

    private void Change_To_Game()
    {
        DOTween.Clear();
        SceneManager.LoadScene(2);
    }

    private void Change_To_Register()
    {
        DOTween.Clear();
        SceneManager.LoadScene(0);
    }
}

[System.Serializable]
public class UI_Content
{
    public List<TextMeshProUGUI> _content;
}
