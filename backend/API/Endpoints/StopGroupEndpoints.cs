namespace API.Endpoints;

public static class StopGroupEndpoints
{
    public static void MapStopGroupEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("v1");
        group.MapGet("groups", GetGroups);
        group.MapGet("api/groups", GetGroupsApi);
        group.MapPost("api/groups", CreateGroup);
        group.MapPut("api/groups/{groupId}", UpdateGroup);
        group.MapDelete("api/groups/{groupId}", DeleteGroup);
        group.MapPut("api/groups/order", UpdateOrder);
    }

    public static async Task<IResult> GetGroups()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> GetGroupsApi()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> CreateGroup()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> UpdateGroup()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> DeleteGroup()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> UpdateOrder()
    {
        throw new NotImplementedException();
    }
}