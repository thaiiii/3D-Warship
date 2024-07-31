using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    [Header("Ships")]
    public GameObject[] ships;
    public EnemyScript enemyScript;
    private ShipScript shipScript;
    private List<int[]> enemyShips;
    private int shipIndex = 0;
    public List<TileScript> allTileScripts;    

    [Header("Audio")]
    public AudioSource audioSource_Result; 
    public AudioClip soundClip_Win;  
    public AudioClip soundClip_Lose; 
    public AudioSource audioSource_WaterSplash; // Kéo và thả AudioSource từ Inspector vào đây
    public AudioClip soundClip_WaterSplash; // Kéo và thả AudioClip từ thư viện của bạn vào đây 
    public AudioSource audioSource_Explosion; // Kéo và thả AudioSource từ Inspector vào đây
    public AudioClip soundClip_Explosion; // Kéo và thả AudioClip từ thư viện của bạn vào đây
    
    [Header("HUD")]
    public Button nextBtn;
    public Button rotateBtn;
    public Button backBtn;
    public Button backBtn1;
    public Button replayBtn;
    public Text topText;
    public Text result;
    public Text playerShipText;
    public Text enemyShipText;
    public Text playerText;
    public Text enemyText;
    public Toggle expertMode;

    [Header("Objects")]
    public GameObject missilePrefab;
    public GameObject enemyMissilePrefab;
    public GameObject firePrefab;
    public GameObject woodDock;
    public GameObject mainCamera;

    private bool setupComplete = false;
    private bool playerTurn = true;
    
    private List<GameObject> playerFires = new List<GameObject>();
    private List<GameObject> enemyFires = new List<GameObject>();
    
    private int enemyShipCount = 5;
    private int playerShipCount = 5;
    public bool expert = false;
    Color32 expertModeSkyColor = new Color32(137, 31, 48, 255);
    Color32 normalModeSkyColor = new Color32(48, 77, 137, 255);
