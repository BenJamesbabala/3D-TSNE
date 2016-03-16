using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class Logic : MonoBehaviour {


	List<Dictionary<string,float>> finalData;
	List<List<Dictionary<string,float>>> dataSet = new List<List<Dictionary<string, float>>>();
	List<GameObject> points = new List<GameObject>();
	// Use this for initialization
	void Start () {
		string startPath = Directory.GetParent(Directory.GetParent (Application.dataPath).FullName).FullName;
		string newPath = Path.Combine (startPath, "csvs/");
		//GameObject.Find ("debug").GetComponent<Text> ().text = newPath;
		DirectoryInfo dir = new DirectoryInfo (newPath);
		FileInfo[] info = dir.GetFiles("*.csv*");
		foreach (FileInfo f in info) {
			List<Dictionary<string,float>> data = CSVReader.Read (f.FullName);
			dataSet.Add (data);
		}

		Dictionary<float,Color> labelColors = new Dictionary<float, Color> ();
		for (var i = 0; i < dataSet[0].Count; i++) {
			GameObject point = (GameObject)Instantiate(Resources.Load("trainPoint"));
			if (labelColors.ContainsKey (dataSet[0] [i] ["label"])) {
				point.GetComponent<Renderer> ().material.color = labelColors [dataSet[0] [i] ["label"]];
			} else {
				labelColors.Add(dataSet[0] [i] ["label"],new Color(Random.value,Random.value,Random.value,1));
				point.GetComponent<Renderer> ().material.color = labelColors [dataSet[0] [i] ["label"]];
			}
			point.transform.localScale =  new Vector3(2.5f,2.5f,2.5f);
			point.transform.position = new Vector3(dataSet[0][i]["x"], dataSet[0][i]["y"], dataSet[0][i]["z"]);
			points.Add (point);
		}
	}
	
	// Update is called once per frame
	void Update () {
		int currentGoal = ((int)Time.fixedTime / 2) - 1;
		if (currentGoal > dataSet.Count -1) {
			currentGoal = dataSet.Count -1;
		}
		if (currentGoal < 0) {
			currentGoal = 0;
		}
		finalData = dataSet [currentGoal];
		for (var i = 0; i < dataSet[0].Count; i++) {
			float deltaPositionX = (finalData [i] ["x"] - points[i].transform.position.x) * (Time.deltaTime);
			float deltaPositionY = (finalData [i] ["y"] - points[i].transform.position.y) * (Time.deltaTime);
			float deltaPositionZ = (finalData [i] ["z"] - points[i].transform.position.z) * (Time.deltaTime);
			points [i].transform.position = new Vector3(points [i].transform.position.x + deltaPositionX, points [i].transform.position.y + deltaPositionY , points [i].transform.position.z + deltaPositionZ);
		}
	}
}
