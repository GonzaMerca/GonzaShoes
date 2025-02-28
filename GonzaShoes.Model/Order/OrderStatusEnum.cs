namespace GonzaShoes.Model.Order
{
    public enum OrderStatusEnum
    {
        Pending, // Pedido recibido, pero no pagado
        Paid,    // Pedido pagado
        Shipped, // Pedido enviado
        Delivered // Pedido entregado
    }
}
