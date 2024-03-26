using FluentValidation;
using MuratBaloglu.Application.Models.Blogs;

namespace MuratBaloglu.Application.Validators.Blogs
{
    public class BlogCreateModelValidator : AbstractValidator<BlogAddModel>
    {
        public BlogCreateModelValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lutfen Title alanini bos gecmeyiniz");
        }
    }
}
