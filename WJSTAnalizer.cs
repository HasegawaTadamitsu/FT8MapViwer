
public partial class WJSTAnalizer
{

    private byte[] data;
    public WJSTAnalizer(byte[] arg)
    {
        data = arg;
    }

    public int getHeader()
    {
        return data[(4 * 2) + 3];
    }
    public string dump()
    {
        String ret = "";
        for (int i = 0; i < data.Length; i++)
        {
            ret += data[i] + "\r\n";
        }

        return BitConverter.ToString(data) + "x" + (uint)((data[(4 * 10) + 2]) * 1);
    }

    public int getDT()
    {
        int val = (int)((
              (uint)((255 - data[(4 * 7) - 1]) * 16777216) +
              (uint)((255 - data[(4 * 7) + 0]) * 65536) +
              (uint)((255 - data[(4 * 7) + 1]) * 256) +
              (uint)((255 - data[(4 * 7) + 2]) * 1)) * -1 - 1);

        return val;

        // 1,2:dump:AD-BC-CB-DA-00-00-00-02
        // 3,4     -00-00-00-02-00-00-00-06
        // 5,6     -57-53-4A-54-2D-58-01-01
        // 7,8     -7D-8B-C8-FF-FF-FF-F3-3F
        // 8,9     -D3-33-33-40-00-00-00-00
        // 10,11     -00-05-61-00-00-00-01-7E
        // 12,13     -00-00-00-11-4A-41-30-45
        //      -56-49-20-4A-4B-31-4F-54
        //      -50-20-2B-30-31-00-00
    }
    public int getFreq()
    {
        int val = (int)(
      (uint)((data[(4 * 10) - 1]) * 16777216) +
      (uint)((data[(4 * 10) + 0]) * 65536) +
      (uint)((data[(4 * 10) + 1]) * 256) +
      (uint)((data[(4 * 10) + 2]) * 1));
        return val;
    }
    public String getMsg()
    {
        int size = (int)(
            (uint)((data[(4 * 12) + 2]) * 256) +
                (uint)((data[(4 * 12) + 3]) * 1));
        byte[] str_b = new byte[size];
        Array.Copy(data, (4 * 12 + 4), str_b, 0, size);
        return System.Text.Encoding.UTF8.GetString(str_b);

    }
}

