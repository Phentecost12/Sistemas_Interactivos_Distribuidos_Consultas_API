using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Linq;

public class HTTP_Users_Manager : MonoBehaviour
{
    public TMP_InputField UsernameInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField ScoreInputField;
    public string Data_Base;

    private string HTTP_Login_token;
    private string HTTP_Login_username;
    // Start is called before the first frame update
    void Start()
    {
        HTTP_Login_token = PlayerPrefs.GetString("token");

        if (string.IsNullOrEmpty(HTTP_Login_token))
        {
            Debug.Log("No hay ninguna sesion activa");
        }
        else
        {
            HTTP_Login_username = PlayerPrefs.GetString("username");
            StartCoroutine(GetPerfil(HTTP_Login_username));
        }

       
    }

    void Update()
    {
        if(Input.GetKeyDown( KeyCode.B))
        {
            Debug.Log(PlayerPrefs.GetString("token"));
            Debug.Log(PlayerPrefs.GetString("username"));
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
        StartCoroutine(GetAllUsers());
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
            }
            else
            {
                Debug.Log(request.error);
            }
        }
    }

    IEnumerator GetAllUsers()
    {
        UnityWebRequest request = UnityWebRequest.Get(Data_Base + "usuarios");
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
                UserList users = JsonUtility.FromJson<UserList>(request.downloadHandler.text);
                users.usuarios.OrderByDescending(u=> u.data.score);

                foreach (UserData user in users.usuarios)
                {
                    Debug.Log(user.username + " tiene un puntaje de: " + user.data.score);
                }
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
