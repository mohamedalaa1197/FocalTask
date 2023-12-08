using FocalTask.DomainLayer;
using Microsoft.VisualBasic.FileIO;

namespace FocalTask.RepositoryLayer;

public static class FocalDbContext
{
    private const string _metaDataFilePath = "./RepositoryLayer/metadata.csv";
    private const string _metaDataStateFilePath = "./RepositoryLayer/stats.csv";

    public static List<MetaData>? ReadSpecificRecord(int targetMovieId)
    {
        try
        {
            var specificRecords = new List<MetaData>();

            using var parser = new TextFieldParser(_metaDataFilePath);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Skip header row if exists
            if (!parser.EndOfData)
                parser.ReadLine();

            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();

                if (fields == null || fields.Length < 5) continue;
                var movieId = Convert.ToInt32(fields[1]);

                if (movieId != targetMovieId)
                    continue;

                var metaData = new MetaData
                {
                    Id = Convert.ToInt32(fields[0]),
                    MovieId = Convert.ToInt32(fields[1]),
                    Title = fields[2],
                    Language = fields[3],
                    Duration = fields[4],
                    ReleaseYear = Convert.ToInt32(fields[5])
                };

                if (!IsNotValid(metaData))
                {
                    specificRecords.Add(metaData);
                }
            }

            return specificRecords;
        }
        catch (Exception e)
        {
            return new List<MetaData>();
        }
    }

    public static bool AddRowToCsv(MetaData newData)
    {
        try
        {
            // Create a temporary file to hold the updated data
            var tempFile = Path.GetTempFileName();

            using (var writer = new StreamWriter(tempFile))
            using (var reader = new StreamReader(_metaDataFilePath))
            {
                var headerLine = reader.ReadLine();
                writer.WriteLine(headerLine);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    writer.WriteLine(line);
                }

                var newRow =
                    $"{newData.Id},{newData.MovieId},{newData.Title},{newData.Language},{newData.Duration},{newData.ReleaseYear}";
                writer.WriteLine(newRow);
            }

            File.Copy(tempFile, _metaDataFilePath, true);
            File.Delete(tempFile);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public static IEnumerable<MetaData> ReadAllMetaDataFromCsvFile()
    {
        var metaDataList = new List<MetaData>();

        using var parser = new TextFieldParser(_metaDataFilePath);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");

        // Skip header row
        if (!parser.EndOfData)
            parser.ReadLine();

        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();

            if (fields == null || fields.Length < 6) continue;
            var metaData = new MetaData
            {
                Id = Convert.ToInt32(fields[0]),
                MovieId = Convert.ToInt32(fields[1]),
                Title = fields[2],
                Language = fields[3],
                Duration = fields[4],
                ReleaseYear = Convert.ToInt32(fields[5])
            };

            metaDataList.Add(metaData);
        }

        return metaDataList;
    }

    public static IEnumerable<MovieStats> ReadAllStateDataFromCsvFile(int targetMovieId)
    {
        var metaDataList = new List<MovieStats>();

        using var parser = new TextFieldParser(_metaDataStateFilePath);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");

        if (!parser.EndOfData)
            parser.ReadLine();

        while (!parser.EndOfData)
        {
            var fields = parser.ReadFields();
            var movieId = Convert.ToInt32(fields[0]);

            if (movieId != targetMovieId)
                continue;
            if (fields == null || fields.Length < 2) continue;
            var metaData = new MovieStats
            {
                MovieId = Convert.ToInt32(fields[0]),
                WatchDurationInMs = long.Parse(fields[1])
            };
            metaDataList.Add(metaData);
        }

        return metaDataList;
    }

    private static bool IsNotValid(MetaData metaData)
    {
        return string.IsNullOrEmpty(metaData.Duration) || string.IsNullOrEmpty(metaData.Language) ||
               string.IsNullOrEmpty(metaData.Title);
    }
}