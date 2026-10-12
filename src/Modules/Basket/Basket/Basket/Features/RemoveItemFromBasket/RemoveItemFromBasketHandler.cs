namespace Basket.Basket.Features.RemoveItemFromBasket;

public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
    : ICommand<RemoveItemFromBasketResult>;

public record RemoveItemFromBasketResult(Guid Id);

public class RemoveItemFormBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
{
    public RemoveItemFormBasketCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product Id cannot be empty");
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username cannot be empty");
    }
}

public class RemoveItemFromBasketHandler(IBasketRepository repository) : ICommandHandler<RemoveItemFromBasketCommand,RemoveItemFromBasketResult>
{
    public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
    {
        var shoppingCart = await repository.GetBasket(command.UserName, false, cancellationToken);
        
        shoppingCart.RemoveItem(command.ProductId);
        
        await repository.SaveChangesAsync(command.UserName, cancellationToken);
        
        return new RemoveItemFromBasketResult(shoppingCart.Id);
    }
}