using MediatR;

namespace ByteBox.FileStore.Application.Abstraction;

public interface IQuery<TResponse> : IRequest<TResponse>;