using UnitTests.Features.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Functional_Tests;

abstract class Program
{
    static void Main(string[] args)
    {
        /*// Example 1: Combining Success Results with Payload
        var result1 = OperationResult<string>.Success("Payload 1");
        var result2 = OperationResult<string>.Success("Payload 2");
        var combinedResult1 = OperationResult<string>.Combine(result1, result2);
        Console.WriteLine($"Example 1: Combined Result has payload: '{combinedResult1.Payload}', Success: {combinedResult1.IsSuccess}");

        // Example 2: Combining Success and Failure Results
        var successResult = OperationResult<string>.Success("Payload");
        var error = new OperationError(ErrorCode.NotFound, "Item not found");
        var failureResult = OperationResult<string>.Failure(new List<OperationError> { error });
        var combinedResult2 = OperationResult<string>.Combine(successResult, failureResult);
        Console.WriteLine($"Example 2: Combined Result Success: {combinedResult2.IsSuccess}, Errors: {combinedResult2.OperationErrors.Count}");
        PrintErrorDescriptions(combinedResult2);

        // Example 3: Combining Multiple Failure Results
        var error1 = new OperationError(ErrorCode.UnauthorizedAccess, "Unauthorized access");
        var error2 = new OperationError(ErrorCode.Timeout, "Request timed out");
        var failureResult1 = OperationResult<string>.Failure(new List<OperationError> { error1 });
        var failureResult2 = OperationResult<string>.Failure(new List<OperationError> { error2 });
        var combinedResult3 = OperationResult<string>.Combine(failureResult1, failureResult2);
        Console.WriteLine($"Example 3: Combined Result Success: {combinedResult3.IsSuccess}, Errors: {combinedResult3.OperationErrors.Count}");
        PrintErrorDescriptions(combinedResult3);

        /#1#/ Example 4: Combining Without Any Payloads
        var successWithoutPayload = OperationResult.Success();
        var serviceUnavailableError = new OperationError(ErrorCode.ServiceUnavailable, "Service unavailable");
        var serviceUnavailableResult = OperationResult<string>.Failure(new List<OperationError> { serviceUnavailableError });
        var combinedResult4 = OperationResult<string>.Combine(successWithoutPayload, serviceUnavailableResult);
        Console.WriteLine($"Example 4: Combined Result Success: {combinedResult4.IsSuccess}, Errors: {combinedResult4.OperationErrors.Count}");
        PrintErrorDescriptions(combinedResult4);#1#
    }

    static void PrintErrorDescriptions(OperationResult<string> result)
    {
        if (!result.IsSuccess && result.OperationErrors.Any())
        {
            Console.WriteLine("Error Descriptions:");
            foreach (var error in result.OperationErrors)
            {
                Console.WriteLine($"- Code: {error.Code}, Message: {error.Message}");
            }
        }*/
        
        
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
            .WithTitle("Initial Title")
            .WithStatus(ViaEventStatus.Active) 
            .Build();
        
        
        viaEvent.UpdateStatus(ViaEventStatus.Cancelled);

        Console.WriteLine(viaEvent.Title.Value);
        Console.WriteLine(viaEvent.Status);
        Console.WriteLine(viaEvent.Id.Value);
        Console.WriteLine(viaEvent.Description.Value);
        Console.WriteLine(viaEvent.DateTimeRange.StartValue);
        Console.WriteLine(viaEvent.DateTimeRange.EndValue);
        Console.WriteLine(viaEvent.MaxGuests.Value);
        Console.WriteLine(viaEvent.Visibility);
        Console.WriteLine(viaEvent.Guests.Count());
        
    }
}