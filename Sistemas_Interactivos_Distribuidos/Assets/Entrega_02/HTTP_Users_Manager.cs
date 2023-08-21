using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Linq;
using System.Net.Cache;
using UnityEngine.SceneManagement;

public class HTTP_Users_Manager : MonoBehaviour
{
    public TMP_InputField UsernameInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField ScoreInputField;
    public string Data_Base;

    private string HTTP_Login_token;
    private string HTTP_Login_username;
    private UI_Scene_Manager Instance;
    // Start is called before the first frame update
    void Start()
    {
        int i = SceneManager.GetActiveScene().buildIndex;
        HTTP_Login_username = PlayerPrefs.GetString("username");
        HTTP_Login_token = PlayerPrefs.GetString("token");
        Instance = UI_Scene_Manager.Instance;

        if( i != 2)
        {
            if (string.IsNullOrEmpty(HTTP_Login_token))
            {
                Debug.Log("No hay ninguna sesion activa");
            }
            else
            {
                StartCoroutine(GetPerfil(HTTP_Login_username));
            }
        }
    }
 

    public void Register() 
    {
        User_Registry data = new User_Registry();

       data.username = UsernameInputField.text;
       data.password = PasswordInputField.text;

        string json = JsonUtility.ToJson(data);

        StartCoroutine(SendRegister(json));
        
    }
    public void Login()
    {
        User_Registry data = new User_Registry();

        data.username = UsernameInputField.text;
        data.password = PasswordInputField.text;

        string json = JsonUtility.ToJson(data);

        StartCoroutine(SendLogin(json));
    }

    public void SetNewScore()
    {
        User_Registry_Score data = new User_Registry_Score();
        data.username = HTTP_Login_username;
        data.data = new Data();
        data.data.score = int.Parse(ScoreInputField.text);

        string json = JsonUtility.ToJson(data);

        StartCoroutine(SendScore(json));
        
    }

    public void GetAllUsers()
    {
        if(!string.IsNullOrEmpty(HTTP_Login_token))
        {
            StartCoroutine(GetUsers());
        }
    }

    public void CloseSecion()
    {
        Instance.Game_To_Login(HTTP_Login_username);
        HTTP_Login_token = string.Empty;
        HTTP_Login_username = string.Empty;

        PlayerPrefs.SetString("username", string.Empty);
        PlayerPrefs.SetString("token", string.Empty);
    }
 
    IEnumerator SendRegister(string json) 
    {
        UnityWebRequest request = UnityWebRequest.Put(Data_Base + "usuarios",json);
        request.SetRequestHeader("Content-Type", "application/json");
        request.method = "POST";
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if(request.responseCode == 200)
            {
                User_Token data = JsonUtility.FromJson<User_Token>(request.downloadHandler.text);
                Debug.Log("Se Registro usuario con id " + data.usuario._id);
                Instance.Register_To_Login(data.usuario.username, data.usuario._id);
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }
    IEnumerator SendLogin(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(Data_Base + "auth/login", json);
        request.SetRequestHeader("Content-Type", "application/json");
        request.method = "POST";
        yield return request.SendWebRequest();

 

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            if (request.responseCode == 200)
            {
                User_Token data = JsonUtility.FromJson<User_Token>(request.downloadHandler.text);
                Debug.Log("Inicio sesion usuario  " + data.usuario.username);
                PlayerPrefs.SetString("token", data.token);
                PlayerPrefs.SetString("username", data.usuario.username);

                HTTP_Login_username = data.usuario.username;
                HTTP_Login_token = data.token;

                Instance.Login_To_Game(data.usuario.username,data.usuario._id);
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }

 

    IEnumerator GetPerfil(string username) 
    {
        UnityWebRequest request = UnityWebRequest.Get(Data_Base + "usuarios/" + username);
        request.SetRequestHeader("x-token",HTTP_Login_token);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error");
        }
        else
        {
            if(request.responseCode == 200)
            {
                User_Token data = JsonUtility.FromJson<User_Token>(request.downloadHandler.text);
                Debug.Log("Se inicio sesion con el usuario: " + data.usuario.username);
                Debug.Log(data.usuario.data.score);
                Instance.Login_To_Game(data.usuario.username,data.usuario._id);
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }

    IEnumerator GetUsers()
    {
        UnityWebRequest request = UnityWebRequest.Get(Data_Base + "usuarios");
        request.SetRequestHeader("x-token",HTTP_Login_token);
        Debug.Log("XD");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            if(request.responseCode == 200)
            {
                UserList users = JsonUtility.FromJson<UserList>(request.downloadHandler.text);
                UserData[] users_to_send = users.usuarios.OrderByDescending(u=> u.data.score).ToArray();

                Instance.Score_Panel_Activation(users_to_send);
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }

    IEnumerator SendScore(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(Data_Base + "usuarios", json);
        request.SetRequestHeader("Content-Type", "application/json");
        request.method = "PATCH";
        request.SetRequestHeader("x-token",HTTP_Login_token);
        yield return request.SendWebRequest();

         if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error");
        }
        else
        {
            Debug.Log(request.downloadHandler.text);
            if(request.responseCode == 200)
            {
                //cosas
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }
}


 

[System.Serializable]
public class User_Token 
{

    public UserData usuario;
    public string token;
}

 
public class User_Registry
{
    public string username;
    public string password;
}

public class User_Registry_Score
{
    public string username;
    public Data data;
}
 
 public class UserList
 {
    public UserData[] usuarios;
 }

[System.Serializable]
public class UserData 
{
    public string _id;
    public string username;
    public bool estado;
    public Data data;
}

 

[System.Serializable]
public class Data 
{
    public int score; 
}
