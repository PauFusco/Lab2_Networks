using System.Net;
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
    TMP_InputField ChatPanel;

    string clientText;

    IPEndPoint ServerEP;

    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        ChatPanel = ChatPanelObj.GetComponent<TMP_InputField>();
    }

    public void StartClient()
    {
        Thread mainThread = new(Send);
        mainThread.Start();
    }

    void Update()
    {
        UItext.text = clientText;

        if (Input.GetKeyDown(KeyCode.Return) && ServerEP != null)
        {
            byte[] toSend = Encoding.ASCII.GetBytes(ChatPanel.text);
            clientText += "\nyou: " + ChatPanel.text;

            ChatPanel.text = "";

            Thread sendMessageThrd = new(() => SendMessage(toSend));
            sendMessageThrd.Start();
        }
    }

    void Send()
    {
        ServerEP = new IPEndPoint(IPAddress.Parse("192.168.1.131"), 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Connect(ServerEP);

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

        byte[] data = new byte[1024];
        int recv = socket.ReceiveFrom(data, ref Remote);

        clientText = ("Message received from {0}: " + Remote.ToString());
        clientText = clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);  
    }

    void SendMessage(byte[] toSend)
    {
        socket.SendTo(toSend, ServerEP);
    }
}
