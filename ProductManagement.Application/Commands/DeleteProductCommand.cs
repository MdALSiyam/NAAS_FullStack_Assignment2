using MediatR;

namespace ProductManagement.Application.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public string Id { get; set; }
    }
}