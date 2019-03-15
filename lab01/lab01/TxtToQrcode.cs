using System;
using System.IO;
using System.Drawing;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Gma.QrCodeNet.Encoding;
using System.Drawing.Imaging;
using System.Data;
using System.Data.OleDb;
using System.Xml;
namespace lab01
{


    class TxtToQrcode
    {

        // 摘要：
        //      将str字符串转化成QRCode,以png文件格式存储
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
                //摘要：
                //    判断字符串是否符合生成二维码的要求
                if (str.Length < 4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("第{0}行字符串长度必须大于四", line);
                    Console.ResetColor();
                    continue;
                }
                else
                {
                    //摘要：
                    //    保存文件路径
                    string dir = path.Substring(0, path.LastIndexOf('\\')) + @"\png\";
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    String fileName = dir + line.ToString("000") + str.Substring(0, 4) + ".png";
                    TxtToQrcode.StringToPng(str, fileName);
                }
                line++;
            }
        }
            //摘要:
            //    将字符串生成二维码,并输出到控制台
        public static void StringToConsole(String str)
            {
                //摘要：
                //    判断字符串是否符合要求
                if (str.Length >= 25)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("命令行参数太多！");
                    Console.ResetColor();
                    return;
                }
                //摘要：
                //    生成二维码
                QrEncoder qre = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrc = qre.Encode(str);
                //摘要：
                //    将二维码输出到控制台
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
                            Console.ResetColor();
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
        /// <summary>
        ///将Excel文件中的字符串转化成QRCode,以png文件格式存储
        ///这种处理Excel文件的方法需要在系统上安装AccessDatabaseEngine.exe文件
        ///因此，这种写法兼容性不太好
        ///这是将Excel文件变成XML格式的字符串，然后将其变成Qrcode
        /// </summary>
        /// <param name="path"></param>
        public static void ExcelToXMLToQrcode(String path)
        { 
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataSet ds = new DataSet("dataset");
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            String tableName = "dh";int k = 2;
            while (true)
            {
                tableName = schemaTable.Rows[0][k].ToString().Trim();k++;
                if (tableName == "TABLE")
                    break;
                string strExcel = "select * from[" + tableName + "]";
                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(strExcel, conn);
                oleDbDataAdapter.Fill(ds);
            }
            StringWriter sr = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sr);
            ds.WriteXml(xw);
            String s = sr.ToString();
            String str = string.Empty;
            for(int i=0;i<s.Length-1;i++)
            {
                str += s[i];
                if (s[i] == '>' && s[i + 1] == '<')
                    str += "\n";
            }
            str += s[s.Length - 1];
            //摘要：
            //    在Excel文件所在目录下，创建excelToPng.png的文件
            //    存储Qrcode
            String filename = path.Substring(0, path.LastIndexOf('\\')) +@"\excelToPng.png";
            TxtToQrcode.StringToPng(str, filename);
        }
        /// <summary>
        /// 将字符串变成QrCode，以png格式存储在文件中
        /// </summary>
        /// <param name="filename">存储文件路径</param>
        /// <param name="str">要编码成QrCode的字符串</param>
        public static void StringToPng(String str,String filename)
        {
            //摘要：
            //     定义字符串长度不大于1000，
            if (str.Length > 1000)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("文件过大");
                Console.ResetColor();
                return;
            }
            QrEncoder qrEncoder = new QrEncoder();
            QrCode qrCode = qrEncoder.Encode(str);
            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two),
            Brushes.Black, Brushes.White);
            using (MemoryStream stream = new MemoryStream())
            {
                render.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                Image im = Bitmap.FromStream(stream);
                im.Save(filename, ImageFormat.Png);
            }
        }

        /// <summary>
        /// 将Excel文件中的数据以Json格式转换成字符串，
        /// 编码成QrCode
        /// 存储在json文件夹下的png图像中
        /// </summary>
        /// <param name="path">Excel文件目录</param>
        public static void ExcelToJsonToQrcode(String path)
        {

        }

    }


}
