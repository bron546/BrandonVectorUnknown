using System.Collections;
using UnityEngine;
 

/// <summary>
/// Unity to GoogleSheet connector Singleton
/// </summary>
public class UnityDataConnector : MonoBehaviour
{
	public bool updating;
	public string currentStatus;

	public static UnityDataConnector instance;

	void Awake()
	{
		if(instance == null)
			instance = this;
		else
			Destroy(gameObject);
	}

	void Start ()
	{
		updating = false;
		currentStatus = "Offline";

		DontDestroyOnLoad(gameObject);

		ConnectToGoogleSheet();
	}

    public void ConnectToGoogleSheet()
	{
		if (updating)
			return;

		StartCoroutine(StartConecting());   
	}

	IEnumerator StartConecting()
	{
		updating = true;

		string connectionString = Constants.url + "?ssid=" + Constants.id + "&sheet=" + Constants.statisticsSheetName + "&pass=" + Constants.password + "&action=GetData";
		WWW www = new WWW(connectionString);
		float elapsedTime = 0.0f;
		currentStatus = "Establishing Connection... ";
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;			
			if (elapsedTime >= Constants.maxWaitTime)
			{
				currentStatus = "Connection aborted. TimeUp.";
				Debug.Log(currentStatus);
				updating = false;
				break;
			}
			
			yield return null;  
		}

		if (!www.isDone || !string.IsNullOrEmpty(www.error))
		{
			currentStatus = "Connection error after" + elapsedTime.ToString() + "seconds: " + www.error;
			Debug.Log(currentStatus);
			updating = false;
			yield break;
		}

		currentStatus = "Connection established.";
		Debug.Log("Connection established... ");
		updating = false;
	}

    /// <summary>
    /// Pass a string array (size < 256), with the data to be saved on the defined google sheet.
    /// </summary>
    /// <param name="data"></param>
	public void SendDataToSheet(string[] data)
	{
		StartCoroutine(SendData(data));
	}
	
	IEnumerator SendData(string[] paramraw )
	{
        string data = "";
        for(int i = 0; i < paramraw.Length; i++)
        {
            data += "&val=" + WWW.EscapeURL(paramraw[i] + "   ");
        }
        
		if(!updating)
		{
            string connectionString = Constants.url + "?ssid=" + Constants.id + "&sheet=" + Constants.statisticsSheetName + "&pass=" + Constants.password
                                     + data + "&action=SetData";

			WWW www = new WWW(connectionString);
			float elapsedTime = 0.0f;
			while (!www.isDone)
			{
				elapsedTime += Time.deltaTime;			
				if (elapsedTime >= Constants.maxWaitTime)
				{
					break;
				}
                yield return null;  
			}
            Debug.Log(www.text);
            if (!www.isDone || !string.IsNullOrEmpty(www.error))
			{
				Debug.LogError ("Connection error while sending analytics... Error:" + www.error);

				// Error handling here.
				yield break;
			}
			
			if (www.text.Contains("OK"))
			{
				Debug.Log("Data Sent successfully.");
				yield break;
			}
		}
	}
}
	
	class Constants
{
    public static string password = "4V1Qs";
    public static float maxWaitTime = 20f;
    public static string statisticsSheetName = "Sheet1";

    public static string url = "https://script.google.com/a/asu.edu/macros/s/AKfycbx3fo87QQqzGeEpDPjU-DA6C16OkUGsV6Cnq_hB/exec";
    public static string id = "11CfWg1CKIitu53hMdbug30ZiqRIyqJYcwNDpPVj8Ads";
}