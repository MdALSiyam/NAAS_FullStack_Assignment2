using MediatR;

namespace ProductManagement.Application.Commands
{
    public class DeleteCategoryCommand : IRequest<Unit>
    {
        public string Id { get; set; }
    }
}