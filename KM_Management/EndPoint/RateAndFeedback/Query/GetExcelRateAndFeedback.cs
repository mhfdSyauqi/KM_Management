using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.RateAndFeedback.Models;
using KM_Management.Shared;

namespace KM_Management.EndPoint.RateAndFeedback.Query
{
    public record GetExcelRateAndFeedbackQuery(RequestExcelRateAndFeedback Argument) : IQuery<List<ResponseExcelRateAndFeedback>>;

    public class GetExcelRateAndFeedbackValidator : AbstractValidator<GetExcelRateAndFeedbackQuery>
    {
        public GetExcelRateAndFeedbackValidator()
        {

        }
    }




    public class GetExcelRateAndFeedbackHandler : IQueryHandler<GetExcelRateAndFeedbackQuery, List<ResponseExcelRateAndFeedback>>
    {

        private readonly IRateAndFeedbackRepository _rateAndFeedbackRepository;
        private readonly IValidator<GetExcelRateAndFeedbackQuery> _validator;

        public GetExcelRateAndFeedbackHandler(IRateAndFeedbackRepository rateAndFeedbackRepository, IValidator<GetExcelRateAndFeedbackQuery> validator)
        {
            _rateAndFeedbackRepository = rateAndFeedbackRepository;
            _validator = validator;
        }

        public async Task<Result<List<ResponseExcelRateAndFeedback>>> Handle(GetExcelRateAndFeedbackQuery request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var filterFeedback = new FilterExcelRateAndFeedback()
            {
                Filter_Date = request?.Argument?.Filter_Date,
                Start_Date = request?.Argument?.Start_Date,
                End_Date = request?.Argument?.End_Date,
                Rating = string.Join(",", request?.Argument.Rating)
            };

            var feedback = await _rateAndFeedbackRepository.GetExcelRateAndFeedbackAsync(filterFeedback, cancellationToken);

            var response = feedback.Select(col => new ResponseExcelRateAndFeedback() 
            {
                Rating = col?.Rating ?? 0,
                Remark = col?.Remark ?? "",
                Uid_Session_Header = col?.Uid_Session_Header.ToString() ?? default,
                Create_By = col?.Create_By ?? "",
                Create_At = col?.Create_At ?? default,
                Total_Category = col?.Total_Category ?? 0,
                Periode = col?.Periode ?? "",
            }).ToList();

            return Result.Success(response);

        }
    }
}
