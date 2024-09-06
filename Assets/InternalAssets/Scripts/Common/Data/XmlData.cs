using System.Xml;
using System;
using System.IO;
using UnityEngine;
using System.Text;

public class XmlData
{

    private readonly XmlDocument document;
    private readonly XmlElement element;

    public XmlData(XmlNode node)
    {
        document = node.OwnerDocument;
        element = (XmlElement)node;
    }

    public static XmlData Empty()
    {
        return LoadXml("<data></data>");
    }

    public static XmlData LoadXml(string xml)
    {
        XmlDocument document = new XmlDocument();
        document.LoadXml(xml);
        return new XmlData(document.DocumentElement);
    }

    public static XmlData LoadFromFile(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            try
            {
                return LoadXml(Crypt(File.ReadAllText(filePath)));
            }
            catch (Exception)
            {
                return Empty();
            }
        }
        else
        {
            return Empty();
        }
    }

    public int GetChildCount()
    {
        return element.ChildNodes.Count;
    }

    public XmlData FindOrCreate(string name, string defaultValue = "")
    {
        XmlNode node = element.SelectSingleNode(name);
        if (node == null)
        {
            return Create(name, defaultValue);
        }
        return new XmlData(node);
    }

    public XmlData Create(string name, string defaultValue = "")
    {
        XmlElement newElement = document.CreateElement(name);
        newElement.InnerXml = defaultValue;
        element.AppendChild(newElement);
        return new XmlData(newElement);
    }

    public XmlData[] GetArray()
    {
        XmlNodeList nodes = element.ChildNodes;
        XmlData[] data = new XmlData[nodes.Count];
        for (int i = 0; i < nodes.Count; i++)
        {
            data[i] = new XmlData(nodes[i]);
        }
        return data;
    }

    public string GetOuterXml()
    {
        return element.OuterXml;
    }

    public string GetInnerXml()
    {
        return element.InnerXml;
    }

    public void SetInnerXml(string value)
    {
        element.InnerXml = value;
    }

    public string GetString(string name, string defaultValue = "")
    {
        XmlData data = FindOrCreate(name, defaultValue);
        return data.GetInnerXml();
    }

    public void SetString(string name, string value)
    {
        XmlData data = FindOrCreate(name);
        data.SetInnerXml(value);
    }

    public int GetInt(string name, int defaultValue = 0)
    {
        XmlData data = FindOrCreate(name, null);
        string str = data.GetInnerXml();
        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out int value))
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }

    public void SetInt(string name, int value)
    {
        XmlData data = FindOrCreate(name);
        data.SetInnerXml(value.ToString());
    }

    public long GetLong(string name, long defaultValue = 0)
    {
        XmlData data = FindOrCreate(name, null);
        string str = data.GetInnerXml();
        if (!string.IsNullOrEmpty(str) && long.TryParse(str, out long value))
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }

    public void SetLong(string name, long value)
    {
        XmlData data = FindOrCreate(name);
        data.SetInnerXml(value.ToString());
    }

    public void Remove(string key)
    {
        XmlNode node = element.SelectSingleNode(key);
        if (node != null)
        {
            node.ParentNode.RemoveChild(node);
        }
    }

    public void Save(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string folderPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string content = Crypt(GetOuterXml());
        File.WriteAllText(filePath, content);
    }

    private static string Crypt(string text)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            stringBuilder.Append((char)(text[i] ^ 213));
        }
        return stringBuilder.ToString();
    }
}