using Srt.Core.Model;

namespace Srt.Core.Core;

public class FileHandler
{
    public void WriteContent(string filePath, List<SrtSnip> snips)
    {
        var lines = new List<string>();

        foreach (var snip in snips)
        {
            lines.Add(snip.Index);
            lines.Add(snip.Timestamp);

            foreach (var contentLine in snip.Content)
            {
                lines.Add(contentLine);
            }

            lines.Add(string.Empty);
        }

        try
        {
            // Create new file if it does not exist
            // Throw IOException if the file is locked/in use
            using var stream = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None);

            using var writer = new StreamWriter(stream);

            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
        }
        catch (IOException ex)
        {
            throw new IOException(
                $"The file '{filePath}' is currently in use or could not be written.",
                ex);
        }
    }

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