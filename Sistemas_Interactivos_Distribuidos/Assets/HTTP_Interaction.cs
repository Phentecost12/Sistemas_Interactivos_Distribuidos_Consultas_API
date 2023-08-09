using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class HTTP_Interaction : MonoBehaviour
{

    private readonly string Json_Server_URL = "https://my-json-server.typicode.com/Phentecost12/DBJson_Server/users";
    private readonly string Rick_and_Morty_API_URL = "https://rickandmortyapi.com/api/character";
    private int UserToSerch = 1;
    public List<RawImage> Card_Images;
    public List<TextMeshProUGUI> Card_Texts;
    public TextMeshProUGUI playerTEXT;
    public TextMeshProUGUI playerIDTEXT;
    public List<InfoPanel> infoPanels;

    void Update()
    {
        playerIDTEXT.text = "ID: " + UserToSerch.ToString();
    }

    public void SendRequest()
    {
        StartCoroutine(GetUsers());
    }

    public void ChangeID(int i)
    {
        UserToSerch = Mathf.Clamp(UserToSerch+i,1,5);
    }

    IEnumerator GetUsers() 
    {
        UnityWebRequest request = UnityWebRequest.Get(Json_Server_URL + "/" + UserToSerch);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            User MyUser = JsonUtility.FromJson<User>(request.downloadHandler.text);
            playerTEXT.text = MyUser.username + " #" + MyUser.id;

            for (int i = 0; i < MyUser.deck.Count; i++)
            {
                StartCoroutine(GetCharacter(MyUser.deck[i], i));
            }
        }
    }

 

    IEnumerator GetCharacter(int characterID, int i) 
    {
        UnityWebRequest request = UnityWebRequest.Get(Rick_and_Morty_API_URL + "/" + characterID);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError) 
        {
            Debug.Log(request.error);
        }
        else 
        {
            Character character = JsonUtility.FromJson<Character>(request.downloadHandler.text);
            Card_Texts[i].text = character.name;
            infoPanels[i].Status.text = character.status;
            infoPanels[i].Species.text = character.species;
            infoPanels[i].Gender.text = character.gender;
            StartCoroutine(GetImage(character.image,i));
        }
    }

 

    IEnumerator GetImage(string url, int i)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else if (request.result != UnityWebRequest.Result.ProtocolError) 
        {
            Card_Images[i].texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}

public class User
{
    public int id;
    public string username;
    public List<int> deck;
}

public class Character 
{
    public string name;
    public string status;
    public string species;
    public string gender;
    public string image;
}

[System.Serializable]
public class InfoPanel
{
    public TextMeshProUGUI Status;
    public TextMeshProUGUI Species;
    public TextMeshProUGUI Gender;
}