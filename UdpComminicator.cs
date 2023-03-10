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
        try
        {
            string localIpString = "192.168.0.181";
            System.Net.IPAddress localAddress =
                System.Net.IPAddress.Parse(localIpString);

            writeMsg("info  listenIP=" + localIpString + " port=" + receivePort);
            System.Net.IPEndPoint localEP =
                new System.Net.IPEndPoint(localAddress, receivePort);
            udpClient = new System.Net.Sockets.UdpClient(localEP);
        }
        catch (System.Net.Sockets.SocketException e)
        {
            writeMsg("exception  " + e);
            Console.WriteLine("exception " + e);
            writeMsg("他で同一PGが立ち上がっている可能性があります。");
            return;
        }
        try
        {
            for (; ; )
            {
                System.Net.IPEndPoint remoteEP = null;
                byte[] data = udpClient.Receive(ref remoteEP);
                onReceive(data);
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
