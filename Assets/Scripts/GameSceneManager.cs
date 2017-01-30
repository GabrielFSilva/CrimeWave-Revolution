using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    #region StaticRegion
    public static GameSceneManager Instance { get; private set; }
    public static GameState gameState { get; private set; }
    private static bool UpdateMonitoredTiles;
    public static void RequestUpdateMonitoredTiles()
    {
        UpdateMonitoredTiles = true;
    }
    public static int   Money { get; private set; }
    [SerializeField]
    private int startMoney;
    #endregion

    public UIManager uiManager;
    public GridManager gridManager;
    public BuildingManager buildingManager;
    public UnitsManager unitsManager;
    public CrimeManager crimeManager;
    
    public Unit selectedUnit { get; private set; }
    public UnitType unitEditingType { get; private set; }

    public float gameTime = 0f;

    public GameObject   endScreenContanier;
    public GameObject   winContanier;
    public GameObject   lossContanier;

    public int          crimeLimit;

    #region Mono
    void Start ()
    {
        Instance = this;
        Time.timeScale = 1f;
        gameState = GameState.PLAYING;
        Money = startMoney;
        gridManager.OnStreetTileClicked += StreetTileClicked;
        gridManager.OnBuildingTileClicked += BuildingTileClicked;
        gridManager.OnCrimeStarted += CrimeStarted;
        buildingManager.OnBuildingHovered += BuildingHovered;
        unitsManager.OnUnitSelected += UnitSelected;
        unitsManager.OnUnitMoved += UnitMoved;
        crimeManager.OnCrimeEnded += CrimeEnded;
        uiManager.OnBuyButtonClicked += BuyButtonClicked;
        uiManager.OnSellButtonClicked += SellButtonClicked;
        uiManager.UpdateCrimeLimitLabel(crimeLimit);
    }

  

    private void CrimeEnded()
    {
        uiManager.UpdateCrimeLabels(crimeManager.notseenCrimes, crimeManager.seenCrimes, crimeManager.stoppedCrimes);
    }

    private void Update()
    {
        if (gameState == GameState.END_GAME)
            Time.timeScale = 0f;
        else if (Input.GetKey(KeyCode.Space))
            Time.timeScale = 25f;
        else
            Time.timeScale = 1f;
        if (UpdateMonitoredTiles)
        {
            UpdateMonitoredTiles = false;
            gridManager.UpdateMonitoredTiles(unitsManager.units);
            crimeManager.CheckSeenCrimes();
            crimeManager.CheckStoppedCrimes();
        }

        if (gameState == GameState.END_GAME)
            return;
        gameTime += Time.deltaTime;
        uiManager.UpdateTimeLabel(Mathf.FloorToInt(gameTime / 8f) + 1);
        if (gameTime > 240f)
            ShowEndGameScreen(true);
        if (crimeManager.seenCrimes + crimeManager.notseenCrimes >= crimeLimit)
            ShowEndGameScreen(false);

    }
    #endregion
    #region TileActions
    private void BuildingTileClicked(GridTile p_tile)
    {
        if (gameState == GameState.BUYING)
        {
            if (unitEditingType == UnitType.POLICE_MAN || unitEditingType == UnitType.POLICE_CAR)
            {
                unitsManager.SpawnUnit(unitEditingType, p_tile, p_tile.linkedBuilding);
                UnitBought(unitEditingType);
            }
            else if (unitEditingType == UnitType.POLICE_STATION)
            {
                unitsManager.SpawnUnit(unitEditingType, p_tile);
                UnitBought(unitEditingType);
            }
        }
    }
   
    private void StreetTileClicked(GridTile p_tile)
    {
        if (gameState == GameState.BUYING && unitEditingType == UnitType.POLICE_CAMERA)
        {
            unitsManager.SpawnUnit(UnitType.POLICE_CAMERA, p_tile);
            UnitBought(unitEditingType);
        }
    }
    private void BuildingHovered(Building p_building, bool p_showRoute)
    {
        gridManager.ShowBuildingRoute(p_building.route, p_showRoute);
    }
    private void CrimeStarted(GridTile p_tile)
    {
        crimeManager.SpawnCrime(p_tile);
        crimeManager.CheckSeenCrimes();
        crimeManager.CheckStoppedCrimes();
    }
    #endregion
    #region UnitsActions
    private void UnitBought(UnitType p_type)
    {
        Money -= GameEconomy.GetUnitBuyPrice(UnitType.POLICE_STATION);
        uiManager.UpdateMoneyLabel();
        UpdateMonitoredTiles = true;
        ReturnToNormalState();
    }
    private void UnitMoved(Unit obj)
    {
        UpdateMonitoredTiles = true;
    }
    private void UnitSelected(Unit p_unit)
    {
        if (gameState == GameState.PLAYING)
        {
            uiManager.EnableNormalUI(false);
            selectedUnit = p_unit;
            unitEditingType = p_unit.unitType;
            gameState = GameState.EDITING;
            uiManager.UpdateSellButtonLabel(p_unit.unitType);
        }
    }
    #endregion
    #region StateSetup
    private void ReturnToNormalState()
    {
        gameState = GameState.PLAYING;
        uiManager.EnableNormalUI(true);
        uiManager.UpdateCancelButton(-1);
    }
    public void ShowEndGameScreen(bool p_isWin)
    {
        Time.timeScale = 0f;
        gameState = GameState.END_GAME;
        endScreenContanier.SetActive(true);
        winContanier.SetActive(p_isWin);
        lossContanier.SetActive(!p_isWin);
    }
    #endregion
    #region UI_Input
    private void BuyButtonClicked(int p_unityTypeIndex)
    {
        if (p_unityTypeIndex < 0)
        {
            gameState = GameState.PLAYING;
        }
        else if (GameEconomy.HaveEnoughMoney((UnitType)p_unityTypeIndex, Money))
        {
            gameState = GameState.BUYING;
            unitEditingType = (UnitType)p_unityTypeIndex;
            uiManager.UpdateCancelButton(p_unityTypeIndex);
        }
        //else
        //  ERROR SOUND
    }
    private void SellButtonClicked()
    {
        if (gameState != GameState.EDITING)
            return;
        Money += GameEconomy.GetUnitSellPrice(selectedUnit.unitType);
        unitsManager.RemoveUnit(selectedUnit);
        uiManager.UpdateMoneyLabel();
        ReturnToNormalState();
    }
    public void RotateButtonClicked()
    {
        if (selectedUnit.unitType == UnitType.POLICE_CAMERA)
            selectedUnit.RotateUnit();
    }
    public void CancelButtonClicked()
    {
        ReturnToNormalState();
    }

    
    public void PlayAgainButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void MenuButtonClicked()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    #endregion
    
}
