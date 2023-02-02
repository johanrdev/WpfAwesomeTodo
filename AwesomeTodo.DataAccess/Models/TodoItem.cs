using Prism.Mvvm;

namespace AwesomeTodo.DataAccess.Models
{
    public class TodoItem : BindableBase
    {
        private int _id;
        private string _title;

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
