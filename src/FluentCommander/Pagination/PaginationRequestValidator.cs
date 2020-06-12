using FluentCommander.Core;
using System;

namespace FluentCommander.Pagination
{
    public class PaginationRequestValidator : IRequestValidator<PaginationRequest>
    {
        public void Validate(PaginationRequest request)
        {
            if (string.IsNullOrEmpty(request.TableName))
            {
                throw new InvalidOperationException("From(target) must be called before calling the Execute() method");
            }
        }
    }
}
