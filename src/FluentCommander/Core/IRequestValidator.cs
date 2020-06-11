namespace FluentCommander.Core
{
    public interface IRequestValidator<TRequest>
    {
        void Validate(TRequest request);
    }
}
