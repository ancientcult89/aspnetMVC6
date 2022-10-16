using Advanced.Models;
using DataModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Advanced.Blazor.Forms
{
    public class DeptStateValidator : OwningComponentBase<DataContext>
    {
        public DataContext DataContext => Service;

        [Parameter]
        public long DepartmentId { get; set; }

        [Parameter]
        public string? State { get; set; }

        [CascadingParameter]
        public EditContext? CurrentEditContext { get; set; }

        private string? DeptName { get; set; }
        private Dictionary<long, string> LocationStates { get; set; }
        protected override void OnInitialized()
        {
            if (CurrentEditContext != null)
            {
                ValidationMessageStore store = new ValidationMessageStore(CurrentEditContext);
                CurrentEditContext.OnFieldChanged += (sender, args) =>
                {
                    string name = args.FieldIdentifier.FieldName;
                    if (name == "DepartmentId" || name == "LocationId")
                        Validate(CurrentEditContext.Model as Person, store);
                };
            }
        }

        protected override void OnParametersSet()
        {
            DeptName = DataContext.Departments.Find(DepartmentId)?.Name;
            LocationStates = DataContext.Locations.ToDictionary(l => l.LocationId, l => l.State);
        }

        private void Validate(Person? model, ValidationMessageStore store)
        {
            if (model?.DepartmentId == DepartmentId && LocationStates != null && CurrentEditContext != null
                && (!LocationStates.ContainsKey(model.LocationId) || LocationStates[model.LocationId] != State))
            {
                store.Add(CurrentEditContext.Field("LocationId"), $"{DeptName} stuff must be in: {State}");
            }
            else
                store.Clear();
            CurrentEditContext?.NotifyValidationStateChanged();
        }
    }
}
