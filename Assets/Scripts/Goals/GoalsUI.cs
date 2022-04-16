using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class GoalsUI : MonoBehaviour
{

    /// <summary>
    /// Tableau de panel représentant les fils du Container qui contient le script GoalsUI
    /// </summary>
    [SerializeField] private GameObject goalsPanel;

    /// <summary>
    /// Tableau de panel représentant les fils du Container qui contient le script GoalsUI
    /// </summary>
    [SerializeField] private Transform[] goalsLine;

    /// <summary>
    /// Tableau des intitulés des objectifs dans le cadre de l'affichage dans un panel
    /// </summary>
    [SerializeField] private TMP_Text[] goalsText;

    /// <summary>
    /// Tableau d'image contenant l'ensemble des images de l'affichage des objectifs dans un panel 
    /// </summary>
    [SerializeField] private Image[] goalsState;

    /// <summary>
    /// Sprite qui représente un rond vert signe que l'objectif est validé 
    /// </summary>
    [SerializeField] private Sprite valideSprite;

    /// <summary>
    /// sprite qui représente un rond rouge signe que l'objectif n'est pas validé
    /// </summary>
    [SerializeField] private Sprite unvalideSprite;

    /// <summary>
    /// Sprite qui représente le contour validé d'un objectif remplit dans l'interface
    /// </summary>
    [SerializeField] private Sprite valideZone;

    // Start is called before the first frame update
    public void Start()
    {
        CreateJsonFile();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de créer un fichier json dans les données du téléphone si il n'existe pas déjà<br>
    /// </summary>
    public void CreateJsonFile(){
        string path = Application.persistentDataPath + "/goals.json";
        if(!File.Exists(path)){
            var data = Resources.Load<TextAsset>($"goals");
            string jsonContents = data.text;
            File.WriteAllText(path, jsonContents);
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant l'initialisation des attributs de la classe qui sont les GameObjects manipulés par la classe
    /// </summary>
    public void InitializeElements(){
        goalsLine = new Transform[23];
        goalsText = new TMP_Text[23];
        goalsState = new Image[23];

        int j=0;
        foreach (Transform child in goalsPanel.GetComponent<Transform>())
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

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant de remplir le panneau d'affichage contenant les intitulés et les états des objectifs 
    /// </summary>
    public void FillPanel(){
        string path = Application.persistentDataPath + "/goals.json";
        string jsonString = File.ReadAllText(path);
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
