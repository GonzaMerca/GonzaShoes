using AutoMapper;
using GonzaShoes.Data.Interfaces;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Order;
using GonzaShoes.Model.Entities.Order;
using GonzaShoes.Service.Interfaces;

namespace GonzaShoes.Service.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductService productService;
        private readonly IMapper mapper;

        public OrderService(IOrderRepository orderRepository, IProductService productService, IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.productService = productService;

            this.mapper = mapper;
        }

        public async Task<OrderDTO?> GetOrderByIdAsync(int id)
        {
            return this.mapper.Map<Order, OrderDTO>(await this.orderRepository.GetOrderByIdAsync(id));
        }

        public async Task<List<OrderDTO>> GetOrdersAsync(OrderSearchDTO searchDTO)
        {
            return this.mapper.Map<List<Order>, List<OrderDTO>>(await this.orderRepository.GetOrdersAsync(searchDTO));
        }

        public async Task<ValidationResultDTO> SaveOrderAsync(OrderDTO dto)
        {
            ValidationResultDTO validationResultDTO = await ValidateSaveOrderAsync(dto);
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            Order? obj;
            if (dto.Id == 0)
            {
                obj = this.mapper.Map<OrderDTO, Order>(dto);
                obj.CreatedUserId = userId;
                obj.DateCreated = DateTime.Now;
            }
            else
            {
                obj = await this.orderRepository.GetOrderByIdAsync(dto.Id);

                this.mapper.Map(dto, obj);
                obj.DateUpdated = DateTime.Now;
                obj.UpdatedUserId = userId;
            }

            await this.orderRepository.SaveOrderAsync(obj);

            await this.UpdateStockAsync(obj.OrderItems, obj.Id, true);

            return validationResultDTO;
        }

        private async Task UpdateStockAsync(List<OrderItem> items, int orderId, bool isDecreasingStock = true, string message = null)
        {
            string description = $"Pedido Nº {orderId}";

            if (!isDecreasingStock && string.IsNullOrWhiteSpace(message))
                description = $"Pedido Nº{orderId}, anulación de pedido";

            if (!string.IsNullOrWhiteSpace(message))
                description = $"Pedido Nº{orderId}, {message}";

            this.productService.SetCurrentUser(userId);
            foreach (OrderItem item in items)
                await this.productService.UpdateStockAsync(item.ProductId, (int)item.Quantity, item.Id, orderId, description, isDecreasingStock);
        }

        private async Task<ValidationResultDTO> ValidateSaveOrderAsync(OrderDTO order)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();

            foreach (var item in order.OrderItems)
                if (!await this.productService.ValidateStockAsync(item.ProductId, (int)item.Quantity))
                    validationResultDTO.ErrorMessages.Add($"No cuenta con el suficiente stock para el producto: {item.ProductName} ({item.ColorName} - {item.SizeNumber})");

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int orderId, bool isActive)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            try
            {
                Order obj = await this.orderRepository.GetOrderByIdAsync(orderId);

                if (obj != null)
                {
                    obj.IsActive = !isActive;
                    obj.UpdatedUserId = this.userId;
                    obj.DateUpdated = DateTime.Now;

                    await this.orderRepository.UpdateStatusAsync(obj);

                    await this.UpdateStockAsync(obj.OrderItems, obj.Id, !isActive, !isActive ? "reactivación de pedido" : "anulación de pedido");
                }
                else
                    validationResultDTO.ErrorMessages.Add($"No se encontró el pedido con Id: {orderId}");
            }
            catch (Exception ex)
            {
                validationResultDTO.ErrorMessages.Add(ex.Message);
            }

            return validationResultDTO;
        }
    }
}
