using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models.Base;
using System.Net;

namespace FaultReportingAPI.Core.Dto
{
    public class DataResult<T> where T : CoreBaseEntity
    {
        public T? Data { get; set; }
        public IEnumerable<T>? ListData { get; set; }
        public int? Count { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
    public class DataResult
    {
        public HttpStatusCode? StatusCode { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class HelperEntity
    {
        public object Entity { get; set; } = default!;
        public EntityStateEnum EntityState { get; set; }
        public string EntityName { get; set; } = default!;
        public Dictionary<string, object>? ChangedFields { get; set; }
    }
    public class HelperEntity<TEntity> where TEntity : class
    {
        public TEntity Entity { get; set; } = default!;
        public EntityStateEnum EntityState { get; set; }
        public Dictionary<string, object>? ChangedFields { get; set; }
    }
    public class Response<T> where T : CoreBaseEntity
    {
        public T? Data { get; set; }
        public IEnumerable<T>? ListData { get; set; }
        public int? Count { get; set; }
        public bool? Success { get; set; }
        public string? Message { get; set; }
    }
    public class ResponseSingle<T> where T : CoreBaseEntity
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool Success { get; set; }

    }
    public class Response: CoreBaseEntity
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
    }
    public class ResponseList<T> where T : CoreBaseEntity
    {
        public IEnumerable<T>? ListData { get; set; }
        public int? Count { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
