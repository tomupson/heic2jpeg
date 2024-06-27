using CommandLine;
using HEIC2JPEG.Options;
using ImageMagick;

namespace HEIC2JPEG;

internal static class Program
{
    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<ConvertOptions>(args)
            .WithParsed(ConvertFiles);
    }

    private static void ConvertFiles(ConvertOptions options)
    {
        if (File.Exists(options.FileOrDirectory))
        {
            ProcessFile(options.FileOrDirectory, options);
            return;
        }

        if (string.IsNullOrWhiteSpace(options.FileOrDirectory))
        {
            options.FileOrDirectory = Directory.GetCurrentDirectory();
        }

        if (!Directory.Exists(options.FileOrDirectory))
        {
            return;
        }

        foreach (string file in Directory.EnumerateFiles(options.FileOrDirectory, "*.heic", SearchOption.AllDirectories))
        {
            ProcessFile(file, options);
        }
    }

    private static void ProcessFile(string file, ConvertOptions options)
    {
        if (!Path.GetExtension(file).Equals(".heic", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        Console.WriteLine($"Processing file: {file}");

        using MagickImage image = new MagickImage(file);

        try
        {
            string newFileName = file.Replace(Path.GetExtension(file), ".jpg");
            image.Write(newFileName);

            if (options.Delete)
            {
                File.Delete(file);
            }
        }
        catch
        {
            Console.WriteLine($"Failed to process file: {file}");
        }

        Console.WriteLine($"Successfully processed{(options.Delete ? " and deleted" : string.Empty)} file: {file}");
    }
}
