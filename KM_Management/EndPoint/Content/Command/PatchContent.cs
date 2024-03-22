using FluentValidation;
using KM_Management.Commons.FluentExtention;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.Category;
using KM_Management.EndPoint.Content.Models;
using KM_Management.Shared;
using System.Net;

namespace KM_Management.EndPoint.Content.Command;

public record PatchContentCommand(RequestPatchContent Argument) : ICommand;


public class PatchContentValidator : AbstractValidator<PatchContentCommand>
{
	private readonly IContentRepository _contentRepository;
	private readonly ICategoryRepository _categoryRepository;

	public PatchContentValidator(IContentRepository contentRepository, ICategoryRepository categoryRepository)
	{
		_contentRepository = contentRepository;
		_categoryRepository = categoryRepository;

		RuleFor(key => key.Argument.Id).BeValidGuid();
		RuleFor(key => key.Argument.Title).NotEmpty().WithMessage("Title is required");
		RuleFor(key => key.Argument).Must((content) => UniqueTitle(content.Title, content.Id)).WithMessage("Title already used");
		RuleFor(key => key.Argument.Description).NotEmpty().WithMessage("Description is required");
		RuleFor(key => key.Argument.Article).NotEmpty().WithMessage("Article is required");

		When(prop => !string.IsNullOrEmpty(prop.Argument.Category_Id), () =>
		{
			RuleFor(key => key.Argument.Category_Id).Must(ValidCategory).WithMessage("Category is not valid");
		});
	}

	private bool UniqueTitle(string title, string id)
	{
		bool isValidId = Guid.TryParse(id, out Guid result);
		if (!isValidId) return false;
		return _contentRepository.VerifyAvailableTitle(title, result);
	}

	private bool ValidCategory(string? categoryId)
	{
		bool isValidId = Guid.TryParse(categoryId, out Guid result);
		if (!isValidId) return false;
		return _categoryRepository.VerifyValidCategory(result);
	}
}

public class PatchContentHandler : ICommandHandler<PatchContentCommand>
{
	private readonly IContentRepository _contentRepository;
	private readonly IValidator<PatchContentCommand> _validator;

	public PatchContentHandler(IContentRepository contentRepository, IValidator<PatchContentCommand> validator)
	{
		_contentRepository = contentRepository;
		_validator = validator;
	}

	public async Task<Result> Handle(PatchContentCommand request, CancellationToken cancellationToken)
	{
		var validation = await _validator.ValidateAsync(request, cancellationToken);

		if (!validation.IsValid)
		{
			var errorMsg = validation.Errors.First()?.ErrorMessage;
			return Result.Failure(new(HttpStatusCode.BadRequest, errorMsg));
		}

		var patchContent = new EntityPatchContent()
		{
			Action = request.Argument.Action,
			Id = Guid.Parse(request.Argument.Id),
			Title = request.Argument.Title,
			Description_Html = request.Argument.Description_Html,
			Description = request.Argument.Description,
			Article = request.Argument.Article,
			Additional_Link = request.Argument.Additional_Link,
			Category = string.IsNullOrEmpty(request.Argument.Category_Id) ? null : Guid.Parse(request.Argument.Category_Id),
			Modified_By = request.Argument.Modified_By
		};

		await _contentRepository.PatchContentAsync(patchContent, cancellationToken);
		return Result.Success();
	}
}