using UnityEngine;

[CreateAssetMenu(fileName = "SSO_GameConfig", menuName="SSO/SSO_GameConfig")]
public class SSO_GameConfig: ScriptableObject
{
    //MAP
    public int lineCount = 5;
    public float lineWidthLv = 2f; //en m
    public float RowDepthR = 2f; //en m
    public int VisibilityRows = 20; //nombre de rangé en visibilité : 20R par défaut
    
    //MOUVEMENT
    public float jumpHeight = 3f;
    public float jumpDuration = 1.2f;
    public float slideDuration = 1.2f;
    //taille de la réduction du joueur lorsqu'il slide
    public float slideHeightScale = 0.5f;
    
    //VITESSE
    //Valeur d'augmentation de Kv par étape
    public float KvGrowthPerStage = 0.1f;
    //Vitesse initiale
    public float v0 = 6f; //en m/s
    
    //STAGE
    public float firstStageDuration = 150f; //en seconde
    public float MinStageDurationSeconds = 120f; //en seconde
}
