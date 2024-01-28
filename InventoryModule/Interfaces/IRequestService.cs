using InventoryModule.Dtos;
using InventoryModule.Enums;

namespace InventoryModule.Interfaces
{
    public interface IRequestService
    {
        public Task<ResponseModel<IEnumerable<RequestResponseDto>>> GetAllAsync();
        public Task<ResponseModel<RequestResponseDto>> GetByIdAsync(int id);
        public Task<ResponseModel<RequestResponseDto>> CreateAsync(CreateRequestDto requestDto);
        Task<ResponseModel<RequestResponseDto>> ChangeRequestStatus(int id, Status status);
    }
}
