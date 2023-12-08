using FocalTask.DomainLayer;
using FocalTask.RepositoryLayer.Interfaces;

namespace FocalTask.RepositoryLayer.Services;

public class MovieRepository : IMovieRepository
{
    public async Task<bool> CreateMovieMetaData(MetaData metaData)
    {
        return FocalDbContext.AddRowToCsv(metaData);
    }

    public async Task<List<MetaData>?> GetMetaDataByMovieId(int movieId)
    {
        return FocalDbContext.ReadSpecificRecord(movieId);
    }

    public async Task<List<MetaData>?> GetAllMetaData()
    {
        return FocalDbContext.ReadAllMetaDataFromCsvFile().ToList();
    }

    public List<MovieStats>? GetAllMovieState(int targetMovie)
    {
        return FocalDbContext.ReadAllStateDataFromCsvFile(targetMovie).ToList();
    }
}