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
        IPEndPoint ipep = new(IPAddress.Parse("192.168.1.131"), 9050);
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        server.Connect(ipep);

        Thread connect = new(Connect);
        connect.Start();
    }

    void Connect()
    {
        Thread sendThread = new(() => Send("New User"));
        sendThread.Start();

        Thread receiveThread = new(Receive);
        receiveThread.Start();
    }

    void Send(string toSend)
    {
        byte[] data = Encoding.ASCII.GetBytes(toSend);

        server.Send(data);
    }

    void Receive()
    {
        byte[] data = new byte[1024];

        server.Receive(data);
        int recv = data.Length;
        clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);
    }
}
