using System.ComponentModel.DataAnnotations.Schema;

namespace SnackBar.DAL.Interfaces
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState
        {
            get; set;
        }
    }
}