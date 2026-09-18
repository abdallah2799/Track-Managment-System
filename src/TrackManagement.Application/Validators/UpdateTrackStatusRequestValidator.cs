using FluentValidation;
using TrackManagement.Application.DTOs;

namespace TrackManagement.Application.Validators;

public class UpdateTrackStatusRequestValidator : AbstractValidator<UpdateTrackStatusRequest>
{
    public UpdateTrackStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum();
    }
}
