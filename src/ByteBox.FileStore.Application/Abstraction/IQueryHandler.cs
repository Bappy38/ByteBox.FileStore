using MediatR;

namespace ByteBox.FileStore.Application.Abstraction;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>;