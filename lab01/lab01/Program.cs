using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
namespace lab01
{
    

    class Program
    {

        static void Main(String[] args)
        {
            int k=0;
            bool method = false;
            for(int i=0;i<args.Length;i++)
            {
                if (args[i] == "-f")
                {
                    k = i + 1;
                    method = true;
                }
            }
            if(method)
            {
                TxtToQrcode.TxtToPNG(args[k]);
            }
            else
            {
                string str = "";
                for (int i = 0; i < args.Length - 1; i++)
                    str += (args[i] + " ");
                str += args[args.Length - 1];
                TxtToQrcode.StringToConsole(str);
            }
        }

    }
}
