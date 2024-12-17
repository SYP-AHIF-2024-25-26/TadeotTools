using Database.Repository;

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

    public record GetGroupsResponse(int Id, string Name, string Description, int Rank, int[] StopIds);

    // TODO: Put this in Functions File
    public static GetGroupsResponse[] GetAllGroups(TadeoTDbContext context, bool onlyPublic)
        => context.StopGroups
            .Where(g => onlyPublic ? g.IsPublic : true)
            .Select(g => new GetGroupsResponse(
                g.Id,
                g.Name,
                g.Description,
                g.Rank,
                context.StopGroupAssignments
                    .Where(a => a.StopGroupId == g.Id)
                    .OrderBy(a => a.Order)
                    .Select(a => a.StopId).ToArray()
            )).ToArray();

    public static async Task<IResult> GetGroups(TadeoTDbContext context)
    {
        return Results.Ok(GetAllGroups(context, true));
    }

    public static async Task<IResult> GetGroupsApi(TadeoTDbContext context)
    {
        return Results.Ok(GetAllGroups(context, false));
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