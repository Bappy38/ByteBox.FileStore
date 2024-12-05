using MediatR;

namespace ByteBox.FileStore.Application.Abstraction;

public interface ICommand : IRequest
{ }

public interface ICommand<TResponse> : IRequest<TResponse>
{ }