using ObjCRuntime;
using UIKit;

namespace Surveyors_Calculator;

public class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}