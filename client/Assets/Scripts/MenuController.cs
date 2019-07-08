using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour 
{
	public static MenuController Instance;

	public InputField usernameField;
	public InputField passwordField;
	public Text authErrorMessageText;
	public InputField scoreField;
	public Text leaderboardErrorMessageText;

	public GameObject scoreDataSample;
	public Transform scoresTransform;

	public GameObject[] pages;

	private void Awake()
	{
		if (Instance == null) {
			Instance = this;
		}
	}

	public void SetLeaderboardErrorMessage(string message)
	{
		leaderboardErrorMessageText.text = message;
	}

	public void SetAuthErrorMessage(string message)
	{
		authErrorMessageText.text = message;
	}

	public void ChangePage(int index)
	{
		for (int i = 0; i < pages.Length; i++) {
			pages [i].SetActive (false);
		}

		pages [index].SetActive (true);
	}

	public void Login()
	{
		DatabaseHandler.Instance.Login (usernameField.text,passwordField.text);
	}

	public void Register()
	{
		DatabaseHandler.Instance.Register (usernameField.text,passwordField.text);
	}

	public void GetLeaderboard()
	{
		DatabaseHandler.Instance.GetLeaderboard (InitLeaderboard);
	}

	private void InitLeaderboard(List<ScoreData> scores)
	{
		for (int i = 0; i < scoresTransform.childCount; i++) {
			Destroy (scoresTransform.GetChild(i).gameObject);
		}

		for (int i = 0; i < scores.Count; i++) {
			GameObject score = Instantiate (scoreDataSample, scoresTransform, false);

			score.transform.GetChild (0).GetComponent<Text> ().text = scores [i].username;
			score.transform.GetChild (1).GetComponent<Text> ().text = scores [i].score.ToString();
		}
	}

	public void UpdateScore()
	{
		if(scoreField.text == "")
		{
			return;
		}

		DatabaseHandler.Instance.UpdateScore (int.Parse (scoreField.text));
	}

	public void Logout()
	{
		DatabaseHandler.Instance.SetToken ("");
		ChangePage (0);
	}
}
