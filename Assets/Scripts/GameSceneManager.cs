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
    [Header("Match Setup")]
    [SerializeField]
    private int startMoney;
    [SerializeField]
    private int matchDuration;
    [SerializeField]
    private int crimeLimit;
    #endregion
    [Header("Managers")]
    public SoundManager             soundManager;
    public UIManager                uiManager;
    public GridManager              gridManager;
    public BuildingManager          buildingManager;
    public UnitsManager             unitsManager;
    public CrimeManager             crimeManager;
    public MatchCountdownManager    matchCountdownManager;
    
    public Unit selectedUnit { get; private set; }
    public UnitType unitEditingType { get; private set; }

    public float gameTime = 0f;

    public GameObject   endScreenContanier;
    public GameObject   winContanier;
    public GameObject   lossContanier;


    #region Mono
    void Start ()
    {
        soundManager = SoundManager.GetInstance();
        soundManager.PlayBGM();
        Instance = this;
        Time.timeScale = 1f;
        gameState = GameState.PLAYING;
        Money = startMoney;

        gridManager.OnStreetTileClicked += StreetTileClicked;
        gridManager.OnBuildingTileClicked += BuildingTileClicked;
        gridManager.OnCrimeStarted += CrimeStarted;
        gridManager.OnTileHover += TileHover;

        buildingManager.OnBuildingHovered += BuildingHovered;

        unitsManager.OnUnitSelected += UnitSelected;
        unitsManager.OnUnitMoved += UnitMoved;

        crimeManager.OnCrimeEnded += CrimeEnded;

        uiManager.OnBuyButtonClicked += BuyButtonClicked;
        uiManager.OnSellButtonClicked += SellButtonClicked;
        uiManager.OnRotateButtonClicked += RotateButtonClicked;
        uiManager.OnPauseButtonClicked += PauseButtonClicked;
        uiManager.UpdateMoneyLabel();
        uiManager.UpdateCrimeLimitLabel(crimeLimit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("TitleScreen");

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

        if (gameTime > (matchDuration * 29f / 30f) - 1f)
            matchCountdownManager.StartCountdown((matchDuration * 1f / 30f));

        uiManager.UpdateTimeLabel(Mathf.FloorToInt(gameTime / 8f) + 1);
        if (gameTime > matchDuration)
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
                UnitBought(unitEditingType, unitEditingType == UnitType.POLICE_MAN ? 
                    SoundVolumes.sfxUnitBought_PoliceMan : SoundVolumes.sfxUnitBought_PoliceCar);
            }
            else if (unitEditingType == UnitType.POLICE_STATION)
            {
                unitsManager.SpawnUnit(unitEditingType, p_tile);
                UnitBought(unitEditingType, SoundVolumes.sfxUnitBought_PoliceStation);
            }
        }
    }
   
    private void StreetTileClicked(GridTile p_tile)
    {
        if (gameState == GameState.BUYING && unitEditingType == UnitType.POLICE_CAMERA)
        {
            unitsManager.SpawnUnit(UnitType.POLICE_CAMERA, p_tile);
            UnitBought(unitEditingType, SoundVolumes.sfxUnitBought_PoliceCamera);
        }
    }
    private void TileHover(GridTile p_tile, int p_hoverStatus)
    {
        if (gameState == GameState.BUYING)
        {
            if (unitEditingType == UnitType.POLICE_CAMERA && p_tile.tileType == GridTile.TileType.STREET)
                uiManager.unitPlacement.PlaceUnit(p_tile, unitEditingType);
            else if (unitEditingType != UnitType.POLICE_CAMERA && p_tile.tileType == GridTile.TileType.BUILDING)
                uiManager.unitPlacement.PlaceUnit(p_tile, unitEditingType);
            else
                uiManager.unitPlacement.ResetUnits();
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
    private void CrimeEnded()
    {
        uiManager.UpdateCrimeBars(crimeLimit ,crimeManager.notseenCrimes, 
            crimeManager.seenCrimes, crimeManager.stoppedCrimes);
    }
    #endregion
    #region UnitsActions
    private void UnitBought(UnitType p_type, float p_sfxVolume)
    {
        Money -= GameEconomy.GetUnitBuyPrice(p_type);
        uiManager.UpdateMoneyLabel();
        uiManager.unitPlacement.ResetUnits();
        UpdateMonitoredTiles = true;
        ReturnToNormalState();
        soundManager.PlaySFX(SFXType.UNIT_BOUGHT_POLICE_CAMERA + (int)p_type, p_sfxVolume);
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
        if (unitEditingType == UnitType.POLICE_CAMERA)
            gridManager.EnableTilePlacementIcons(false, TileType.STREET);
        else
            gridManager.EnableTilePlacementIcons(false, TileType.BUILDING);
    }
    public void ShowEndGameScreen(bool p_isWin)
    {
        if (p_isWin)
            soundManager.PlaySFX(SFXType.MATCH_WON, SoundVolumes.sfxUnitBought_MatchWon);
        else
            soundManager.PlaySFX(SFXType.MATCH_LOSS, SoundVolumes.sfxUnitBought_MatchLost);
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
        else if (p_unityTypeIndex == 5)
        {
            CancelButtonClicked();
            soundManager.PlaySFX(SFXType.ERROR, SoundVolumes.sfxError);
        }
        else if (GameEconomy.HaveEnoughMoney((UnitType)p_unityTypeIndex, Money))
        {
            soundManager.PlaySFX(SFXType.BUTTON_PRESS, SoundVolumes.sfxButtonPress);
            if (gameState == GameState.BUYING)
            {
                uiManager.unitPlacement.ResetUnits();
                if (unitEditingType == UnitType.POLICE_CAMERA)
                    gridManager.EnableTilePlacementIcons(false, TileType.STREET);
                else
                    gridManager.EnableTilePlacementIcons(false, TileType.BUILDING);
            }
            gameState = GameState.BUYING;
            unitEditingType = (UnitType)p_unityTypeIndex;
            uiManager.UpdateCancelButton(p_unityTypeIndex);
            if (unitEditingType == UnitType.POLICE_CAMERA)
                gridManager.EnableTilePlacementIcons(true, TileType.STREET);
            else
                gridManager.EnableTilePlacementIcons(true, TileType.BUILDING);
        }
        else
            soundManager.PlaySFX(SFXType.ERROR, SoundVolumes.sfxError);
    }
    private void SellButtonClicked()
    {
        if (gameState != GameState.EDITING)
            return;
        Money += GameEconomy.GetUnitSellPrice(selectedUnit.unitType);
        soundManager.PlaySFX(SFXType.UNIT_SOLD, SoundVolumes.sfxUnitSold);
        unitsManager.RemoveUnit(selectedUnit);
        uiManager.UpdateMoneyLabel();
        ReturnToNormalState();
    }
    public void RotateButtonClicked()
    {
        if (selectedUnit.unitType == UnitType.POLICE_CAMERA)
        {
            selectedUnit.RotateUnit();
            soundManager.PlaySFX(SFXType.UNIT_ROTATED, SoundVolumes.sfxUnitRotated);
        }
    }
    public void PauseButtonClicked()
    {
        soundManager.PlaySFX(SFXType.BUTTON_PRESS, SoundVolumes.sfxButtonPress);
        SceneManager.LoadScene("TitleScreen");
    }
    public void CancelButtonClicked()
    {
        uiManager.unitPlacement.ResetUnits();
        ReturnToNormalState();
    }
    public void PlayAgainButtonClicked()
    {
        soundManager.PlaySFX(SFXType.BUTTON_PRESS, SoundVolumes.sfxButtonPress);
        SceneManager.LoadScene("GameScene");
    }
    public void MenuButtonClicked()
    {
        soundManager.PlaySFX(SFXType.BUTTON_PRESS, SoundVolumes.sfxButtonPress);
        SceneManager.LoadScene("TitleScreen");
    }
    #endregion
}
