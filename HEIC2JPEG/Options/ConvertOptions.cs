using CommandLine;

namespace HEIC2JPEG.Options;

[Verb("convert", HelpText = "Convert a .heic to a .jpg")]
internal sealed class ConvertOptions
{
    [Option('d', "delete", Default = true, HelpText = "Determines whether or not to delete the original file")]
    public bool Delete { get; set; }

    [Value(0, HelpText = "The file or directory to convert. Defaults to the current directory if unspecified")]
    public string? FileOrDirectory { get; set; }
}
