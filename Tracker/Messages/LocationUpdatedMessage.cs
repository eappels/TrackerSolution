using CommunityToolkit.Mvvm.Messaging.Messages;
using Tracker.Models;

namespace Tracker.Messages;

public class LocationUpdatedMessage : ValueChangedMessage<CustomLocation>
{
    public LocationUpdatedMessage(CustomLocation value)
        : base(value)
    {
    }
}