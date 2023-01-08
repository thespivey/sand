using Sand.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sand
{
    internal class PatternLoader
    {
        public static async Task<Pattern> LoadThr(FileInfo file)
        {
            // The THR format (theta-rho) is textual.
            // One point is stored per line.
            // A point is represented by two space-delimited floating point numbers
            // The first number is an angle in radians. 0 is North and positive numbers wind clockwise.
            // The second number is a radius in the range 0-1
            // The coordinate system of our table is in similar, but 0 is North and postive numbers wind counterclockwise.
            // We also store angles in degrees.  So we just need to convert from radians to degrees and invert.

            var pattern = new Pattern{ PatternId = 510, Points = new List<Point>() };

            using StreamReader reader = new(file.OpenRead());
            while (true)
            {
                var line = await reader.ReadLineAsync();
                if (line == null)
                {
                    break;
                }

                if (line.StartsWith("#"))
                {
                    continue;
                }

                var coords = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (coords.Length != 2)
                {
                    throw new Exception($"Invalid format: {line}");
                }

                var angle = float.Parse(coords[0]);
                var radius = float.Parse(coords[1]);

                // Invert, convert, and clamp angle
                angle = angle * -180 / MathF.PI;
                while (angle < 0) { angle += 360; }
                while (angle >= 360) { angle -= 360; }

                pattern.Points.Add(new Point { Angle = angle, Radius = radius });
            }

            return pattern;
        }
    }
}