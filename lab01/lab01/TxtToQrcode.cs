using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Gma.QrCodeNet.Encoding;
using System.Drawing.Imaging;

namespace lab01
{


        class TxtToQrcode
        {    // 摘要：
             //      将str字符串转化成QRCode,以bmp文件格式存储
            public static void TxtToPNG(string path)
            {
                if (!File.Exists(path))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("文件不存在！");
                    Console.ResetColor();
                    return;
                }
                
                StreamReader reader = new StreamReader(path);
                int line = 1;
                // 摘要：
                //      读入一行，将生成的二维码存储在bmp文件中
                while (reader.Peek() > 0)
                {
                    String str = reader.ReadLine();
                    if (str.Length < 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("第{0}行字符串长度必须大于四", line);
                        Console.ResetColor();
                        continue;
                    }
                    else if (str.Length > 28)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("第{0}字符串长度必须小于二十五", line);
                        Console.ResetColor();
                        continue;
                    }
                    QrEncoder qrEncoder = new QrEncoder();
                    QrCode qr = qrEncoder.Encode(str);
                    string dir = path.Substring(0, path.LastIndexOf('/')) + "/png/";
                    String fileName = dir+line.ToString("000") + str.Substring(0, 4) + ".png";
                    GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        renderer.WriteToStream(qr.Matrix, ImageFormat.Png, ms);
                        Image img = Image.FromStream(ms);
                        img.Save(fileName);
                    }
                    line++;
                }
            }
            public static void StringToConsole(String str)
            {
                QrEncoder qre = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrc = qre.Encode(str);
                if (qrc.Matrix.Height >= 28)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("命令行参数太多！");
                    Console.ResetColor();
                    return;
                }
                for (int i = 0; i < qrc.Matrix.Height; i++)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < (qrc.Matrix.Width + 2); j++)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write("  ");
                        }
                        Console.WriteLine();
                    }
                    for (int j = 0; j < qrc.Matrix.Width; j++)
                    {
                        if (j == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write("  ");
                        }
                        if (!qrc.Matrix[i, j])
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        Console.Write("  ");
                        Console.ResetColor();
                        if (j == qrc.Matrix.Width - 1)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write("  ");
                        }

                    }
                    Console.ResetColor();
                    Console.WriteLine();
                    if (i == qrc.Matrix.Height - 1)
                    {
                        for (int j = 0; j < (qrc.Matrix.Width + 2); j++)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write("  ");
                        }
                        Console.ResetColor();
                        Console.WriteLine();
                    }
                }
            }
        
        }


}
