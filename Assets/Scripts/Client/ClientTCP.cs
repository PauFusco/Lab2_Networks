using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;

public class ClientTCP : MonoBehaviour
{
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string clientText;
    Socket server;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        UItext.text = clientText;

    }

    public void StartClient()
    {
        Thread connect = new Thread(Connect);
        connect.Start();
    }
    void Connect()
    {
        IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("192.168.1.131"), 9050);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        server.Connect(ipep);

        Thread sendThread = new Thread(Send);
        sendThread.Start();

        Thread receiveThread = new Thread(Receive);
        receiveThread.Start();

    }
    void Send()
    {
        byte[] data = Encoding.ASCII.GetBytes("Client Ping");
        server.Send(data);
    }

    void Receive()
    {
        byte[] data = new byte[1024];
        int recv = 0;

        server.Receive(data);
        recv = data.Length;
        clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);
    }

}
