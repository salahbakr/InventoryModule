using AutoMapper;
using InventoryModule.Dtos;
using InventoryModule.Entities;
using InventoryModule.Interfaces;

namespace InventoryModule.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// A method to get list of orders
        /// </summary>
        /// <returns>A response model containing list of orders</returns>

        public async Task<ResponseModel<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            if (orders.Count() == 0)
            {
                return new ResponseModel<IEnumerable<OrderResponseDto>>
                {
                    Error = "Error while retrieving orders",
                    Message = "There are no orders"
                };
            }

            return new ResponseModel<IEnumerable<OrderResponseDto>>
            {
                Message = "Retrieved orders",
                Data = _mapper.Map<IEnumerable<OrderResponseDto>>(orders)
            };
        }

        /// <summary>
        /// A method to make an automatic order if the item quantitiy falls below order point
        /// </summary>
        /// <param name="items"></param>
        /// <returns>does not return anything</returns>

        public async Task CreateOrdersAsync(IEnumerable<Item> items)
        {
            List<Order> orders = new List<Order>();

            foreach (var item in items)
            {
                if (item.Quantity < item.OrderPoint)
                {
                    orders.Add(new Order
                    {
                        SupplierName = "Supplier 1",
                        ArrivalDate = DateTime.Now.AddDays(5),
                        Item = item
                    });
                }
            }

            await _orderRepository.CreateAsync(orders);
        }
    }
}
