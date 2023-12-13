using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Models;
using PasswordManager.RepositoryManager;

namespace PasswordManager.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : class
    {
        private readonly IGenericRepository<T> repository;

        public NotFoundFilter(IGenericRepository<T> repository)
        {
            this.repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments.Values.FirstOrDefault();

            if (idValue == null)
            {
                await next.Invoke();
                return;
            }

            var id = (int)idValue;
            var anyEntity = await repository.GetByIdAsync(id);

            if (anyEntity != null)
            {
                await next.Invoke();
                return;
            }

            var errorViewModel = new ErrorViewModel();
            errorViewModel.Errors.Add($"{typeof(T).Name}({id}) not found");

            context.Result = new RedirectToActionResult("Error", "Home", errorViewModel);
        }
    }
}
