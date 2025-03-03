using Microsoft.AspNetCore.Routing;

namespace Common.Library.Interfaces;

public interface IEndpointDefinition
{
    void RegisterEndpoints(IEndpointRouteBuilder app);
}
