﻿using FluentValidation;
using MediatR;

namespace RestroLogic.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;


        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();


                if (failures.Count != 0)
                    throw new ValidationException(failures);
            }


            return await next();
        }
    }
}
