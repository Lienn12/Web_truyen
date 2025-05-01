using System;
using System.Drawing;
using System.Drawing.Imaging;

public class ImageGenerator
{
    public static void TaoAnhBiaMacDinh(string tenTruyen, string pathOutput)
    {
        int width = 512;
        int height = 800;
        Color backgroundColor = ColorTranslator.FromHtml("#977DFF"); 
        Color textColor = Color.White;

        using (Bitmap bmp = new Bitmap(width, height))
        using (Graphics g = Graphics.FromImage(bmp))
        {
            // Vẽ nền
            g.Clear(backgroundColor);
            Font font = new Font("Quicksand", 36, FontStyle.Bold, GraphicsUnit.Pixel);
            SizeF textSize = g.MeasureString(tenTruyen, font);

            float x = (width - textSize.Width) / 2;
            float y = (height - textSize.Height) / 2;

            using (Brush brush = new SolidBrush(textColor))
            {
                g.DrawString(tenTruyen, font, brush, x, y);
            }

            // Lưu ảnh
            bmp.Save(pathOutput, ImageFormat.Png);
        }

        Console.WriteLine("Đã tạo ảnh bìa: " + pathOutput);
    }
}
