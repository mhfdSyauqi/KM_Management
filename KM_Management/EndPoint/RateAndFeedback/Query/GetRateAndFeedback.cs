using FluentValidation;
using KM_Management.Commons.Mediator;
using KM_Management.EndPoint.RateAndFeedback.Models;
using KM_Management.Shared;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;

namespace KM_Management.EndPoint.RateAndFeedback.Query;

public record GetRateAndFeedbackQuery(RequestRateAndFeedback Argument) : IQuery<ResponseRateAndFeedback>;

public class GetRateAndFeedbackValidator : AbstractValidator<GetRateAndFeedbackQuery>
{
    public GetRateAndFeedbackValidator()
    {

    }
}

public class GetRateAndFeedbackHandler : IQueryHandler<GetRateAndFeedbackQuery, ResponseRateAndFeedback>
{
    private readonly IRateAndFeedbackRepository _rateAndFeedbackRepository;
    private readonly IValidator<GetRateAndFeedbackQuery> _validator;

    public GetRateAndFeedbackHandler(IRateAndFeedbackRepository rateAndFeedbackRepository, IValidator<GetRateAndFeedbackQuery> validator)
    {
        _rateAndFeedbackRepository = rateAndFeedbackRepository;
        _validator = validator;
    }

    public async Task<Result<ResponseRateAndFeedback>> Handle(GetRateAndFeedbackQuery request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var filterFeedback = new FilterRateAndFeedback()
        {
            Filter_Date = request.Argument.Filter_Date,
            Start_Date = request.Argument.Start_Date,
            End_Date = request.Argument.End_Date,
            Rating = string.Join(",", request.Argument.Rating),
            Page_Limit = request.Argument.Page_Limit,
            Current_Page = request.Argument.Current_Page,
        };

        var filterSummary = new FilterRateAndFeedbackSummary()
        {
            Filter_Date = request.Argument.Filter_Date,
            Start_Date = request.Argument.Start_Date,
            End_Date = request.Argument.End_Date,
        };

        var feedback = await _rateAndFeedbackRepository.GetRateAndFeedbackAsync(filterFeedback, cancellationToken);
        var summary = await _rateAndFeedbackRepository.GetRateAndFeedbackSummaryAsync(filterSummary, cancellationToken);

        var response = new ResponseRateAndFeedback()
        {
            Periode = feedback.FirstOrDefault().Periode,
            Summary =  new Summary_User_Feedback()
            {
                User_Preview = summary.User_Preview,
                Total_Feedback = summary.Total_Feedback,
                Overall_Rating = summary.Overall_Rating
            },
            Items = feedback.Select(col => new User_Feedback()
            {
                Rating = col.Rating,
                Remark = col.Remark,
                Uid_Session_Header = col.Uid_Session_Header.ToString("N"),
                Create_By = col.Create_By,
                Create_At = col.Create_At,
                Total_Category = col.Total_Category,
            }).ToList(),
            Total_Row = feedback.FirstOrDefault()?.Total_Row,
            Curr_Page = feedback.FirstOrDefault()?.Curr_Page,
            Next_Page = feedback.FirstOrDefault()?.Next_Page,
            Prev_Page = feedback.FirstOrDefault()?.Prev_Page,
            Max_Page = feedback.FirstOrDefault()?.Max_Page,
        };
        return Result.Success(response);
    }
}