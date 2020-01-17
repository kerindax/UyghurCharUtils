using System.Windows.Forms;
// +----------------------------------------------------------------------
// | Update: 2020-01-17 13:54
// +----------------------------------------------------------------------
// | Author: Kerindax <1482152356@qq.com>
// +----------------------------------------------------------------------
namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Uyghur.CharUtils utils = new Uyghur.CharUtils();
            string source = "سالام C#";
            string target1 = utils.Basic2Extend(source);//基本区 转换 扩展区
            string target2 = utils.Extend2Basic(target1);//扩展区 转换 基本区

            string target3 = utils.Basic2RExtend(source);//基本区 转换 反向扩展区
            string target4 = utils.RExtend2Basic(target3);//反向扩展区 转换 基本区

            MessageBox.Show(target1 + "\n" + target2 + "\n" + target3 + "\n" + target4);
        }
    }
}
