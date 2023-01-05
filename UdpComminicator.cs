/// <summary>
/// Summary description for Class1
/// </summary>
/// 
public class UdpCommunicator
{
    private int receivePort = 2237;
    Thread thread = null;

    public delegate void OnReceiveDelegater(byte[] args);
    public OnReceiveDelegater onReceive;
    public delegate void OnExceptionReceiveDelegater();
    public OnExceptionReceiveDelegater toStop;
    public delegate void WriteMsgDelegater(String msg);
    public WriteMsgDelegater writeMsg;
    private System.Net.Sockets.UdpClient udpClient;

    public UdpCommunicator()
    {
    }

    public void start()
    {
        thread = new Thread(new ThreadStart(doExec));
        thread.Start();
    }

    private void doExec()
    {
        string localIpString = "192.168.0.181";
        System.Net.IPAddress localAddress =
            System.Net.IPAddress.Parse(localIpString);

        writeMsg("info  listenIP=" + localIpString + " port=" + receivePort);
        System.Net.IPEndPoint localEP =
            new System.Net.IPEndPoint(localAddress, receivePort);
        udpClient = new System.Net.Sockets.UdpClient(localEP);

        try
        {
            for (; ; )
            {
                System.Net.IPEndPoint remoteEP = null;
                byte[] rcvBytes = udpClient.Receive(ref remoteEP);
                writeMsg("receive " + rcvBytes.Length);
                onReceive(rcvBytes);
            }
        }
        catch (System.Net.Sockets.SocketException e)
        {
            Console.WriteLine("exception " + e);
        }
        catch (Exception e)
        {
            writeMsg("exception  " + e);
            Console.WriteLine("exception " + e);
            toStop();
        }
    }

    public void stop()
    {
        Console.WriteLine("toStop");
        if (thread != null)
        {
            thread.Interrupt();
        }
        if (udpClient != null)
        {
            udpClient.Close();
        }
        Console.WriteLine("終了しました。");

    }
}
