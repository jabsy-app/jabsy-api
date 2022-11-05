public class Logger {
    public static void Log(string message) {
        var now = DateTime.Now;
        Console.WriteLine($"[{now.ToShortDateString()} {now.ToShortTimeString()}] {message}");
    }   
}