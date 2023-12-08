using FocalTask.DomainLayer;

namespace FocalTask.RepositoryLayer.Interfaces;

public interface IMovieRepository
{
    Task<bool> CreateMovieMetaData(MetaData metaData);
    Task<List<MetaData>?> GetMetaDataByMovieId(int movieId);
    List<MovieStats>? GetAllMovieState(int targetMovie);
    Task<List<MetaData>?> GetAllMetaData();
}