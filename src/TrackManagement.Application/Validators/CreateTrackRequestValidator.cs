using FluentValidation;
using TrackManagement.Application.DTOs;

namespace TrackManagement.Application.Validators;

public class CreateTrackRequestValidator : AbstractValidator<CreateTrackRequest>
{
    public CreateTrackRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.ArtistId)
            .NotEmpty()
            .WithMessage("ArtistId is required.");

        RuleFor(x => x.Isrc)
            .NotEmpty()
            .Matches("^[A-Z]{2}[A-Z0-9]{3}[0-9]{7}$")
            .WithMessage("Isrc must be a valid 12-character ISRC code (e.g. USRC17607839).");

        RuleFor(x => x.ReleaseDate)
            .NotEqual(default(DateOnly))
            .WithMessage("ReleaseDate is required.");

        RuleFor(x => x.Genre)
            .NotEmpty()
            .MaximumLength(100);
    }
}
