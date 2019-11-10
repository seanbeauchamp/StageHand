using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine;

public class StepsManager : MonoBehaviour
{
    public string docName;
    public static List<ActorSteps> actorSteps;

    // Start is called before the first frame update
    void Awake()
    {
        LoadStepData(docName);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void LoadStepData(string xmlFileName)
    {
        actorSteps = new List<ActorSteps>();

        TextAsset xmlData = (TextAsset)Resources.Load(xmlFileName);
        XmlDocument xmlOpeningDocument = new XmlDocument();
        xmlOpeningDocument.LoadXml(xmlData.text);

        //loop from root "stage" to the top level "steps"
        for (int n = 0; n < xmlOpeningDocument["stage"].ChildNodes.Count; n++)
        {
            XmlNode stepNode = xmlOpeningDocument["stage"].ChildNodes[n];
            string actorName = stepNode.Attributes["actor"].Value;
            actorSteps.Add(new ActorSteps(actorName));
            //loop through the step nodes of the steps parent
            foreach (XmlNode innerNode in stepNode.ChildNodes)
            {
                int tempOrder = 0;
                string tempAction ="";
                double tempValue = 0f;
                //now the three unique nodes inside the step
                foreach (XmlNode uniqueNode in innerNode.ChildNodes)
                {
                    switch (uniqueNode.Name)
                    {
                        case "order":
                            tempOrder = Convert.ToInt32(uniqueNode.InnerText);
                            break;
                        case "action":
                            tempAction = uniqueNode.InnerText;
                            break;
                        case "value":
                            tempValue = Convert.ToDouble(uniqueNode.InnerText);
                            break;
                        default:
                            Debug.Log("Error in Steps Manager Switch Statement. Value: " + uniqueNode.Name);
                            break;
                    }
                }
                actorSteps[n].steps.Add(new Step(tempOrder, tempAction, tempValue));
            }
        }
    }
}
