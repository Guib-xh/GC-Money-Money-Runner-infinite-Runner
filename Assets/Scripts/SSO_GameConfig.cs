using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SSO_GameConfig", menuName="SSO/SSO_GameConfig")]
public class SSO_GameConfig: ScriptableObject
{
    //MAP
    public int lineCount = 5;
    public float lineWidthLv = 2f; //en m
    public float rowDepthR = 2f; //en m
    public int visibilityRows = 40; //nombre de rangé en visibilité : 20R par défaut
    
    //MOUVEMENT
    public float jumpHeight = 3f;
    public float lateralMoveDuration = 0.25f;
    public float jumpDuration = 1.2f;
    public float slideDuration = 1.2f;
    //taille de la réduction du joueur lorsqu'il slide
    public float slideHeightScale = 0.5f;
    
    //VITESSE
    //Valeur d'augmentation de Kv par étape
    public float kvGrowthPerStage = 0.4f;
    //Vitesse initiale
    public float v0 = 6f; //en m/s
    public float kvMax = 4f;
    
    //STAGE
    public float firstStageDuration = 150f; //en seconde
    public float minStageDurationSeconds = 120f; //en seconde
    
    //VIE
    public int startingHearts = 3;
    public float postCollisionInvulnerabilitySeconds = 1f;
    
    //SCORE
    public int scorePerMoney = 10;
    public int scorePerRow = 15;
    public int scorePerBoss = 2000;

}
