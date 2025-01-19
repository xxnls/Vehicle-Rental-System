namespace API.Interfaces
{
    /// <summary>
    /// Represents a model with base properties, such as <i>CreatedDate</i>, <i>ModifiedDate</i>, <i>DeletedDate</i>, and <i>IsActive</i>.
    /// </summary>
    public interface IBaseModel
    {
        DateTime CreatedDate { get; set; }
        DateTime? ModifiedDate { get; set; }
        DateTime? DeletedDate { get; set; }
        bool IsActive { get; set; }
    }
}
