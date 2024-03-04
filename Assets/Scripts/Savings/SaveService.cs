using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using Newtonsoft.Json;
using System.Text;

public class SaveService : MonoBehaviour, IService
{
    [SerializeField] private DataContainer _dataContainer;

    public DataContainer DataContainer => _dataContainer;
    public event Action SaveDataOnQuit;

    public void LoadData(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        var json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath,"Data.json"));
        if (json != null && json != "")
        {
            JsonUtility.FromJsonOverwrite(json, _dataContainer);
        }
        else _dataContainer.SetDefaultSettings(growSettings, coinsSpawnSettings);
    }

    public void SaveData()
    {
        File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "Data.json"), JsonConvert.SerializeObject(_dataContainer, Formatting.Indented, 
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }),
            Encoding.UTF8);
        Debug.Log("SaveDataToJSON");
    }

    public void SaveBranchesInContainer(List<Branch> branches)
    {
        List<BranchSaveConfig> configs = new List<BranchSaveConfig>();
        foreach (Branch branch in branches)
        {
            configs.Add(new BranchSaveConfig(branch.Index, branch.transform.position, branch.transform.rotation));
        }

        _dataContainer.BranchConfigs = configs;
        Debug.Log("SaveBranches");
    }

    public void SaveGrowableValue(IGrowable growable)
    {
        if (growable is Branch branch)
        {
            foreach (var config in _dataContainer.BranchConfigs)
            {
                if (config.Index == branch.Index && config.Position == branch.transform.position)
                {
                    config.FillValue = branch.GetFilledTopLocalPoint().y / branch.GetMaxHeight();
                    break;
                }
            }
        }
        else
        {
            var value = growable.GetFilledTopLocalPoint().y / growable.GetMaxHeight();
            _dataContainer.TrunkConfig.FillValue = (float)Math.Round(value, 3);
        }
        Debug.Log("SaveGrowableValue");
    }

    public void SaveTrunk(TrunkPart trunk)
    {
        _dataContainer.TrunkConfig = new TrunkSaveConfig(trunk.transform.position, 0);
        Debug.Log("SaveTrunk");
    }

    public void SaveCoins(List<Coin> coins)
    {
        List<CoinsSaveConfig> configs = new List<CoinsSaveConfig>();

        foreach (var coin in coins)
        {
            configs.Add(new CoinsSaveConfig(coin.transform.position));
        }

        _dataContainer.CoinsSaveConfigs = configs;
        Debug.Log("SaveCoins");
    }

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

    public void OnApplicationQuit()
    {
        SaveDataOnQuit?.Invoke();
        SaveData();
    }
}
