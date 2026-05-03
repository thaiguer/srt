using Srt.Core.Model;

namespace Srt.Core.Core;

public class FileReader
{
    public List<SrtSnip> ReadContent(string filePath)
    {
        var result = new List<SrtSnip>();

        if (!File.Exists(filePath))
            throw new FileNotFoundException("SRT file not found.", filePath);

        var lines = File.ReadAllLines(filePath);

        SrtSnip? current = null;

        foreach (var rawLine in lines)
        {
            var line = rawLine.TrimEnd();

            // Empty line = end of current block
            if (string.IsNullOrWhiteSpace(line))
            {
                if (current != null)
                {
                    result.Add(current);
                    current = null;
                }

                continue;
            }

            // Start new block
            if (current == null)
            {
                current = new SrtSnip
                {
                    Index = line
                };

                continue;
            }

            // Timestamp line
            if (string.IsNullOrEmpty(current.Timestamp))
            {
                current.Timestamp = line;
                continue;
            }

            // Subtitle content
            current.Content.Add(line);
        }

        // In case file does not end with empty line
        if (current != null)
        {
            result.Add(current);
        }

        return result;
    }
}