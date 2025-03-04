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
        private readonly IMapper mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            this.orderRepository = orderRepository;

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

            return validationResultDTO;
        }

        private async Task<ValidationResultDTO> ValidateSaveOrderAsync(OrderDTO Order)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();

            //if (string.IsNullOrWhiteSpace(Order.Name))
            //    validationResultDTO.ErrorMessages.Add("Falta ingresar el nombre del usuario");
            //else if (Order.Name.Length > 50)
            //    validationResultDTO.ErrorMessages.Add("El nombre no puede contener mas de 50 caracteres");
            //else
            //{
            //    Order? OrderDb = await this.orderRepository.GetOrderByNameAsync(Order.Name);

            //    if (OrderDb != null && (Order.Id == 0 || OrderDb.Id != Order.Id))
            //        validationResultDTO.ErrorMessages.Add("El nombre de la marca debe ser único");
            //}

            return validationResultDTO;
        }

        public async Task<ValidationResultDTO> UpdateStatusAsync(int OrderId, bool isActive)
        {
            ValidationResultDTO validationResultDTO = new ValidationResultDTO();
            if (validationResultDTO.HasErrors)
                return validationResultDTO;

            try
            {
                Order obj = new Order()
                {
                    Id = OrderId,
                    IsActive = !isActive,
                    UpdatedUserId = this.userId,
                    DateUpdated = DateTime.Now
                };

                await this.orderRepository.UpdateStatusAsync(obj);
            }
            catch (Exception ex)
            {
                validationResultDTO.ErrorMessages.Add(ex.Message);
            }

            return validationResultDTO;
        }
    }
}
