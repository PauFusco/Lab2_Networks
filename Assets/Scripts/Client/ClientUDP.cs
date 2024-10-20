﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientUDP : MonoBehaviour
{
    Socket socket;

    public GameObject UItextObj;
    TextMeshProUGUI UItext;

    public GameObject ChatPanelObj;
    TMP_InputField MessageInput;

    string clientText;

    IPEndPoint ServerEP;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        MessageInput = ChatPanelObj.GetComponent<TMP_InputField>();
    }

    public void StartClient()
    {
        
        ServerEP = new IPEndPoint(IPAddress.Parse(""/*PUT YOUR IP HERE*/), 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Connect(ServerEP);

        Thread Validate = new(ValidateConnection);
        Validate.Start();
    }

    void Update()
    {
        UItext.text = clientText;

        if (Input.GetKeyDown(KeyCode.Return) && ServerEP != null)
        {
            byte[] toSend = Encoding.ASCII.GetBytes(MessageInput.text);

            MessageInput.text = "";

            Thread sendMessageThrd = new(() => SendMessage(toSend));
            sendMessageThrd.Start();
        }
    }

    void ValidateConnection()
    {
        byte[] data = new byte[1024];
        string handshake = " entered the chat";

        data = Encoding.ASCII.GetBytes(handshake);

        socket.SendTo(data, ServerEP);

        Thread receive = new(Receive);
        receive.Start();
    }

    void Receive()
    {
        IPEndPoint sender = new(IPAddress.Any, 0);
        EndPoint Remote = sender;

        while (true)
        {
            byte[] data = new byte[1024];
            int recv = socket.ReceiveFrom(data, ref Remote);

            clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);
        }
    }

    void SendMessage(byte[] toSend)
    {
        socket.SendTo(toSend, ServerEP);
    }
}
