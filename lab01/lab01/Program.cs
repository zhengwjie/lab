using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.Drawing.Imaging;
namespace lab01
{
    

    class Program
    {

        static void Main(String[] args)
        {
            int k=0;
            //摘要:
            //    method判断将二维码输出到控制台还是保存在文件中
            bool method = false;
            for(int i=0;i<args.Length;i++)
            {
                if (args[i] == "-f")
                {
                    k = i + 1;
                    method = true;
                }
            }
            //摘要：
            //     判断输入的文件是Excel,txt
            //     
            if(method&&k<args.Length)
            {
                String[] str = args[k].Split('.');
                String last = str[str.Length - 1];
                if (last == "txt")
                    TxtToQrcode.TxtToPNG(args[k]);
                else if (last == "xlsx")
                {
                    TxtToQrcode.ExcelToJsonToQrcode(args[k]);
                    TxtToQrcode.ExcelToXMLToQrcode(args[k]);
                }
                else if(args[k]=="mysql")
                {
                    TxtToQrcode.MysqlToxmlQrcode("server=localhost;Initial Catalog=chap1;User ID=root;Password=123456",
                "select * from student;select * from students");
                    TxtToQrcode.MysqlToJsonQrcode("server=localhost;Initial Catalog=chap1;User ID=root;Password=123456",
                "select * from students;select * from student");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("不支持的文件格式！！！");
                    Console.ResetColor();
                }
            }
            //摘要:
            //    输出到控制台中
            else
            {
                string str = "";
                for (int i = 0; i < args.Length - 1; i++)
                    str += args[i] + " ";
                str += args[args.Length - 1];
                TxtToQrcode.StringToConsole(str);
            }
        }

    }
}
