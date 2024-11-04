using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Net;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using UnityEngine.Playables;

public class ObsConnect : MonoBehaviour
{
    private string socketURL = "";
    [SerializeField] public string SocketURL = "localhost";
    [SerializeField] private string port = "4444";
    [SerializeField] private string pass = "123456";

    private OBSWebsocket obs = new OBSWebsocket();

    [SerializeField]
    PlayableDirector timeline;

    private void Awake()
    {
        if (timeline)
        {
            timeline.playOnAwake = false;
            timeline.extrapolationMode = DirectorWrapMode.None;
            timeline.Stop();
        }
    }

    private void Start()
    {
        if (timeline)
        {
            timeline.played += StartOBSRecording;
            timeline.stopped += StopOBSRecording;
            timeline.paused += PauseOBSRecording;
        }

        if (string.IsNullOrEmpty(SocketURL) == false)
        {
            SetScoketURL(SocketURL);
            ConnectOBS();
        }
    }

    private void OnDestroy()
    {
        DisConnectOBS();
    }

    [ContextMenu("Connect OBS")]
    public void ConnectOBS()
    {
        if (!obs.IsConnected || obs == null)
        {
            obs = new OBSWebsocket();

            obs.Connect(socketURL + port + "/", pass);
        }
    }

    [ContextMenu("DisConnect OBS")]
    public void DisConnectOBS()
    {
        if (obs.IsConnected)
        {
            obs.Disconnect();
            obs = null;
        }
    }

    public void StartOBSRecording()
    {
        Debug.Log("Rec Start");
        Debug.Log(obs.GetRecordingStatus().IsRecordingPaused);
        obs.StartRecording();
    }

    public void StartOBSRecording(PlayableDirector director)
    {
        if (obs.GetRecordingStatus().IsRecordingPaused)
        {
            obs.ResumeRecording();
        }
        else
        {
            StartOBSRecording();
        }
    }

    public void StopOBSRecording()
    {
        obs.StopRecording();
    }

    public void StopOBSRecording(PlayableDirector director)
    {
        StopOBSRecording();
    }

    public void PauseOBSRecording()
    {
        obs.PauseRecording();
    }

    public void PauseOBSRecording(PlayableDirector director)
    {
        PauseOBSRecording();
    }

    public string AddressUpdate(string str)
    {
        return "ws://" + str + ":";
    }

    public void SetScoketURL(string str)
    {
        socketURL = "ws://" + str + ":";
    }

    public void SetPort(string str)
    {
        port = str;
    }

    public void SetPass(string str)
    {
        pass = str;
    }
}
