using FocalTask.ApplicationLayer.DTOs.MetaData;
using FocalTask.ApplicationLayer.DTOs.State;

namespace FocalTask.ApplicationLayer.Interfaces;

public interface IMovieService
{
    Task<bool> CreateMetaData(CreateMovieMetaDataPayload movieMetaDataPayload);
    Task<List<GetMovieMetaDataResponse>> GetMetaDataByMovieId(int movieId);
    Task<List<GetMovieStateResponse>> GetMovieStateResponse();
}