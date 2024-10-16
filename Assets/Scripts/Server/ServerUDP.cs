using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ServerUDP : MonoBehaviour
{
    Socket socket;

    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string serverText;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();

    }
    public void startServer()
    {
        serverText = "Starting UDP Server...";

        IPEndPoint ipep = new(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);

        Thread newConnection = new(Receive);
        newConnection.Start();
    }

    void Update()
    {
        UItext.text = serverText;
    }

    void Receive()
    {
        int recv;
        byte[] data = new byte[1024];

        serverText = serverText + "\n" + "Waiting for new Client...";

        IPEndPoint sender = new(IPAddress.Any, 0);
        EndPoint Remote = sender;

        while (true)
        {
            recv = socket.ReceiveFrom(data, ref Remote);

            serverText = serverText + "\n" + "Message received from {0}:" + Remote.ToString();
            serverText = serverText + "\n" + Encoding.ASCII.GetString(data, 0, recv);

            Thread sendPing = new(() => Send(Remote));
            sendPing.Start();
        }
    }

    void Send(EndPoint Remote)
    {
        string Ping = "Ping";

        byte[] Ping_Encoded = Encoding.ASCII.GetBytes(Ping);

        socket.SendTo(Ping_Encoded, Remote);
    }


}
