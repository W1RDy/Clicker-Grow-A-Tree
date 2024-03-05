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

    public void LoadData(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        var json = InteractorWithBrowser.LoadData();
        if (json != null && json != "")
        {
            JsonUtility.FromJsonOverwrite(json, _dataContainer);
            _dataContainer.IsDefaultData = false;
        }
        else _dataContainer.SetDefaultSettings(growSettings, coinsSpawnSettings);
    }

    public void SaveData()
    {
        var json = JsonConvert.SerializeObject(_dataContainer, Formatting.Indented,
    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
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

    public void OnApplicationQuit()
    {
        SaveDataOnQuit?.Invoke();
        SaveData();
    }
}
