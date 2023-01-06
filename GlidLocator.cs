using System.Text.RegularExpressions;

public class GridLocator
{
    private String gl;
    private double x;
    private double y;
    public GridLocator(String arg)
    {
        gl = arg;
        double[] o = calc(arg);
        x = o[0];
        y = o[1];

    }
    private static double[] calc(String gl)
    {
        char[] ch = gl.ToCharArray();
        var x = (ch[0] - 'A') * 20 + (ch[2] - '0') * 2 - 180 + 1.0;
        var y = (ch[1] - 'A') * 10 + (ch[3] - '0') * 1 - 90 + 0.5;
        return new double[2] { x, y };

    }
    public double getX()
    {
        return x;
    }
    public double getY()
    {
        return y;
    }

    public Boolean isGridLocator(String arg)
    {
        if (arg.Length != 4) return false;
        bool result = Regex.IsMatch(arg, "^[A-Z]{2}[0-9]{2}$");
        if (result == false) return false;
        if (arg == "RR73") return false;
        return true;
    }
}
