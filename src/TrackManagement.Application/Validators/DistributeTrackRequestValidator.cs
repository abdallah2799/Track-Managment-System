using FluentValidation;
using TrackManagement.Application.DTOs;

namespace TrackManagement.Application.Validators;

public class DistributeTrackRequestValidator : AbstractValidator<DistributeTrackRequest>
{
    public DistributeTrackRequestValidator()
    {
        RuleFor(x => x.DspIds)
            .NotEmpty()
            .WithMessage("At least one DspId must be provided.");

        RuleForEach(x => x.DspIds)
            .NotEmpty()
            .WithMessage("DspIds must not contain empty ids.");

        RuleFor(x => x.DspIds)
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("DspIds must not contain duplicates.")
            .When(x => x.DspIds is { Count: > 0 });
    }
}
