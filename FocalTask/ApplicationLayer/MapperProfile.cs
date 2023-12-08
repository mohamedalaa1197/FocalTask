using AutoMapper;
using FocalTask.ApplicationLayer.DTOs.MetaData;
using FocalTask.DomainLayer;

namespace FocalTask.ApplicationLayer;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<MetaData, CreateMovieMetaDataPayload>().ReverseMap();
        CreateMap<MetaData, GetMovieMetaDataResponse>().ReverseMap();
    }
}