//Audio 
     

    // Start is called before the first frame update
    void Start()
    {
        expertMode.onValueChanged.AddListener(delegate { ToggleValueChanged(); });

        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        nextBtn.onClick.AddListener(() => NextShipClicked());
        rotateBtn.onClick.AddListener(() => RotateClicked());
        enemyShips = enemyScript.PlaceEnemyShips();

    }

    private void NextShipClicked()
    { 
        if (!shipScript.OnGameBoard())
        {
            shipScript.FlashColor(Color.red);
        } else
        {
            if(shipIndex <= ships.Length - 2)
            {
                shipIndex++;
                shipScript = ships[shipIndex].GetComponent<ShipScript>();
                shipScript.FlashColor(Color.yellow);
            }
            else
            {
                rotateBtn.gameObject.SetActive(false);
                nextBtn.gameObject.SetActive(false);
                woodDock.SetActive(false);
                topText.text = "Guess an enemy tile.";
                setupComplete = true;
                expertMode.gameObject.SetActive(false);
                if(expert) {
                    enemyShipText.enabled = false;
                    enemyText.enabled = false;
                } 
                for (int i = 0; i < ships.Length; i++) ships[i].SetActive(false);
            }
        }
        
    }

    private void ToggleValueChanged()
    {
        expert = expertMode.isOn;
        Debug.Log("Giá trị boolean đã thay đổi thành: " + expert);
        if (expert) {
            Camera cameraComponent = mainCamera.GetComponent<Camera>();
            cameraComponent.backgroundColor = expertModeSkyColor;
        }
        else {
            Camera cameraComponent = mainCamera.GetComponent<Camera>();
            cameraComponent.backgroundColor = normalModeSkyColor;
        }
    }

    public void TileClicked(GameObject tile)
    {
        if(setupComplete && playerTurn)
        {            
            Vector3 tilePos = tile.transform.position;
            tilePos.y += 15;
            playerTurn = false;
            Instantiate(missilePrefab, tilePos, missilePrefab.transform.rotation);
        } else if (!setupComplete)
        {
            PlaceShip(tile);
            shipScript.SetClickedTile(tile);
        }
    }

    private void PlaceShip(GameObject tile)
    {
        shipScript = ships[shipIndex].GetComponent<ShipScript>();
        shipScript.ClearTileList();
        Vector3 newVec = shipScript.GetOffsetVec(tile.transform.position);
        ships[shipIndex].transform.localPosition = newVec;
    }

    void RotateClicked()
    {
        shipScript.RotateShip();
    }

    public void CheckHit(GameObject tile)
    {
        int tileNum = Int32.Parse(Regex.Match(tile.name, @"\d+").Value);
        int hitCount = 0;
        foreach(int[] tileNumArray in enemyShips)
        {
            if (tileNumArray.Contains(tileNum))
            {
                for (int i = 0; i < tileNumArray.Length; i++)
                {
                    if (tileNumArray[i] == tileNum)
                    {
                        tileNumArray[i] = -5;
                        hitCount++;
                    }
                    else if (tileNumArray[i] == -5)
                    {
                        hitCount++;
                    }
                }
                if (hitCount == tileNumArray.Length)
                {
                    if(!expert) {
                        enemyShipCount--;
                        audioSource_WaterSplash.PlayOneShot(soundClip_WaterSplash);
                        audioSource_Explosion.PlayOneShot(soundClip_Explosion);
                        topText.text = "SUNK!!!!!!";
                        enemyFires.Add(Instantiate(firePrefab, tile.transform.position, Quaternion.identity));
                        tile.GetComponent<TileScript>().SetTileColor(1, new Color32(68, 0, 0, 255));
                        tile.GetComponent<TileScript>().SwitchColors(1);
                    }
                    else {
                        enemyShipCount--;
                        audioSource_WaterSplash.PlayOneShot(soundClip_WaterSplash);
                        topText.text = "SHOT !!!";
                        enemyFires.Add(Instantiate(firePrefab, tile.transform.position, Quaternion.identity));
                        tile.GetComponent<TileScript>().SetTileColor(1, new Color32(38, 57, 76, 255));
                        tile.GetComponent<TileScript>().SwitchColors(1);
                    }
                }
                else
                {
                    if(!expert) {
                        audioSource_WaterSplash.PlayOneShot(soundClip_WaterSplash);
                        audioSource_Explosion.PlayOneShot(soundClip_Explosion);
                        topText.text = "HIT!!";
                        tile.GetComponent<TileScript>().SetTileColor(1, new Color32(255, 0, 0, 255));
                        tile.GetComponent<TileScript>().SwitchColors(1);
                    }
                    else {
                        audioSource_WaterSplash.PlayOneShot(soundClip_WaterSplash);
                        topText.text = "SHOT !!!";
                        tile.GetComponent<TileScript>().SetTileColor(1, new Color32(38, 57, 76, 255));
                        tile.GetComponent<TileScript>().SwitchColors(1);
                    }
                }
                break;
            }
            
        }
        if(hitCount == 0)
        {
            
                tile.GetComponent<TileScript>().SetTileColor(1, new Color32(38, 57, 76, 255));
                tile.GetComponent<TileScript>().SwitchColors(1);
                audioSource_WaterSplash.PlayOneShot(soundClip_WaterSplash);
                if(!expert) 
                    topText.text = "Missed, there is no ship there";
                else
                    topText.text = "SHOT !!!";
        }
        Invoke("EndPlayerTurn", 1.0f);
    }

    public void EnemyHitPlayer(Vector3 tile, int tileNum, GameObject hitObj)
    {
        enemyScript.MissileHit(tileNum);
        tile.y += 0.2f;
        playerFires.Add(Instantiate(firePrefab, tile, Quaternion.identity));
        if (hitObj.GetComponent<ShipScript>().HitCheckSank())
        {
            playerShipCount--;
            playerShipText.text = playerShipCount.ToString();
            enemyScript.SunkPlayer();
        }
       Invoke("EndEnemyTurn", 2.0f);
    }

    private void EndPlayerTurn()
    {
        for (int i = 0; i < ships.Length; i++) ships[i].SetActive(true);
        foreach (GameObject fire in playerFires) fire.SetActive(true);
        foreach (GameObject fire in enemyFires) fire.SetActive(false);
        enemyShipText.text = enemyShipCount.ToString();
        topText.text = "Enemy's turn";
        enemyScript.NPCTurn();
        ColorAllTiles(0);
        if (playerShipCount < 1)                                               //<<<<<<<<<<<<<< End
        {                                          
            GameOver("YOU LOSE !!!");
        }                 
    }

    public void EndEnemyTurn()
    {
        for (int i = 0; i < ships.Length; i++) ships[i].SetActive(false);
        foreach (GameObject fire in playerFires) fire.SetActive(false);
        foreach (GameObject fire in enemyFires) fire.SetActive(true);
        playerShipText.text = playerShipCount.ToString();
        topText.text = "Select a tile";
        playerTurn = true;
        ColorAllTiles(1);
        if (enemyShipCount < 1)                                                 //<<<<<<<<<<<<<< End
        {
            GameOver("YOU WIN !!!"); 
        }                      
        
    }

    private void ColorAllTiles(int colorIndex)
    {
        foreach (TileScript tileScript in allTileScripts)
        {
            tileScript.SwitchColors(colorIndex);
        }
    }

    void GameOver(string gameResult)
    {
        PostProcessVolume ppVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
        ppVolume.enabled = true; 
        result.enabled = true;
        topText.enabled = false;
        playerShipText.enabled = false;
        enemyShipText.enabled = false;
        playerText.enabled = false;
        enemyText.enabled = false;
        result.text = gameResult;
        backBtn.gameObject.SetActive(false);
        backBtn1.gameObject.SetActive(true);
        replayBtn.gameObject.SetActive(true);
        playerTurn = false;

        if(gameResult == "YOU WIN !!!") {
            audioSource_Result.Stop();
            audioSource_Result.clip = soundClip_Win;
            audioSource_Result.loop = false;
            audioSource_Result.Play();
        } 
        else {
            audioSource_Result.Stop();
            audioSource_Result.clip = soundClip_Lose;
            audioSource_Result.loop = false;
            audioSource_Result.Play();
        }
    }
}
