using Via.EventAssociation.Core.Domain.Aggregates.Locations;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Location.CreateBooking;

public class CreateBooking_Test
{
    [Fact]
    public void CreateBookingShouldReturnSuccess()
    {
        // Arrange
        var locationId = ViaLocationId.Create().Payload;
        var EventId = ViaEventId.Create().Payload;
        var startDate = ViaStartDate.Create(DateTime.Now).Payload;
        var endDate = ViaEndDate.Create(DateTime.Now.AddHours(1)).Payload;
        var bookingId = ViaBookingId.Create().Payload;
        
        var booking= ViaBooking.Create(bookingId, locationId, EventId, startDate, endDate);
        // Act
        
        var result = booking;
        
        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public void CreateBookingShouldReturnFailureWhenLocationIdIsNull()
    {
        // Arrange
        var EventId = ViaEventId.Create().Payload;
        var startDate = ViaStartDate.Create(DateTime.Now).Payload;
        var endDate = ViaEndDate.Create(DateTime.Now.AddHours(1)).Payload;
        var bookingId = ViaBookingId.Create().Payload;
        
        var booking= ViaBooking.Create(bookingId, null, EventId, startDate, endDate);
        // Act
        
        var result = booking;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    [Fact]
    
    public void CreateBookingShouldReturnFailureWhenEventIdIsNull()
    {
        // Arrange
        var locationId = ViaLocationId.Create().Payload;
        var startDate = ViaStartDate.Create(DateTime.Now).Payload;
        var endDate = ViaEndDate.Create(DateTime.Now.AddHours(1)).Payload;
        var bookingId = ViaBookingId.Create().Payload;
        
        var booking= ViaBooking.Create(bookingId, locationId, null, startDate, endDate);
        // Act
        
        var result = booking;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    
    [Fact]
    
    public void CreateBookingShouldReturnFailureWhenStartDateIsNull()
    {
        // Arrange
        var locationId = ViaLocationId.Create().Payload;
        var EventId = ViaEventId.Create().Payload;
        var endDate = ViaEndDate.Create(DateTime.Now.AddHours(1)).Payload;
        var bookingId = ViaBookingId.Create().Payload;
        
        var booking= ViaBooking.Create(bookingId, locationId, EventId, null, endDate);
        // Act
        
        var result = booking;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    [Fact]
    
    public void CreateBookingShouldReturnFailureWhenEndDateIsNull()
    {
        // Arrange
        var locationId = ViaLocationId.Create().Payload;
        var EventId = ViaEventId.Create().Payload;
        var startDate = ViaStartDate.Create(DateTime.Now).Payload;
        var bookingId = ViaBookingId.Create().Payload;
        
        var booking= ViaBooking.Create(bookingId, locationId, EventId, startDate, null);
        // Act
        
        var result = booking;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    
    [Fact]
    
    public void CreateBookingShouldReturnFailureWhenBookingIdIsNull()
    {
        // Arrange
        var locationId = ViaLocationId.Create().Payload;
        var EventId = ViaEventId.Create().Payload;
        var startDate = ViaStartDate.Create(DateTime.Now).Payload;
        var endDate = ViaEndDate.Create(DateTime.Now.AddHours(1)).Payload;
    
        
        var booking= ViaBooking.Create(null, locationId, EventId, startDate, endDate);
        // Act
        
        var result = booking;
        
        // Assert
        Assert.True(result.OperationErrors.Any());
    }
    
    
}