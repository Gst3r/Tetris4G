using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class GoalsUI : MonoBehaviour
{

    [SerializeField] private Transform[] goalsLine;

    [SerializeField] private TMP_Text[] goalsText;

    [SerializeField] private Image[] goalsState;

    [SerializeField] private Sprite valideSprite;

    [SerializeField] private Sprite unvalideSprite;

    [SerializeField] private Sprite valideZone;

    // Start is called before the first frame update
    void Start()
    {
        InitializeElements();
        FillPanel();
    }

    public void InitializeElements(){
        goalsLine = new Transform[25];
        goalsText = new TMP_Text[25];
        goalsState = new Image[25];

        int j=0;
        foreach (Transform child in transform)
        {
            goalsLine[j] = child;
            j++;
        }
        
        for (int i=0; i<goalsLine.Length;i++){
            int k=0;
            Transform[] bufferedChild = new Transform[2];
            foreach (Transform child in goalsLine[i].transform){
                bufferedChild[k] = child;
                k++;
            }
            goalsText[i] = bufferedChild[0].GetComponent<TMP_Text>();
            goalsState[i] = bufferedChild[1].GetComponent<Image>();
        }

    }

    public void FillPanel(){
        var data = Resources.Load<TextAsset>($"goals");
        string jsonString = data.text;
        string jsonData = jsonString.Split('{')[1].Split('}')[0];
        string[] jsonDicoData = jsonData.Split(',');
        
        for(int i=0;i<jsonDicoData.Length;i++){
            string[] jsonLineDicoData = jsonDicoData[i].Split(':');

            goalsText[i].text = jsonLineDicoData[0].Split('\"')[1];

            if(jsonLineDicoData[1]=="true"){
                goalsState[i].sprite = valideSprite;
                goalsLine[i].GetComponent<Image>().sprite=valideZone;
                goalsLine[i].GetComponent<Image>().color= new Color(255,255,255,255);
            }else{
                goalsState[i].sprite = unvalideSprite;
            }
        }
    }
}
