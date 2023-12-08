using AutoMapper;
using FocalTask.ApplicationLayer.DTOs.MetaData;
using FocalTask.ApplicationLayer.DTOs.State;
using FocalTask.ApplicationLayer.Interfaces;
using FocalTask.DomainLayer;
using FocalTask.RepositoryLayer.Interfaces;

namespace FocalTask.ApplicationLayer.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public MovieService(IMovieRepository movieRepository, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateMetaData(CreateMovieMetaDataPayload movieMetaDataPayload)
    {
        var metaData = _mapper.Map<MetaData>(movieMetaDataPayload);
        return await _movieRepository.CreateMovieMetaData(metaData);
    }

    public async Task<List<GetMovieMetaDataResponse>> GetMetaDataByMovieId(int movieId)
    {
        var metaData = await _movieRepository.GetMetaDataByMovieId(movieId);
        var result = metaData.GroupBy(meta => meta.Language)
            .Select(g =>
            {
                var highestId = g.Max(z => z.Id);
                return g.First(f => f.Id == highestId);
            });
        return _mapper.Map<List<GetMovieMetaDataResponse>>(result.OrderBy(meta => meta.Language));
    }

    public async Task<List<GetMovieStateResponse>> GetMovieStateResponse()
    {
        var getAllMovies = await _movieRepository.GetAllMetaData();

        // Get only the movie for english to calculate its statistics
        var moviesInLanguage = getAllMovies.Where(movie => movie.Language == "EN");

        var result = moviesInLanguage.Select(movie =>
        {
            var stateForMovie = _movieRepository.GetAllMovieState(movie.MovieId);
            var watches = stateForMovie.Count();
            var totalDurationInSeconds = stateForMovie.Sum(state => state.WatchDurationInMs / 1000);
            var averageDuration = watches > 0 ? totalDurationInSeconds / watches : 0;

            return new GetMovieStateResponse
            {
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                MovieId = movie.MovieId,
                AverageWatchDurationS = averageDuration,
                Watches = watches
            };
        }).OrderByDescending(x => x.Watches).ToList();

        return result;
    }
}