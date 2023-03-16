namespace CarRentalSystem.Application.Mapping;

using AutoMapper;

public interface IMapFrom<T>
{
    // We are using a new C# feature here – default interface methods.
    void Mapping(Profile mapper) => mapper.CreateMap(typeof(T), this.GetType());
}
