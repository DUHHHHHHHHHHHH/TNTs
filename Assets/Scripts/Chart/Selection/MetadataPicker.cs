using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class MetadataPicker
{
    public string Title;
    public string TitleUnicode;
    public string Artist;
    public string ArtistUnicode;
    public string Creator;
    public string Version;
    public string Source;
    public string Tags;

    public static MetadataPicker Parse(string filePath)
    {
        var metadata = new MetadataPicker();
        bool inMetadataSection = false;
        foreach (var line in File.ReadLines(filePath))
        {
            if (line.Trim() == "[Metadata]")
            {
                inMetadataSection = true;
                continue;
            }
            if (inMetadataSection)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("[")) // Controlla anche l'inizio di una nuova sezione
                    break; 

                var match = Regex.Match(line, @"^(?<key>[^:]+):(?<value>.*)$");
                if (match.Success)
                {
                    string key = match.Groups["key"].Value.Trim();
                    string value = match.Groups["value"].Value.Trim();

                    switch (key)
                    {
                        case "Title": metadata.Title = value; break;
                        case "TitleUnicode": metadata.TitleUnicode = value; break;
                        case "Artist": metadata.Artist = value; break;
                        case "ArtistUnicode": metadata.ArtistUnicode = value; break;
                        case "Creator": metadata.Creator = value; break;
                        case "Version": metadata.Version = value; break;
                        case "Source": metadata.Source = value; break;
                        case "Tags": metadata.Tags = value; break;
                    }
                }
            }
        }
        return metadata;
    }
}
