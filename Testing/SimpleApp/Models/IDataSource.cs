using System.Collections;

namespace SimpleApp.Models
{
    public interface IDataSource
    {
        IEnumerable<Product> Products { get; }
    }
}
