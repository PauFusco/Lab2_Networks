using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using TMPro;
using System.Text;

public class ServerTCP : MonoBehaviour
{
    Socket socket;

    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string serverText;

    public struct User
    {
        public string name;
        public Socket socket;
    }

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        UItext.text = serverText;
    }

    public void startServer()
    {
        serverText = "Starting TCP Server...";

        IPEndPoint ipep = new(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(ipep);

        socket.Listen(10);

        Thread mainThread = new(CheckNewConnections);
        mainThread.Start();
    }

    void CheckNewConnections()
    {
        while (true)
        {
            User newUser = new();
            newUser.name = "";
            newUser.socket = socket.Accept();

            IPEndPoint clientep = (IPEndPoint)socket.RemoteEndPoint;

            serverText += "\n" + "Connected with " + clientep.Address.ToString() + ":" + clientep.Port.ToString();

            Thread newConnection = new(() => Receive(newUser));
            newConnection.Start();
        }

    }

    void Receive(User user)
    {
        byte[] data = new byte[1024];
        int recv = 0;

        while (true)
        {
            data = new byte[1024];
            recv = user.socket.Receive(data);

            if (recv == 0)
                break;
            else
            {
                serverText = serverText + "\n" + Encoding.ASCII.GetString(data, 0, recv);
            }

            Thread answer = new(() => Send(user));
            answer.Start();
        }
    }

    void Send(User user)
    {
        byte[] data = Encoding.ASCII.GetBytes("Server Ping");
        user.socket.Send(data);
    }
}
