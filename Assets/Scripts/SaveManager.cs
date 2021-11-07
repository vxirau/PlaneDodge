using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();

        if (state.usingAccelerometer && !SystemInfo.supportsAccelerometer)
        {
            state.usingAccelerometer = false;
            Save();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString("save",Helper.Serialize<SaveState>(state));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
        }
    }

    public bool IsColorOwned(int index)
    {
        return (state.colorOwned & (1 << index)) != 0;
    }

    public bool IsTrailOwned(int index)
    {
        return (state.trailOwned & (1 << index)) != 0;
    }

    public bool BuyColor(int index, int cost)
    {
        if (state.gold >= cost)
        {
            state.gold -= cost;
            UnlockColor(index);

            Save();

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool BuyTrail(int index, int cost)
    {
        if (state.gold >= cost)
        {
            state.gold -= cost;
            UnlockTrail(index);

            Save();

            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnlockColor(int index)
    {
        state.colorOwned |= 1 << index;
    }

    public void UnlockTrail(int index)
    {
        state.trailOwned |= 1 << index;
    }

    public void CompleteLevel(int index)
    {
        if (state.completedLevel == index)
        {
            state.completedLevel++;
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}
