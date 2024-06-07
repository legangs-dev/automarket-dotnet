namespace AutoMarket.API.Applications.Vehicle.Commands;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, Guid>
{
    public Task<Guid> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}

public record CreateVehicleCommand(string ListingId, int Year, decimal Price) : IRequest<Guid>;

internal class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator(ILogger<CreateVehicleCommandValidator> logger)
    {
        RuleFor(x => x.ListingId).NotEmpty();
        RuleFor(x => x.Year).InclusiveBetween(1900, DateTime.Now.Year);
        RuleFor(x => x.Price).GreaterThan(0);

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Instance created - {ClassName}", GetType().Name);
        }
    }
}
