using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;
using System.IO;
using System;
using System.Text;

public class Listener : ImageResultsListener
{
    private const string DELIMITER = ",";
    private const string NOT_A_NUMBER = "NaN";

    public float currentMetric;

    public StreamWriter outputFile;
	
	
	public Text textArea;
	
	void Start () {
	    outputFile = new StreamWriter (DateTime.Now.ToString ("yyyyMMdd_HHmmss") + "PlayerMetrics.csv");
        outputFile.WriteLine ("Time,Joy,Fear,Disgust,Sadness,Anger,Surprise,Contempt,Valence,Engagement,Smile,InnerBrowRaise,BrowRaise,BrowFurrow,NoseWrinkle," +
            "UpperLipRaise,LipCornerDepressor,ChinRaise,LipPucker,LipPress,LipSuck,MouthOpen,Smirk,EyeClosure,Attention");
	}
	
    public override void onFaceFound(float timestamp, int faceId)
    {
        Debug.Log("Found the face");
    }

    public override void onFaceLost(float timestamp, int faceId)
    {
        Debug.Log("Lost the face");
    }
    
    public override void onImageResults(Dictionary<int, Face> faces)
    {
		var rowToWrite = new StringBuilder ();

        rowToWrite.Append (Time.time + DELIMITER);
		
        if (faces.Count > 0)
        {
            DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer>();
            if (dfv != null)
            {
                dfv.ShowFace(faces[0]);
            }

            // Adjust font size to fit the selected platform.
            if ((Application.platform == RuntimePlatform.IPhonePlayer) ||
                (Application.platform == RuntimePlatform.Android))
            {
                textArea.fontSize = 36;
            }
            else
            {
                textArea.fontSize = 20;
            }

            textArea.text = faces[0].ToString();
            textArea.CrossFadeColor(Color.white, 0.2f, true, false);
			
			foreach (Emotions emotion in Enum.GetValues(typeof(Emotions))) 
            {
                faces [0].Emotions.TryGetValue (emotion, out currentMetric);
                rowToWrite.Append (currentMetric + DELIMITER);
            }

            foreach (Expressions expression in Enum.GetValues(typeof(Expressions))) 
            {
                faces [0].Expressions.TryGetValue (expression, out currentMetric);
                rowToWrite.Append (currentMetric + DELIMITER);
            }
        }
        else
        {
            textArea.CrossFadeColor(new Color(1, 0.7f, 0.7f), 0.2f, true, false);
			
			for (int x = 0; x < Enum.GetNames(typeof(Emotions)).Length + Enum.GetNames(typeof(Expressions)).Length; x++)
            {
                if (rowToWrite.Length > 0)
                {
                    rowToWrite.Append(DELIMITER);
                }
                rowToWrite.Append(NOT_A_NUMBER);
            }
        }
		
		outputFile.WriteLine (rowToWrite.ToString ());
    }
	
	void OnApplicationQuit ()
    {
        outputFile.Flush ();
        outputFile.Close ();
    }
    // Use this for initialization
    
	
	// Update is called once per frame
	void Update () {
	
	}
}