using System.Net.Http.Headers;
using AppointmentMaker.Domain.Shared;
using AutoMapper;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AppointmentMaker.Application.Features.Shared;

public class CursorResponse<TEntityDto>
{
    //protected CursorResponse(List<TEntityDto> list,
    //    IComparable? cursor,
    //    bool hasNextPage)
    //{
    //    List = list;
    //    Cursor = cursor;
    //    HasNextPage = hasNextPage;
    //}

    //public List<TEntityDto> List { get; set; } = new();
    //public IComparable? Cursor { get; set; }
    //public bool HasNextPage { get; set; }

    //public static Result<CursorResponse<TEntityDto>> GetBaseCursorResponse<TEntity>
    //    (IEnumerable<TEntity> entities,
    //    int pageSize,
    //    IComparable? cursor,
    //    string propertyName,
    //    bool isAscendingOrder,
    //    IMapper mapper)
    //{

    //    PropertyInfo? propertyInfo = typeof(TEntity).GetProperty(propertyName);

    //    if(propertyInfo == null)
    //    {
    //        return Result.Failure<CursorResponse<TEntityDto>>
    //            (new Error("ServerError", $"The property with the name {propertyName} does not exist"));
    //    }

    //    if(propertyInfo.PropertyType.GetTypeInfo().DeclaringType != cursor?.GetType())
    //    {
    //        return Result.Failure<CursorResponse<TEntityDto>>
    //            (new Error("ServerError", "The cursor type does not match with property type"));
    //    }

    //    entities = isAscendingOrder
    //        ? entities.OrderBy(e => propertyInfo.GetValue(e) ?? throw new NullReferenceException())
    //        : entities.OrderByDescending(e => propertyInfo.GetValue(e) ?? throw new NullReferenceException());

    //    if (cursor != null)
    //    {
    //        entities = entities.Where(e =>
    //        {
    //            IComparable value = (IComparable)(propertyInfo.GetValue(e) ?? throw new NullReferenceException());
    //            return isAscendingOrder ? value.CompareTo(cursor) >= 0 : value.CompareTo(cursor) <= 0;
    //        });
    //    }

    //    entities = entities.Take(pageSize + 1);

    //    var dtos = mapper.Map<List<TEntityDto>>(entities.Take(pageSize));

    //    var newCursor = (IComparable)(propertyInfo.GetValue(entities.Last()) ?? throw new NullReferenceException());

    //    return new CursorResponse<TEntityDto>
    //        (dtos, newCursor, entities.Count() == pageSize + 1);
    //}


    private CursorResponse(List<TEntityDto> list,
        string? cursor,
        bool hasNextPage)
    {
        List = list;
        Cursor = cursor;
        HasNextPage = hasNextPage;
    }

    public List<TEntityDto> List { get; set; } = new();
    public string? Cursor { get; set; }
    public bool HasNextPage { get; set; }

    public static Result<CursorResponse<TEntityDto>> GetCursorResponse<TEntity>
    (IEnumerable<TEntity> entities,
        int pageSize,
        string? cursor,
        string propertyName,
        bool isAscendingOrder,
        IMapper mapper)
    {

        PropertyInfo? propertyInfo = typeof(TEntity).GetProperty(propertyName);

        if (propertyInfo == null)
        {
            return Result.Failure<CursorResponse<TEntityDto>>
                (new Error("Error.ServerError", $"The property with the name {propertyName} does not exist"));
        }

        if (!propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IComparable)))
        {
            return Result.Failure<CursorResponse<TEntityDto>>
                (new Error("Error.ComparableError", "Field must be comparable"));
        }

        entities = isAscendingOrder
            ? entities.OrderBy(e => propertyInfo.GetValue(e) ?? throw new NullReferenceException())
            : entities.OrderByDescending(e => propertyInfo.GetValue(e) ?? throw new NullReferenceException());

        if (cursor != null)
        {
            var cursorData = Deserialize( cursor, propertyInfo.PropertyType);

            entities = entities.Where(e =>
            {
                IComparable value = (IComparable)(propertyInfo.GetValue(e) ?? throw new NullReferenceException());
                return isAscendingOrder ? value.CompareTo(cursorData) >= 0 : value.CompareTo(cursor) <= 0;
            });
        }

        entities = entities.Take(pageSize + 1);

        var dtos = mapper.Map<List<TEntityDto>>(entities.Take(pageSize));

        var newCursor = propertyInfo.GetValue(entities.Last()) ?? throw new NullReferenceException();


        return new CursorResponse<TEntityDto>
            (dtos, Encode(newCursor), entities.Count() == pageSize + 1);
    }

    private static IComparable Deserialize(string base64, Type type)
    {
        var data = Convert.FromBase64String(base64);
        string json = Encoding.UTF8.GetString(data);
        return (JsonSerializer.Deserialize(json, type) as IComparable)!;
    }

    private static string Encode(object obj)
    {
        var newCursorJson = JsonSerializer.Serialize(obj);
        var byteArray = Encoding.UTF8.GetBytes(newCursorJson);
        return Convert.ToBase64String(byteArray);
    }
}
