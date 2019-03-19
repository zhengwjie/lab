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
            try
            {
                if (args.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("请传入相关参数");
                    Console.ResetColor();
                    return;
                }
                int k = 0;
                //摘要:
                //    method判断将二维码输出到控制台还是保存在文件中
                bool method = false;
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-f")
                    {
                        k = i + 1;
                        method = true;
                        if (args.Length <= k)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("请输入文件名！");
                            Console.ResetColor();
                            return;
                        }
                    }
                }
                //摘要：
                //     判断输入的文件是Excel,txt
                //     
                if (method)
                {
                    String[] str = args[k].Split('.');
                    String last = str[str.Length - 1];
                    if (last == "txt")
                    {
                        try
                        {
                            TxtToQrcode.TxtToPNG(args[k]);
                        }catch(Exception)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("找不到文件！");
                            Console.ResetColor();
                            return;
                        }
                    } 
                    else if (last == "xlsx")
                    {
                        try
                        {
                            TxtToQrcode.ExcelToJsonToQrcode(args[k]);
                            TxtToQrcode.ExcelToXMLToQrcode(args[k]);
                        }
                        catch (Exception)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("找不到文件！");
                            Console.ResetColor();
                            return;
                        }
                    }
                    else if (args[k] == "mysql")
                    {
                        k = k + 2;
                        if (k >= args.Length)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("请输入连接字符串与查询语句！");
                            Console.ResetColor();
                            return;
                        }
                        try
                        {
                            TxtToQrcode.MysqlToxmlQrcode(args[k - 1], args[k]);
                            TxtToQrcode.MysqlToJsonQrcode(args[k - 1], args[k]);
                        }
                        catch (Exception)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("连接字符串或查询语句错误！");
                            Console.ResetColor();
                            return;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("不支持的文件格式！！！");
                        Console.ResetColor();
                        return;
                    }
                }
                //摘要:
                //    输出到控制台中
                //    将第二个命令行参数作为要生成二维码的信息
                else
                {
                    try
                    {
                        TxtToQrcode.StringToConsole(args[1]);
                    } catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("请输入要生成二维码的字符串");
                        Console.ResetColor();
                        return;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("参数错误");
            }
        }
    }
}
