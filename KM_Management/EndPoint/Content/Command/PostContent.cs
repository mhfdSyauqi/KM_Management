using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Content.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Content.Command;

public record PostContentCommand(RequestPostContent Argument) : ICommand;

public class PostContentValidator : AbstractValidator<PostContentCommand>
{
	private readonly IContentRepository _contentRepository;
	private readonly ICategoryRepository _categoryRepository;

	public PostContentValidator(IContentRepository contentRepository, ICategoryRepository categoryRepository)
	{
		_contentRepository = contentRepository;
		_categoryRepository = categoryRepository;

		RuleFor(key => key.Argument.Title).NotEmpty().WithMessage("Title is required.").Must(UniqueTitle).WithMessage("Title already used.");
		RuleFor(key => key.Argument.Description).NotEmpty().WithMessage("Description is required");
		RuleFor(key => key.Argument.Article).NotEmpty().WithMessage("Article is required");

		When(prop => !string.IsNullOrEmpty(prop.Argument.Category_Id), () =>
		{
			RuleFor(key => key.Argument.Category_Id).Must(ValidCategory).WithMessage("Category is not valid");
		});
	}

	private bool UniqueTitle(string title)
	{
		return _contentRepository.VerifyAvailableTitle(title);
	}

	private bool ValidCategory(string? categoryId)
	{
		bool isValidId = Guid.TryParse(categoryId, out Guid result);
		if (!isValidId) return false;
		return _categoryRepository.VerifyValidCategory(result);
	}
}

public class PostContentHandler : ICommandHandler<PostContentCommand>
{
	private readonly IContentRepository _contentRepository;
	private readonly IValidator<PostContentCommand> _validator;

	public PostContentHandler(IContentRepository contentRepository, IValidator<PostContentCommand> validator)
	{
		_contentRepository = contentRepository;
		_validator = validator;
	}

	public async Task<Result> Handle(PostContentCommand request, CancellationToken cancellationToken)
	{
		var validation = await _validator.ValidateAsync(request, cancellationToken);

		if (!validation.IsValid)
		{
			var errorMsg = validation.Errors.First()?.ErrorMessage;
			return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
		}

		var postContent = new EntityPostContent()
		{
			Action = request.Argument.Action,
			Title = request.Argument.Title,
			Description_Html = request.Argument.Description_Html,
			Description = request.Argument.Description,
			Article = request.Argument.Article,
			Additional_Link = request.Argument.Additional_Link,
			Category = string.IsNullOrEmpty(request.Argument.Category_Id) ? null : Guid.Parse(request.Argument.Category_Id),
			Create_By = request.Argument.Create_By
		};

		await _contentRepository.PostContentAsync(postContent, cancellationToken);
		return Result.Success();
	}
}
