public class SavesManager : Singleton<SavesManager>
{
    private XmlData data;


    private const string fileName = "player.sav";

    protected override void OnIntialized()
    {
        LoadData();
    }

    private void LoadData()
    {
        data = XmlData.LoadFromFile(fileName);
    }

    public XmlData FindOrCreate(string key)
    {
        return data.FindOrCreate(key);
    }

    public string GetString(string key, string defaultValue = "") 
    {
        return data.GetString(key, defaultValue);
    }

    public void SetString(string key, string value)
    {
        data.SetString(key, value);
        Save();
    }

    public int GetInt(string key, int defaultValue = 0)
    {
        return data.GetInt(key, defaultValue);
    }

    public void SetInt(string key, int value)
    {
        data.SetInt(key, value);
        Save();
    }

    public long GetLong(string key, long defaultValue = 0)
    {
        return data.GetLong(key, defaultValue);
    }

    public void SetLong(string key, long value)
    {
        data.SetLong(key, value);
        Save();
    }

    public void Delete(string key)
    {
        data.Remove(key);    
    }

    public void Save()
    {
        data.Save(fileName);
    }
}