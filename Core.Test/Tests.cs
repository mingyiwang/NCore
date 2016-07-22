using System;
using System.Diagnostics;
using System.Threading;
using Core.Concurrent;

namespace Core.Test {
    public static class Tests {

        public static int Test(int threads, Action action) {

            Checks.True(threads >=1, $"Threads[{threads}] is smaller then 1");

            using (var countdown = new CountdownEvent(threads)) {
                var watch = new Stopwatch();
                watch.Start();
                for (var i = 1; i <= threads; i++) {
                    var temp = i;
                    Threads.StartNew($"Test-Thread-[{temp}]", () => {
                        try {
                            action();
                        } catch(Exception error) {
                            Console.WriteLine(error.Message);
                        }
                        finally {
                            if (countdown != null) {
                                countdown.Signal(1);
                            }
                        }
                    });
                }

                countdown.Wait();

                watch.Stop();
                Console.WriteLine("Operation took " + watch.Elapsed.Milliseconds + " MS");

                Checks.True(countdown.CurrentCount == 0,
                    $"Operation Failed : Count{countdown.CurrentCount}"
                );

                return countdown.CurrentCount;
            }

        }
    }

}