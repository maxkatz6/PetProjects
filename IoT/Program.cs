using System;
using Iot.Device.CpuTemperature;
using System.Threading;
using System.Device.I2c;
using Iot.Device.Bmp180;

namespace IoT
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0
                || !int.TryParse(args[0], out var busId))
            {
                busId = 1;
            }

            var cpuTemperature = new CpuTemperature();

            var i2cSettings = new I2cConnectionSettings(busId, Bmp180.DefaultI2cAddress);
            var i2cDevice = I2cDevice.Create(i2cSettings);
            var i2cBmp280 = new Bmp180(i2cDevice);

            using (i2cBmp280)
            {
                while (true)
                {
                    i2cBmp280.SetSampling(Sampling.Standard);

                    var tempValue = i2cBmp280.ReadTemperature();
                    Console.WriteLine($"Temperature {tempValue.Celsius} \u00B0C");

                    var preValue = i2cBmp280.ReadPressure();
                    Console.WriteLine($"Pressure {preValue} hPa");

                    var seaLevelPressureValue = i2cBmp280.ReadSeaLevelPressure();
                    Console.WriteLine($"SeaLevelPressure {seaLevelPressureValue} hPa");

                    var altValue = i2cBmp280.ReadAltitude();
                    Console.WriteLine($"Altitude {altValue:0.##} m");

                    if (cpuTemperature.IsAvailable)
                    {
                        Console.WriteLine($"The CPU temperature is {cpuTemperature.Temperature.Celsius}");
                    }

                    if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        return;
                    }

                    Thread.Sleep(1000);
                }
            }
        }
    }
}
