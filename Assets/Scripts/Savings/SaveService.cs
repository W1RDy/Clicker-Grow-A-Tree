using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Unity.VisualScripting;
using UnityEngine.Networking;

public class SaveService : MonoBehaviour, IService
{
    [SerializeField] private DataContainer _dataContainer;

    public DataContainer DataContainer => _dataContainer;
    public event Action SaveDataOnQuit;
    public event Action QuitApplication;
    private string _json;
    private bool _dataSetted;
    public bool IsLoaded { get; private set; }

    public void LoadData(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        Debug.Log("Bind");
        StartCoroutine(WaitWhileInit(growSettings, coinsSpawnSettings));
    }

    public void SetData(string json)
    {
        _json = json;
        _dataSetted = true;
    }

    private void LoadDataAfterWaiting(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        Debug.Log(_json);
        if (_json != null && _json != "")
        {
            JsonUtility.FromJsonOverwrite(_json, _dataContainer);
            //_dataContainer = JsonUtility.FromJson<DataContainer>(_json);
            _dataContainer.IsDefaultData = false;
        }
        else _dataContainer.SetDefaultSettings(growSettings, coinsSpawnSettings);
        IsLoaded = true;
        Debug.Log("DataIsLoaded");
    }

    private IEnumerator WaitWhileInit(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        yield return new WaitUntil(() => InteractorWithBrowser.PlayerIsInitialized());
        InteractorWithBrowser.LoadData();
        yield return new WaitUntil(() => _dataSetted);
        Debug.Log("EndWaiting");
        LoadDataAfterWaiting(growSettings, coinsSpawnSettings);
    }

    public void SaveData()
    {
        var json = JsonConvert.SerializeObject(_dataContainer, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        //var json = JsonUtility.ToJson(_dataContainer);
        InteractorWithBrowser.SaveData(json);

        Debug.Log("SaveDataToJSON");
    }

    public void SaveBranchesInContainer(List<Branch> branches)
    {
        List<BranchSaveConfig> configs = new List<BranchSaveConfig>();
        foreach (Branch branch in branches)
        {
            if (branch.GetRelativeGrowable() == null) continue;
            var angle = branch.transform.localRotation.eulerAngles.z;
            angle = angle > 180 ? Mathf.Abs(angle - 360) : angle;

            var relativeIndex = branch.GetRelativeGrowable().GetIndex();

            var config = new BranchSaveConfig(branch.Index, branch.transform.localPosition, angle, branch.transform.localPosition.x > 0, branch.BranchLevel, relativeIndex);
            config.FillValue = branch.GetFilledTopLocalPoint().y / branch.GetMaxHeight();
            configs.Add(config);
        }

        _dataContainer.BranchConfigs = configs;
        Debug.Log("SaveBranches");
    }

    public void SaveTrunk(List<TrunkPart> trunks)
    {
        var result = new List<TrunkSaveConfig>();
        foreach(var trunk in  trunks)
        {
            Debug.Log(trunk.GetFilledTopLocalPoint().y / trunk.GetMaxHeight());
            result.Add(new TrunkSaveConfig(trunk.transform.position, trunk.GetFilledTopLocalPoint().y / trunk.GetMaxHeight(), trunk.Index));
        }

        _dataContainer.TrunkSaveConfig = result;
        Debug.Log("SaveTrunk");
    }

    //public void SaveCoins(List<Coin> coins)
    //{
    //    List<CoinsSaveConfig> configs = new List<CoinsSaveConfig>();

    //    foreach (var coin in coins)
    //    {
    //        configs.Add(new CoinsSaveConfig(coin.transform.position));
    //    }

    //    _dataContainer.CoinsSaveConfigs = configs;
    //    Debug.Log("SaveCoins");
    //}

    public void SaveGrowSettings(GrowSettings growSettings)
    {
        _dataContainer.GrowConfig = new GrowSaveConfig(growSettings);
        Debug.Log("SaveGrowSettings");
    }

    public void SaveCoinsSettings(CoinsSpawnSettings coinsSpawnSettings)
    {
        _dataContainer.CoinsSpawnConfig = new CoinsSaveSettingsConfig(coinsSpawnSettings);
        Debug.Log("SaveCoinsSettings");
    }

    public void SaveHeight(float height)
    {
        _dataContainer.Height = height;
        Debug.Log("SaveHeight");
    }

    public void SaveScore(int score)
    {
        _dataContainer.Score = score;
        Debug.Log("SaveScore");
    }

    public void SaveCoins(int coins)
    {
        _dataContainer.Coins = coins;
        Debug.Log("SaveCoins");
    }

    public void SaveCameraPos(Vector3 pos)
    {
        _dataContainer.CameraPos = pos;
        Debug.Log("SaveCameraPos");
    }

    public void SaveUpgradePaths(List<UpgradePathSaveConfig> saveConfigs)
    {
        _dataContainer.UpgradePathSaveConfigs = saveConfigs;
        Debug.Log("SaveUpgradePaths");
    }

    public void SaveAllData()
    {
        SaveDataOnQuit?.Invoke();
        SaveData();
    }

    public void OnDestroy()
    {
        QuitApplication?.Invoke();
    }
}
