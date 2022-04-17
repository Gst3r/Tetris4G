using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

/// <summary>
/// Auteur : Sterlingot Guillaume<br>
/// Description : Cette classe permet la gestion des objectifs du jeu
/// </summary>
public class GoalsManager : MonoBehaviour
{

    /// <summary>
    /// Chaine de caractère contenant l'ensemble des données sous forme de dictionnaire présent dans le fichier goals.json  
    /// </summary>
    private string[] jsonDicoData;

    /// <summary>
    /// Tableau de chaîne de caractère permettant de stocker l'ensemble des intitulés des objectifs
    /// </summary>
    public string[] goals; 

    /// <summary>
    /// Dictionnaire permettant d'associé un intitulé d'objectif à un statut qui est TRUE si accomplie, FALSE sinon
    /// </summary>
    private Dictionary<string,bool> successDico;

    private void Start(){
        goals = new string[22];
        successDico = new Dictionary<string,bool>();
        FillGoals(); //Remplir le tableau des objectifs non remplit et le dictionnaire avec le fichier json si il existe, manuellement sinon 
    }

    private void Update(){
        GoalsController();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode condense toute les méthodes qui permettent de controller les objectifs du jeu<br>
    /// (stockage des objectifs, affichage dynamique et statique des objectifs, verification de chaque objectif)
    /// </summary>
    public void GoalsController(){
        string intitule;

        //On verifie si un des objectifs est remplit et si c'est le cas, on remplit l'intitule avec l'intitule qui a ete complete
        if(CheckGoals(out intitule)){
            DynamiqueDisplayGoal(intitule); // Permet d'afficher l'objectif accomplit en cours de jeu
            GoalIsComplete(intitule); // Permet de changer le statut de l'objectif pour qu'il apparaisse dans le menu des objectifs
            RegisterGoal(intitule); //Permet d'enregistrer le statut de l'objectif remplit dans un fichier json pour conserver son accomplissement
        }
    }

    //CREER UNE METHODE QUI AFFICHE LE DICTIONNAIRE DANS L'ELEMENT D'INTERFACE INITIALISE PLUS HAUT

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de remplir le tableau contenant les intitulés des objectifs à accomplir en jeu ainsi que le dictionnaire d'état des objectifs
    /// </summary>
    public void FillGoals(){
        string path = Application.persistentDataPath + "/goals.json"; //Récupération du fichier json
        string jsonString = File.ReadAllText(path);//Lecture du fichier json
        string jsonData = jsonString.Split('{')[1].Split('}')[0];//Récupération des lignes du dictionnaire dans un string
        this.jsonDicoData = jsonData.Split(',');//Récupération ligne par ligne des objectifs dans un dictionnaire
         

        //Remplissage du dictionnaire d'objectif présent dans le script et du tableau de nom d'objectif
        for(int i=0; i<jsonDicoData.Length-1; i++){
            string[] jsonLineDicoData = jsonDicoData[i].Split(':');
            successDico.Add(jsonLineDicoData[0], (jsonLineDicoData[1]=="true")?true:false);
            goals[i] = jsonLineDicoData[0];
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de changer le statut d'un objectif de "non-accomplit" à "accomplit"
    /// </summary>
    /// <param name="intitule">
    /// Chaîne de caractère qui représente l'intitulé de l'objectif à changer
    /// </param>
    public void GoalIsComplete(string intitule){
        successDico[intitule]= true;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet d'afficher un objectif à l'écran
    /// </summary>
    /// <param name="intitule">
    /// Chaîne de caractère qui représente l'intitulé de l'objectif à changer
    /// </param>
    public void DynamiqueDisplayGoal(string intitule){
        //SUCCESS DICO ENTRE EN SCENE
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet d'enregistrer dans un fichier json le nouveau statut de l'objectif 
    /// </summary>
    /// <param name="intitule">
    /// Chaîne de caractère qui représente l'intitulé de l'objectif à changer
    /// </param>
    public void RegisterGoal(string intitule){        
        // ATTENTION ICI LA TABULATION ET LE RETOUR A LA LIGNE DES FICHIERS EST LUE !!!!!
        // Réecriture du fichier json à partir de l'ancien mais en prenant en compte le changement effectué
        string contents = "{";
        for(int i=0; i<jsonDicoData.Length; i++){
            string[] jsonLineDicoData = jsonDicoData[i].Split(':');
            if(jsonLineDicoData[0]==intitule)
                contents += jsonLineDicoData[0]+":true";
            else
                contents += jsonDicoData[i];
            if(i!=jsonDicoData.Length-1)
                contents += ",";
        }

        contents += "}";

        string path = Application.persistentDataPath + "/goals.json";
        File.WriteAllText(path, contents);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si un objectifs a été atteind
    /// </summary>
    /// <param name="intitule">
    /// une chaîne de caractère correspondant à l'intitule de l'objectif atteind
    /// </param>
    /// <return>
    /// Un booléen qui indique TRUE si un objectif est remplis, FALSE sinon
    /// </return>
    public bool CheckGoals(out string intitule){
        string goalIntitule = "";

        if(CheckFirstGame(out goalIntitule) || CheckMarathonGoal(out goalIntitule) || CheckSprintGoal(out goalIntitule) || CheckUltraGoal(out goalIntitule) || CheckFullGoal(out goalIntitule)){
            intitule = goalIntitule;
            return true;
        }else{
            intitule = "";
            return false;
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si un des objectifs de première partie de jeu est atteind
    /// </summary>
    /// <param name="intitule">
    /// une chaîne de caractère correspondant à l'intitule de l'objectif atteind
    /// </param>
    /// <return>
    /// Un booléen qui indique TRUE si un des objectifs de première partie de jeu est atteind, FALSE sinon
    /// </return>
    public bool CheckFirstGame(out string intitule){
        
        if(Controller.GetGameMode()==Mode.MARATHON && SceneManager.GetActiveScene().name=="Tutorial"  && successDico[goals[0]] == false){
            intitule=goals[0];
            return true;
        }else if(Controller.GetGameMode()==Mode.MARATHON && SceneManager.GetActiveScene().name=="Game" && successDico[goals[1]] == false){
            intitule=goals[1];
            return true;
        }else if(Controller.GetGameMode()==Mode.SPRINT && successDico[goals[2]] == false){
            intitule=goals[2];
            return true;
        }else if(Controller.GetGameMode()==Mode.ULTRA && successDico[goals[3]] == false){
            intitule=goals[3];
            return true;
        }
        
        intitule="";
        return false;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si un des objectifs du mode marathon a été atteind
    /// </summary>
    /// <param name="intitule">
    /// une chaîne de caractère correspondant à l'intitule de l'objectif atteind
    /// </param>
    /// <return>
    /// Un booléen qui indique TRUE si tout les objectifs du mode Marathon sont remplis, FALSE sinon
    /// </return>
    public bool CheckMarathonGoal(out string intitule){
        string intituleMarathonGoal;

        if(CheckScoreGoal(out intituleMarathonGoal)){
            intitule = intituleMarathonGoal;
            return true;
        }else{ 
            intitule = "";
            return false;
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si un des objectifs du mode sprint a été atteind
    /// </summary>
    /// <param name="intitule">
    /// une chaîne de caractère correspondant à l'intitule de l'objectif atteind
    /// </param>
    /// <return>
    /// Un booléen qui indique TRUE si tout les objectifs du mode Sprint sont remplis, FALSE sinon
    /// </return>
    public bool CheckSprintGoal(out string intitule){
        string intituleSprintGoal = "";

        if(CheckTimeGoal(out intituleSprintGoal)){
            intitule = intituleSprintGoal;
            return true;
        }else{
            intitule = "";
            return false;
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si un des objectifs du mode ultra a été atteind
    /// </summary>
    /// <param name="intitule">
    /// une chaîne de caractère correspondant à l'intitule de l'objectif atteind
    /// </param>
    /// <return>
    /// Un booléen qui indique TRUE si tout les objectifs du mode Ultra sont remplis, FALSE sinon
    /// </return>
    public bool CheckUltraGoal(out string intitule){
        string intituleUltraGoal = "";
        
        if(CheckLineGoal(out intituleUltraGoal)){
            intitule = intituleUltraGoal;
            return true;
        }else{ 
            intitule = "";
            return false;
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si tout les objectifs ont été atteind.
    /// </summary>
    /// <return>
    /// Un booléen qui indique TRUE si tout les objectifs du jeu sont remplis, FALSE sinon
    /// </return>
    public bool CheckFullGoal(out string intitule){
        foreach(bool state in successDico.Values){
            if(!state && successDico[goals[21]] == false){    
                intitule = goals[21];
                return false;
            }
        }
        intitule = "";
        return true;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si un des objectifs de score du mode marathon a été atteind
    /// </summary>
    /// <param name="intitule">
    /// une chaîne de caractère correspondant à l'intitule de l'objectif atteind
    /// </param>
    /// <return>
    /// Un booléen qui indique TRUE si tout les objectifs de score du mode Marathon sont remplis, FALSE sinon
    /// </return>
    public bool CheckScoreGoal(out string intitule){
        int[] goalScores = {0, 0, 0, 0, 40, 120, 200, 360, 520, 760, 1000, 1200};

        //On parcourt les intitulés de goals
        for(int i=4;i<12;i++){
            //La première condition permet de vérifier si le score visé est atteind ou non
            //La deuxième condition permet de vérifier si l'objectif de scorer précédent a été atteind avant de valider l'objectif (synchro)
            if(ScoreManager.GetScore()>=goalScores[i] && (i==4 || successDico[goals[i-1]] == true) && successDico[goals[i]] == false){
                intitule=goals[i];
                return true;
            }
        }
        intitule = "";
        return false;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si un des objectifs de temps du mode sprint a été atteind
    /// </summary>
    /// <param name="intitule">
    /// une chaîne de caractère correspondant à l'intitule de l'objectif atteind
    /// </param>
    /// <return>
    /// Un booléen qui indique TRUE si tout les objectifs de temps du mode sprint sont remplis, FALSE sinon
    /// </return>
    public bool CheckTimeGoal(out string intitule){
        int[] goalTime = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 6, 8, 10};
        int[] goalLines = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 10, 15, 20};

        //On parcourt les intitulés de goals
        for(int i=19;i<22;i++){
            //La première condition permet de vérifier si le score visé est atteind ou non
            //La deuxième condition permet de vérifier si l'objectif de scorer précédent a été atteind avant de valider l'objectif (synchro)
            if(ScoreManager.GetNbLines()>=goalLines[i] && ScoreManager.GetTime()<=goalTime[i] && (i==19 || successDico[goals[i-1]] == true) && successDico[goals[i]] == false){
                intitule=goals[i];
                return true;
            }
        }
        intitule="";
        return false;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si un des objectifs de ligne du mode sprint a été atteind
    /// </summary>
    /// <param name="intitule">
    /// une chaîne de caractère correspondant à l'intitule de l'objectif atteind
    /// </param>
    /// <return>
    /// Un booléen qui indique TRUE si tout les objectifs de ligne du mode sprint sont remplis, FALSE sinon
    /// </return>
    public bool CheckLineGoal(out string intitule){
         int[] goalLines = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 5, 8, 13, 18, 23};

        //On parcourt les intitulés de goals
        for(int i=12;i<19;i++){
            //La première condition permet de vérifier si le nombre de ligne visé est atteind ou non
            //La deuxième condition permet de vérifier si l'objectif de scorer précédent a été atteind avant de valider l'objectif (synchro)
            if(ScoreManager.GetNbLines()>=goalLines[i] && (i==12 || successDico[goals[i-1]] == true) && successDico[goals[i]] == false){
                intitule=goals[i];
                return true;
            }
        }
        intitule="";
        return false;
    }
}
