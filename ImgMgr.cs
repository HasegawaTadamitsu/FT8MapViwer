public class ImageMgr
{

    private int pbox_width;
    private int pbox_height;
    private System.Drawing.Bitmap originalBmp;
    private System.Drawing.Bitmap circleBmp;
    private GridLocator myGL = new GridLocator("PM85");

    public ImageMgr(int pbox_width, int pbox_height)
    {
        this.pbox_width = pbox_width;
        this.pbox_height = pbox_height;

        originalBmp = FT8MapViwer.Properties.Resources.japanMap;
        circleBmp = FT8MapViwer.Properties.Resources.circle;
    }


    public System.Drawing.Bitmap getImageOrg()
    {
        return (System.Drawing.Bitmap)originalBmp.Clone();
        //return originalBmp;

    }

    public System.Drawing.Bitmap getPositionMark()
    {
        return circleBmp;
    }

    public static Point longLat2PicBoxPo(double[] longLat, Point picSize)
    {
        //var x = (int)Math.Round((longLat[0] + 180) / 360 * picSize.X);
        //var y = (int)Math.Round((-longLat[1] + 90) / 180 * picSize.Y);
        var x = (int)Math.Round((longLat[0]  - 120 ) / (160-120) * picSize.X);
        var y = (int)Math.Round((-longLat[1] +  46 ) / (46-18) * picSize.Y);
        return new Point(x, y);
    }

    public Point toChangePoint(double[] longLat)
    {
        return longLat2PicBoxPo(longLat, new Point(originalBmp.Width, originalBmp.Height));
    }
    public Point getMyPicBoxPo()
    {
        return
        longLat2PicBoxPo(new double[] { myGL.getLong(), myGL.getLat() }, new Point(originalBmp.Width, originalBmp.Height));
    }

    public GridLocator getMyGridLocator()
    {
        return myGL;
    }
}
