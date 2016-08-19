using UnityEngine;
using System;
using System.Collections;

public class CreateGrid : MonoBehaviour {
	
	public static int redshiftScale = 100; // The number of Unity units in one redshift unit (varies depending on data conversion programme)
	public static int lengthLimit = 100; // The radius of the outer circle
	public static int intervalLength = 25; // Length interval between circles
	public static string GlyDistance; // The distance in Gly for the circle labels

	void Start () {

		// Overriding and setting lengthLimit = 100 to avoid confusion about the age of the universe

//		// This 'block' gets the radius of the outer circle from WiggleZConstants.txt
//		// The value is not assigned here as it is used in several scripts, so it is easier to store it separately
//		TextAsset Constants = Resources.Load<TextAsset> ("Constants");
//		string[] ConstantsArray = Constants.text.Split ('\n'); // Create array, with each line in text file as an element
//		foreach (string constant in ConstantsArray) {
//			string[] parts_of_constant = constant.Split (':'); // Convert from string to array, e.g. visualisationSize:300 -> {visualisationSize, 300}
//			if (parts_of_constant [0] == "visualisationSize") {
//				lengthLimit = Int32.Parse (parts_of_constant [1]); // Assign value from text file to lengthLimit
//			}
//		}

		for (int i = intervalLength; i <= lengthLimit; i+= intervalLength) {
			GameObject gridCircle = new GameObject ("GridCircle"); // Create the circle
			Circle.drawCircle (gridCircle, i); // Call drawCircle function from separate C# script

			// Create the text giving the redshift
			GameObject redshiftText = new GameObject ("RedshiftText"); // Create text for labelling each circle with a distance
			var redshiftTextMesh = redshiftText.AddComponent<TextMesh> ().text = ("Redshift " + ((float)i/redshiftScale).ToString("n2")); // Set the text for each label
			redshiftText.GetComponent<TextMesh> ().fontSize = 15; // Resize each label
			redshiftText.GetComponent<TextMesh> ().alignment = TextAlignment.Center;
			redshiftText.transform.position = new Vector3 (-4, 0, i); // Put each label in its appropriate place
			redshiftText.transform.Rotate (35, 0, 0);

			// Create text giving the comoving distance in Gly
			GameObject comovingText = new GameObject ("ComovingText"); // Create text for labelling each circle with a distance
			if (i == 25) { // I hate that I have to do this, but there exists no simple formula for calculating the comoving distance (calculator: http://www.astro.ucla.edu/~wright/CosmoCalc.html)
				GlyDistance = "3";

			} else if (i == 50) {
				GlyDistance = "6";

			} else if (i == 75) {
				GlyDistance = "9";

			} else if (i == 100) {
				GlyDistance = "11";

			}
			// Disregard the rest as I'm cutting it off at z = 1 to avoid confusion about age of universe
//			} else if (i == 125) {
//				GlyDistance = "13";
//
//			} else if (i == 150) {
//				GlyDistance = "14";
//
//			} else if (i == 175) {
//				GlyDistance = "16";
//
//			} else if (i == 200) {
//				GlyDistance = "17";
//
//			}

			var comovingTextMesh = comovingText.AddComponent<TextMesh> ().text = (GlyDistance + " billion light-years"); // Set the text for each label
			comovingText.GetComponent<TextMesh> ().fontSize = 15; // Resize each label
			comovingText.GetComponent<TextMesh> ().alignment = TextAlignment.Center;
			comovingText.transform.position = new Vector3 (7, 0, -i); // Put each label in its appropriate place
			comovingText.transform.Rotate (35, 180, 0);
		}
	}
}