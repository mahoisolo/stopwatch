using System;
using System.Threading;

public class Stopwatch
{
    public delegate void StopwatchEventHandler(string message);
    public event StopwatchEventHandler? OnStarted; 
    public event StopwatchEventHandler? OnStopped; 
    public event StopwatchEventHandler? OnReset; 

    private TimeSpan timeElapsed;
    private bool isRunning;
    private Thread? timerThread; // Use nullable thread

    public Stopwatch()
    {
        timeElapsed = TimeSpan.Zero;
        isRunning = false;
    }

    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            timerThread = new Thread(Tick);
            timerThread.Start();
            OnStarted?.Invoke("Stopwatch Started!");
        }
    }

    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            timerThread?.Join(); // Wait for the thread to finish
            OnStopped?.Invoke("Stopwatch Stopped!");
        }
    }

    public void Reset()
    {
        Stop();
        timeElapsed = TimeSpan.Zero;
        OnReset?.Invoke("Stopwatch Reset!");
    }

    private void Tick()
    {
        while (isRunning)
        {
            Thread.Sleep(1000); // Wait for 1 second
            timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));
            Console.WriteLine($"Time Elapsed: {timeElapsed}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Stopwatch stopwatch = new Stopwatch();

       
        stopwatch.OnStarted += message => Console.WriteLine(message);
        stopwatch.OnStopped += message => Console.WriteLine(message);
        stopwatch.OnReset += message => Console.WriteLine(message);

Console.WriteLine("Press S to Start");
Console.WriteLine("Press T to Stop");
Console.WriteLine("Press R to Reset");
Console.WriteLine("Press Q to Quit");        


        while (true)
        {
            var input = Console.ReadKey(true).KeyChar;

            switch (input)
            {
                case 's':
                case 'S':
                    stopwatch.Start();
                    break;
                case 't':
                case 'T':
                    stopwatch.Stop();
                    break;
                case 'r':
                case 'R':
                    stopwatch.Reset();
                    break;
                case 'q':
                case 'Q':
                    stopwatch.Stop();
                    return;
                default:
                    Console.WriteLine("Invalid input. Please press S, T, R, or Q.");
                    break;
            }
        }
    }
}
