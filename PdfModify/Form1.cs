using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PdfModify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RemoveText(oldFile,newFile, "惠州大亚湾西区新寮新村三巷11号龙泉花园3栋2008号房");
            MessageBox.Show("finished");
        }
        string oldFile = @"C:\\Lib\\zzz.pdf";
        string newFile = @"C:\\Lib\\new.pdf";
        string fontFile = @"C:\Windows\Fonts\simsun.ttc,1";
        public void RemoveText(string srcFile,string targetFile,string text)
        {
            // 输出文件pdf
            string outputFilePath = targetFile;
            // 输入文件pdf
            string inputFilePath = srcFile;
            try
            {
                using (Stream inputPdfStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Stream outputPdfStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                using (Stream outputPdfStream2 = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    // 打开原始pdf进行读操作
                    PdfReader reader = new PdfReader(inputPdfStream);
                    
                    // 创建一个stamper对象，进行写操作
                    PdfStamper stamper = new PdfStamper(reader, outputPdfStream); //{ FormFlattening = true, FreeTextFlattening = true };

                    // 1. 以图片覆盖需要写入的地方“坐落”
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(new Bitmap(Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox6.Text)), BaseColor.WHITE);
                    
                    // “坐落”一栏的位置
                    image.SetAbsolutePosition(Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text));
                    
                    // 将图像添加到第二页
                    var contentByte = stamper.GetOverContent(2);
                    contentByte.ResetRGBColorFill();
                    stamper.GetOverContent(2).AddImage(image, true);

                    // 2. 在被图像覆盖的地方写入目标文字
                    iTextSharp.text.io.StreamUtil.AddToResourceSearch(Assembly.Load("iTextAsian"));
                    BaseFont baseFont = BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", BaseFont.NOT_EMBEDDED);
                    //var baseFont = BaseFont.CreateFont(fontFile,BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    contentByte.BeginText();
                    contentByte.SetFontAndSize(baseFont, Convert.ToInt32(textBox1.Text));

                    SizeF size = GetLenOfString(text);

                    textBox5.Text += "width: " + size.Width + "\theight:"+size.Height+ "\r\n";

                    float X = Convert.ToInt32(textBox2.Text) + (Convert.ToInt32(textBox4.Text) - size.Width) / 2;

                    float centerY = (20 - size.Height) / 2.0f;
                    textBox5.Text += "centerY: " + centerY + "\r\n";
                    float Y = Convert.ToInt32(textBox3.Text) + centerY*2;
                    textBox5.Text += "startX: " + X + "\r\n";
                    textBox5.Text += "startY: " + Y + "\r\n";

                    contentByte.ShowTextAligned(PdfContentByte.ALIGN_LEFT, text,X, Y, 0);

                    contentByte.EndText();

                    stamper.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private SizeF GetLenOfString(string multiLineString)
        {
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
            {
                PrivateFontCollection pfc = new PrivateFontCollection();
                pfc.AddFontFile(@"C:\Windows\Fonts\STSONG.TTF");
                System.Drawing.Font font = new System.Drawing.Font(pfc.Families[0], 10, FontStyle.Regular,GraphicsUnit.Pixel);
                SizeF size = graphics.MeasureString(multiLineString, font);
                return size;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
