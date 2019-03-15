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
            String pa = @"F:\C#\lab01\test.xlsx";
            TxtToQrcode.ExcelToXMLToQrcode(pa);
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
            //摘要:
            //    以png格式保存在文件中
            if(method&&k<args.Length)
            {
                TxtToQrcode.TxtToPNG(args[k]);
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
