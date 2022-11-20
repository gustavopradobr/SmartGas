using System;
using UnityEngine;

public static class PlayerPrefsHelper
{
    public static bool GetBool(string key, bool defaultValue)
    {
        try
        {
            if (PlayerPrefs.HasKey(key))
            {
                return Convert.ToBoolean(PlayerPrefs.GetString(key));
            }
            else
            {
                PlayerPrefs.SetString(key, Convert.ToString(defaultValue));
                PlayerPrefs.Save();
                return defaultValue;
            }
        }
        catch
        {
            PlayerPrefs.SetString(key, Convert.ToString(defaultValue));
            PlayerPrefs.Save();
            return defaultValue;
        }
    }
    public static void SetBool(string key, bool desiredValue)
    {
        PlayerPrefs.SetString(key, Convert.ToString(desiredValue));
        PlayerPrefs.Save();
    }
    public static string GetString(string key, string defaultValue)
    {
        try
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }
            else
            {
                PlayerPrefs.SetString(key, defaultValue);
                PlayerPrefs.Save();
                return defaultValue;
            }
        }
        catch
        {
            PlayerPrefs.SetString(key, defaultValue);
            PlayerPrefs.Save();
            return defaultValue;
        }
    }

    public static int GetInt(string key, int defaultValue)
    {
        try
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                PlayerPrefs.SetInt(key, defaultValue);
                PlayerPrefs.Save();
                return defaultValue;
            }
        }
        catch
        {
            PlayerPrefs.SetInt(key, defaultValue);
            PlayerPrefs.Save();
            return defaultValue;
        }
    }

    public static float GetFloat(string key, float defaultValue)
    {
        try
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetFloat(key);
            }
            else
            {
                PlayerPrefs.SetFloat(key, defaultValue);
                PlayerPrefs.Save();
                return defaultValue;
            }
        }
        catch
        {
            PlayerPrefs.SetFloat(key, defaultValue);
            PlayerPrefs.Save();
            return defaultValue;
        }
    }
}
