using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Models;
using System.Linq;
using System;

public class DatabaseHandler : MonoBehaviour
{
    public static DatabaseHandler Instance;

    private string token = "";

    private void Start()
    {
        token = PlayerPrefs.GetString("token", "");

        if (token == "")
        {
            MenuController.Instance.ChangePage(0);
        }
        else
        {
            MenuController.Instance.ChangePage(1);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetToken(string token)
    {
        PlayerPrefs.SetString("token", token);
        this.token = token;
    }

    public void Login(string username, string password)
    {
        RequestHelper request = new RequestHelper
        {
            Uri = "http://localhost:3000/auth/login",
            Body = new User
            {
                username = username,
                password = password
            }
        };

        RestClient.Post<Response>(request).Then((res) =>
        {
            PlayerPrefs.SetString("token", res.message);
            Debug.Log(res.message);
            MenuController.Instance.SetAuthErrorMessage(res.message);

            if (res.code == "1")
            {
                MenuController.Instance.ChangePage(1);
            }
            token = res.message;
        }).Catch((err) =>
           {
               MenuController.Instance.SetAuthErrorMessage(err.Message);
               Debug.Log(err.Message);
           });
    }

    public void Register(string username, string password)
    {
        RequestHelper request = new RequestHelper
        {
            Uri = "http://localhost:3000/auth/register",
            Body = new User
            {
                username = username,
                password = password
            }
        };

        RestClient.Post<Response>(request).Then((res) =>
        {
            MenuController.Instance.SetAuthErrorMessage(res.message);

            Debug.Log(res.message);
        }).Catch((err) =>
           {
               MenuController.Instance.SetAuthErrorMessage(err.Message);

               Debug.Log(err.Message);
           });
    }

    public void GetLeaderboard(Action<List<ScoreData>> onSucces)
    {
        RequestHelper request = new RequestHelper
        {
            Uri = "http://localhost:3000/leaderboard/list",
            Body = new Token
            {
                token = token,
            }
        };

        request.Headers.Add("x-access-token", token);

        RestClient.Get<Response>(request).Then((res) =>
        {

            if (res.code == "1")
            {

                List<ScoreData> scores = JsonHelper.ArrayFromJson<ScoreData>(res.message).ToList();

                onSucces(scores);
            }

            MenuController.Instance.SetLeaderboardErrorMessage(res.message);


            RestClient.CleanDefaultHeaders();
        }).Catch((err) =>
           {
               RestClient.CleanDefaultHeaders();
               MenuController.Instance.SetLeaderboardErrorMessage(err.Message);

               Debug.Log(err.Message);
           });
    }

    public void UpdateScore(int score)
    {
        RequestHelper request = new RequestHelper
        {
            Uri = "http://localhost:3000/leaderboard/update",
            Body = new ScoreData
            {
                token = token,
                score = score
            }
        };

        request.Headers.Add("x-access-token", token);

        RestClient.Put<Response>(request).Then((res) =>
        {
            RestClient.CleanDefaultHeaders();

            MenuController.Instance.SetLeaderboardErrorMessage(res.message);

            Debug.Log(res.message);
        }).Catch((err) =>
           {
               RestClient.CleanDefaultHeaders();
               MenuController.Instance.SetLeaderboardErrorMessage(err.Message);

               Debug.Log(err.Message);
           });
    }
}

[System.Serializable]
public class User
{
    public string username;
    public string password;
}

[System.Serializable]
public class Response
{
    public string message;
    public string code;
}

[System.Serializable]
public class Token
{
    public string token;
}

[System.Serializable]
public class ScoreData
{
    public string token;
    public int score;
    public string username;
}