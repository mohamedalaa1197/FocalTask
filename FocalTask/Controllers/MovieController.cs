using FocalTask.ApplicationLayer.DTOs.MetaData;
using FocalTask.ApplicationLayer.DTOs.State;
using FocalTask.ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FocalTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    /// <summary>
    /// API To Get The Movie Meta Data by Movie Id
    /// </summary>
    /// <param name="movieId"></param>
    /// <returns></returns>
    [HttpGet("{movieId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetMovieMetaDataResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMetaDataByMovieId([FromRoute] int movieId)
    {
        var result = await _movieService.GetMetaDataByMovieId(movieId);
        if (result.Any())
            return Ok(result);
        return NotFound();
    }

    /// <summary>
    /// Api To add Movie Meta Data
    /// </summary>
    /// <param name="movieMetaDataPayload"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddMovieMetaData([FromBody] CreateMovieMetaDataPayload movieMetaDataPayload)
    {
        var result = await _movieService.CreateMetaData(movieMetaDataPayload);
        if (result)
            return Ok();
        return BadRequest();
    }

    /// <summary>
    /// API To Get Movies statistics 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetMovieStateResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMoviesState()
    {
        var result = await _movieService.GetMovieStateResponse();
        if (result.Any())
            return Ok(result);
        return NotFound();
    }
}