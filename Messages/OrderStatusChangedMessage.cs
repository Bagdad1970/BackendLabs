namespace Messages;

public class OrderStatusChangedMessage : BaseMessage
{
    public long OrderId { get; set; }
    public string OrderStatus { get; set; }

    public override string RoutingKey => "order.status.changed";
}