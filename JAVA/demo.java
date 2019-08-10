/**
 *
 * @author Sherer
 * @update 2018-09-07 11:28
 */
public class demo {

    public static void main(String[] args) {
        UyghurCharUtils utils = new UyghurCharUtils();
        String source = "سالام Java";

        String target1 = utils.Basic2Extend(source);//基本区 转换 扩展区
        String target2 = utils.Extend2Basic(target1);//基本区 转换 扩展区

        String target3 = utils.Basic2RExtend(source);//基本区 转换 扩展区
        String target4 = utils.RExtend2Basic(target3);//基本区 转换 扩展区
        System.out.println(target1 + "\n" + target2 + "\n" + target3 + "\n" + target4);
    }
}