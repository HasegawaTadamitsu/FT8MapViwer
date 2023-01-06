public class ImageMgr
{

    private int pbox_width;
    private int pbox_height;
    private System.Drawing.Bitmap originalBmp;

    public ImageMgr(int pbox_width, int pbox_height)
    {
        this.pbox_width = pbox_width;
        this.pbox_height = pbox_height;

        originalBmp = FT8MapViwer.Properties.Resources.worldMap;
    }


    public System.Drawing.Bitmap getImageOrg()
    {
        return originalBmp;
    }
}
